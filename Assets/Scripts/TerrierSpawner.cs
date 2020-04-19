using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrierState { SLEEPING, AWAKE }

public class TerrierSpawner : MonoBehaviour
{
    public GameObject CoalBall;
    public float SpawnFrequency = 1000;

    private TerrierState m_state;
    private float m_counter;

    private static Queue<GameObject> m_ballPool;

    // Start is called before the first frame update
    void Start()
    {
        if (m_ballPool == null)
        {
            m_ballPool = new Queue<GameObject>();
        }
        m_state = TerrierState.SLEEPING;
        m_counter = SpawnFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case TerrierState.AWAKE:
                if (m_counter < SpawnFrequency)
                {
                    m_counter += Time.deltaTime;
                } else
                {
                    m_counter = 0;
                    NewBallAt(transform.position);
                }
                break;
            case TerrierState.SLEEPING:
                break;
        }
    }

    public void Awaken()
    {
        // TODO change sprite
        m_state = TerrierState.AWAKE;
    }

    public void PutToSleep()
    {
        m_state = TerrierState.SLEEPING;
    }

    private void NewBallAt(Vector3 pos)
    {
        if (m_ballPool.Count == 0)
        {
            Instantiate(CoalBall, pos, Quaternion.identity);
        } else
        {
            var gO = m_ballPool.Dequeue();
            gO.SetActive(true);
            gO.transform.position = pos;
            gO.GetComponent<CoalBall>().Init();
        }
    }

    public static void ReleaseBall(GameObject gO)
    {
        gO.SetActive(false);
        m_ballPool.Enqueue(gO);
    }
}
