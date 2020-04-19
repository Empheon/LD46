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

        var e = collision.GetComponent<Eater>();
        if (e != null)
        {
            m_playerController.CollidingEater.Add(e);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var ts = collision.GetComponent<TerrierSpawner>();
        if (ts != null)
        {
            m_playerController.CollidingTerrier.Remove(ts);
        }

        var e = collision.GetComponent<Eater>();
        if (e != null)
        {
            m_playerController.CollidingEater.Remove(e);
        }
    }
}
