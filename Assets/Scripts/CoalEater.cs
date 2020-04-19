using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEater : Eater
{
    private GameObject m_currentFocusObject;

    private float m_refocusFrequency = 1;
    private float m_timerRefocus;
    private float m_shutdownTerrierFrequency = 8;
    private float m_timerShutdown;
    private bool m_isDying = false;

    private bool m_focusOnTerrier = false;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldGenerator.Instance.EatableCoalList.Count == 0)
        {
            m_currentFocusObject = FindClosestCoalBall();
            m_currentFocusObject.GetComponent<CoalBall>().RegisterPredator(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((WorldGenerator.Instance.PlayerInstance.transform.position - transform.position).sqrMagnitude > 10000)
        {
            Destroy(gameObject);
        }
        if (m_isDying)
        {
            return;
        }


        if (WorldGenerator.Instance.EatableCoalList.Count == 0 && m_currentFocusObject == null)
        {
            // No ball anymore? Let's make it disappear for now
            Die(new Vector3(0, 0, 0));
            return;
        }

        if (m_focusOnTerrier && (m_currentFocusObject == null || m_currentFocusObject.GetComponent<TerrierSpawner>().Status == TerrierState.SLEEPING))
        {
            //if (m_currentFocusObject.GetComponent<TerrierSpawner>().Status == TerrierState.SLEEPING)
            //{
            m_focusOnTerrier = false;
            //}
        }

        if (!m_focusOnTerrier)
        {
            CoalBall cb = m_currentFocusObject == null ? null : m_currentFocusObject.GetComponent<CoalBall>();

            if (m_currentFocusObject != null && cb != null && m_timerRefocus > m_refocusFrequency)
            {
                m_currentFocusObject.GetComponent<CoalBall>().UnregisterPredator(this);
                m_currentFocusObject = null;
                m_timerRefocus = 0;
            } else
            {
                m_timerRefocus += Time.deltaTime;
            }

            if (m_currentFocusObject == null || cb == null)
            {
                m_currentFocusObject = FindClosestCoalBall();
                m_currentFocusObject.GetComponent<CoalBall>().RegisterPredator(this);
            }
        }

        if (m_timerShutdown > m_shutdownTerrierFrequency)
        {
            m_focusOnTerrier = true;
            if (m_currentFocusObject != null)
            {
                var cb = m_currentFocusObject.GetComponent<CoalBall>();
                if (cb != null)
                {
                    cb.GetComponent<CoalBall>().UnregisterPredator(this);
                }
            }
            m_currentFocusObject = FindClosestTerrier();
            m_timerShutdown = 0;
        } else
        {
            m_timerShutdown += Time.deltaTime;
        }


        MoveTowardFocusObject();
    }

    private GameObject FindClosestTerrier()
    {
        GameObject closestTerrier = null;
        float min = float.MaxValue;
        foreach (var terrierObj in TerrierSpawner.AwokeTerriers)
        {
            var sqrDist = (terrierObj.transform.position - transform.position).sqrMagnitude;
            if (closestTerrier == null || sqrDist < min)
            {
                closestTerrier = terrierObj.gameObject;
                min = sqrDist;
            }
        }
        return closestTerrier;
    }

    private void MoveTowardFocusObject()
    {
        var direction = Vector3.Normalize(m_currentFocusObject.transform.position - transform.position);
        transform.Translate(direction * Speed * Time.deltaTime);

        if (direction.x > 0)
        {
            MonsterSprite.flipX = false;
        } else
        {
            MonsterSprite.flipX = true;
        }
    }

    private GameObject FindRandomCoalBall()
    {
        return WorldGenerator.Instance.EatableCoalList[Random.Range(0, WorldGenerator.Instance.EatableCoalList.Count)];
    }

    private GameObject FindClosestCoalBall()
    {
        GameObject closestBall = null;
        float min = float.MaxValue;
        foreach (var coalObj in WorldGenerator.Instance.EatableCoalList)
        {
            var sqrDist = (coalObj.transform.position - transform.position).sqrMagnitude;
            if (closestBall == null || sqrDist < min)
            {
                closestBall = coalObj;
                min = sqrDist;
            }
        }
        return closestBall;
    }

    public void CoalBallDisappear()
    {
        m_currentFocusObject = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var cb = collision.gameObject.GetComponent<CoalBall>();
        if (cb != null)
        {
            cb.DestroyBall();
        }

        var ts = collision.gameObject.GetComponent<TerrierSpawner>();
        if (ts != null && ts == m_currentFocusObject.GetComponent<TerrierSpawner>())
        {
            ts.PutToSleep();
            m_currentFocusObject = null;
        }
    }

    public override void Die(Vector3 playerPosition)
    {
        base.Die(playerPosition);
        m_isDying = true;
        if (m_currentFocusObject != null)
        {
            m_currentFocusObject.GetComponent<CoalBall>().UnregisterPredator(this);
        }
    }
}
