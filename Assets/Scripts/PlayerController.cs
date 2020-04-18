using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5;
    public BoxCollider2D KickCollider;
    public TerrierSpawner CollidingTerrier;
    public CoalEater CollidingCoalEater;
    public GameObject AnimatedObject;

    private Rigidbody2D m_rigidbody;
    private float m_kickColliderX;
    private SpriteRenderer m_playerSprite;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_kickColliderX = KickCollider.transform.position.x;
        m_playerSprite = AnimatedObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var hAxis = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(hAxis, Input.GetAxis("Vertical"), 0) * Time.deltaTime * Speed);

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

        if (Input.GetButtonDown("Kick") && CollidingTerrier != null)
        {
            CollidingTerrier.Awaken();
        }

        if (Input.GetButtonDown("Kick") && CollidingCoalEater != null)
        {
            CollidingCoalEater.Die(transform.position);
        }
    }
}
