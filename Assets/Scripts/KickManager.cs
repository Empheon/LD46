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
            m_playerController.CollidingTerrier.Add(ts);
        }

        var ce = collision.GetComponent<CoalEater>();
        if (ce != null)
        {
            m_playerController.CollidingCoalEater.Add(ce);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ts = collision.GetComponent<TerrierSpawner>();
        if (ts != null)
        {
            m_playerController.CollidingTerrier.Remove(ts);
        }

        var ce = collision.GetComponent<CoalEater>();
        if (ce != null)
        {
            m_playerController.CollidingCoalEater.Remove(ce);
        }
    }
}
