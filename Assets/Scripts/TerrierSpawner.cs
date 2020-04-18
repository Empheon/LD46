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

    // Start is called before the first frame update
    void Start()
    {
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
                    Instantiate(CoalBall, transform.position, Quaternion.identity);
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
}
