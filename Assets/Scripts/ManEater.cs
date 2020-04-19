using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManEater : Eater
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((WorldGenerator.Instance.PlayerInstance.transform.position - transform.position).sqrMagnitude > 10000)
        {
            Destroy(gameObject);
        }

        var direction = Vector3.Normalize(WorldGenerator.Instance.PlayerInstance.transform.position - transform.position);
        transform.Translate(direction * Speed * Time.deltaTime);

        if (direction.x > 0)
        {
            MonsterSprite.flipX = false;
        } else
        {
            MonsterSprite.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.LooseHP();
        }
    }
}
