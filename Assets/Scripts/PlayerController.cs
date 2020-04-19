using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovingThing
{
    public float Speed = 5;
    public BoxCollider2D KickCollider;
    public List<TerrierSpawner> CollidingTerrier;
    public List<Eater> CollidingEater;
    public GameObject AnimatedObject;
    public int HitPoints = 3;

    private Rigidbody2D m_rigidbody;
    private float m_kickColliderX;
    private SpriteRenderer m_playerSprite;
    private Animator m_playerAnimator;
    private bool m_isInvincible = false;
    private float m_blinkSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {
        CollidingTerrier = new List<TerrierSpawner>();
        CollidingEater = new List<Eater>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_kickColliderX = 2.20f;
        m_playerSprite = AnimatedObject.GetComponent<SpriteRenderer>();
        m_playerAnimator = AnimatedObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HitPoints <= 0)
        {
            Debug.Log("Game over no health");
        }
        var hAxis = Input.GetAxis("Horizontal");
        var vAxis = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(hAxis, vAxis, 0) * Time.deltaTime * Speed);
        m_playerAnimator.SetFloat("Velocity", hAxis * hAxis + vAxis * vAxis);

        if (Mathf.Abs(hAxis) > 0.00001)
        {
            KickCollider.transform.localPosition = new Vector3((hAxis > 0 ? 1 : -1) * Mathf.Abs(m_kickColliderX), 0, 0);
            if (hAxis > 0)
            {
                m_playerSprite.flipX = false;
            } else
            {
                m_playerSprite.flipX = true;
            }
        }

        if (Input.GetButtonDown("Kick") && CollidingTerrier.Count > 0)
        {
            CollidingTerrier.ForEach(x => x.Awaken());
            m_playerAnimator.SetTrigger("Kick");
        }

        if (Input.GetButtonDown("Kick") && CollidingEater.Count > 0)
        {
            m_playerAnimator.SetTrigger("Punch");
            CollidingEater.ForEach(x => x.Die(transform.position));
            CollidingEater.Clear();
        } else if (Input.GetButtonDown("Kick"))
        {
            m_playerAnimator.SetTrigger("Kick");
        }
    }

    public void LooseHP(int hp = 1)
    {
        if (!m_isInvincible)
        {
            HitPoints -= hp;
            m_isInvincible = true;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 8; i++)
        {
            m_playerSprite.color = new Color(1, .27f, .27f);
            yield return new WaitForSeconds(m_blinkSpeed);
            m_playerSprite.color = Color.white;
            yield return new WaitForSeconds(m_blinkSpeed);
        }
        m_isInvincible = false;
    }
}
