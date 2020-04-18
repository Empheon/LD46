using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBall : MonoBehaviour
{
    public static float Speed = 5;
    private Vector3 m_direction;

    public List<CoalEater> Predators;
    private bool m_isUnregistered = false;

    // Start is called before the first frame update
    void Start()
    {
        Predators = new List<CoalEater>();
        WorldGenerator.Instance.EatableCoalList.Add(gameObject);
        m_direction = Vector3.Normalize(FireManager.GOInstance.transform.position - transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(m_direction * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightFire"))
        {
            Unregister();
        }

        //FireManager fm = collision.gameObject.GetComponent<FireManager>();
        if (collision.gameObject.CompareTag("Fire"))
        {
            collision.gameObject.GetComponent<FireManager>().GrowFire();
            DestroyBall();
        }
    }

    private void Unregister()
    {
        WorldGenerator.Instance.EatableCoalList.Remove(gameObject);
        foreach (var p in Predators)
        {
            p.CoalBallDisappear();
        }
        m_isUnregistered = true;
    }

    public void DestroyBall()
    {
        if (!m_isUnregistered)
        {
            Unregister();
        }

        Destroy(gameObject);
    }

    public void RegisterPredator(CoalEater predator)
    {
        Predators.Add(predator);
    }
}
