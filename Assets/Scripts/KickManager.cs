using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickManager : MonoBehaviour
{
    private PlayerController m_playerController;
    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ts = collision.GetComponent<TerrierSpawner>();
        if (ts != null)
        {
            m_playerController.CollidingTerrier = ts;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<TerrierSpawner>() != null)
        {
            m_playerController.CollidingTerrier = null;
        }
    }
}
