using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrierState { SLEEPING, AWAKE }

public class TerrierSpawner : MonoBehaviour
{
    public GameObject CoalBall;
    public float SpawnFrequency = 1000;
    public SpriteRenderer Glow;
    public float GlowDuration = .5f;

    public TerrierState Status;
    private float m_counter;

    private static Queue<GameObject> m_ballPool;
    private Animator m_animator;
    private Color m_glowColor;
    private static Color m_transparent;

    public static List<TerrierSpawner> AwokeTerriers;

    // Start is called before the first frame update
    void Start()
    {
        AwokeTerriers = new List<TerrierSpawner>();
        m_animator = GetComponent<Animator>();
        if (m_ballPool == null)
        {
            m_ballPool = new Queue<GameObject>();
        }
        m_ballPool.Clear(); // Because of fast play mode, doesn't reload static vars
        Status = TerrierState.SLEEPING;
        m_counter = SpawnFrequency;

        m_glowColor = Glow.color;
        m_transparent = new Color(m_glowColor.r, m_glowColor.g, m_glowColor.b, 0.2f);
        Glow.color = m_transparent;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Status)
        {
            case TerrierState.AWAKE:
                if (m_counter < SpawnFrequency)
                {
                    m_counter += Time.deltaTime;
                } else
                {
                    m_counter = 0;
                    NewBallAt(transform.position);
                }
                break;
            case TerrierState.SLEEPING:
                break;
        }
    }

    public void Awaken()
    {
        if (Status == TerrierState.AWAKE)
        {
            return;
        }

        Status = TerrierState.AWAKE;
        StartCoroutine(GlowOn());
        if (WorldGenerator.Instance.PlayerInstance.transform.position.x < transform.position.x)
        {
            m_animator.SetTrigger("Left");
        } else
        {
            m_animator.SetTrigger("Right");
        }
        AwokeTerriers.Add(this);
    }

    public void PutToSleep()
    {
        if (Status == TerrierState.SLEEPING)
        {
            return;
        }

        Status = TerrierState.SLEEPING;
        StartCoroutine(GlowOff());
        AwokeTerriers.Remove(this);
    }

    private IEnumerator GlowOn()
    {
        float d = 0;
        while (d < GlowDuration)
        {
            d += Time.deltaTime;
            var r = Mathf.Lerp(m_transparent.r, m_glowColor.r, d / GlowDuration);
            var g = Mathf.Lerp(m_transparent.g, m_glowColor.g, d / GlowDuration);
            var b = Mathf.Lerp(m_transparent.b, m_glowColor.b, d / GlowDuration);
            var a = Mathf.Lerp(m_transparent.a, m_glowColor.a, d / GlowDuration);
            Glow.color = new Color(r, g, b, a);
            yield return null;
        }
    }

    private IEnumerator GlowOff()
    {
        float d = 0;
        while (d < GlowDuration)
        {
            d += Time.deltaTime;
            var r = Mathf.Lerp(m_glowColor.r, m_transparent.r, d / GlowDuration);
            var g = Mathf.Lerp(m_glowColor.g, m_transparent.g, d / GlowDuration);
            var b = Mathf.Lerp(m_glowColor.b, m_transparent.b, d / GlowDuration);
            var a = Mathf.Lerp(m_glowColor.a, m_transparent.a, d / GlowDuration);
            Glow.color = new Color(r, g, b, a);
            yield return null;
        }
    }

    private void NewBallAt(Vector3 pos)
    {
        if (m_ballPool.Count == 0)
        {
            Instantiate(CoalBall, pos, Quaternion.identity);
        } else
        {
            var gO = m_ballPool.Dequeue();
            gO.SetActive(true);
            gO.transform.position = pos;
            gO.GetComponent<CoalBall>().Init();
        }
    }

    public static void ReleaseBall(GameObject gO)
    {
        gO.SetActive(false);
        m_ballPool.Enqueue(gO);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightFire"))
        {
            AwokeTerriers.Remove(this);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightFire"))
        {
            if (!AwokeTerriers.Contains(this))
            {
                AwokeTerriers.Add(this);
            }
        }
    }
}
