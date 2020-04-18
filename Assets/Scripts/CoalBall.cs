using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBall : MonoBehaviour
{
    public static float Speed = 5;
    private Vector3 m_direction;

    // Start is called before the first frame update
    void Start()
    {
        m_direction = Vector3.Normalize(FireManager.GOInstance.transform.position - transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(m_direction * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FireManager fm = collision.gameObject.GetComponent<FireManager>();
        if (fm != null)
        {
            fm.GrowFire();
            Destroy(gameObject);
        }
    }
}
