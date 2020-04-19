using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEater : Eater
{
    private GameObject m_currentCoalBall;

    private float m_refocusTimer = 1;
    private float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        if (WorldGenerator.Instance.EatableCoalList.Count == 0)
        {
            m_currentCoalBall = FindClosestCoalBall();
            m_currentCoalBall.GetComponent<CoalBall>().RegisterPredator(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((WorldGenerator.Instance.PlayerInstance.transform.position - transform.position).sqrMagnitude > 10000)
        {
            Destroy(gameObject);
        }
        if (WorldGenerator.Instance.EatableCoalList.Count == 0)
        {
            // No ball anymore? Let's make it disappear for now
            Destroy(gameObject);
            return;
        }

        if (m_currentCoalBall != null && m_timer < m_refocusTimer)
        {
            m_currentCoalBall.GetComponent<CoalBall>().UnregisterPredator(this);
            m_currentCoalBall = null;
            m_timer = 0;
        } else
        {
            m_timer += Time.deltaTime;
        }

        if (m_currentCoalBall == null)
        {
            m_currentCoalBall = FindClosestCoalBall();
            m_currentCoalBall.GetComponent<CoalBall>().RegisterPredator(this);
        }

        var direction = Vector3.Normalize(m_currentCoalBall.transform.position - transform.position);
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
        m_currentCoalBall = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var cb = collision.gameObject.GetComponent<CoalBall>();
        if (cb != null)
        {
            cb.DestroyBall();
        }

        if (collision.gameObject.CompareTag("LightFire"))
        {
            Destroy(gameObject);
        }
    }
}
