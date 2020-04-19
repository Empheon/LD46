using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MovingThing
{
    public float Speed = 5;
    public float ExplusionSpeed = 10;
    public float Torque = 60;
    public SpriteRenderer MonsterSprite;
    
    virtual public void Die(Vector3 playerPosition)
    {
        var rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.None;
        rigidbody.AddForce(Vector3.Normalize(transform.position - playerPosition) * ExplusionSpeed, ForceMode2D.Impulse);
        rigidbody.AddTorque(Torque);
        GetComponent<CapsuleCollider2D>().enabled = false;
        Speed = 0;
        Shadow.gameObject.SetActive(false);
    }
}
