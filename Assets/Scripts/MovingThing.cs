using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThing : MonoBehaviour
{
    public SpriteRenderer Shadow;
    public SpriteRenderer MainSprite;

    private float m_lateUpdateFrequency = .2f;
    private float m_lateUpdateTimer = 0;

    private void LateUpdate()
    {
        if (m_lateUpdateTimer > m_lateUpdateFrequency)
        {
            var order = (int)-(transform.position.y * 100);
            MainSprite.sortingOrder = order;
            Shadow.sortingOrder = order - 1;
        } else
        {
            m_lateUpdateTimer += Time.deltaTime;
        }
    }
}
