using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalEater : MonoBehaviour
{
    public float Speed = 5;
    public float ExplusionSpeed = 10;
    public float Torque = 60;
    public SpriteRenderer MonsterSprite;
    private GameObject m_currentCoalBall;


    // Start is called before the first frame update
    void Start()
    {
        if (WorldGenerator.Instance.EatableCoalList.Count == 0)
        {
            m_currentCoalBall = FindRandomCoalBall();
            m_currentCoalBall.GetComponent<CoalBall>().RegisterPredator(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x) + Mathf.Abs(transform.position.y) > 200)
        {
            Destroy(gameObject);
        }
        if (WorldGenerator.Instance.EatableCoalList.Count == 0)
        {
            // No ball anymore? Let's make it disappear for now
            Destroy(gameObject);
            return;
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

    public void Die(Vector3 playerPosition)
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.None;
        rigidbody.AddForce(Vector3.Normalize(transform.position - playerPosition) * ExplusionSpeed, ForceMode2D.Impulse);
        rigidbody.AddTorque(Torque);
        GetComponent<CapsuleCollider2D>().enabled = false;
        Speed = 0;
    }
}
