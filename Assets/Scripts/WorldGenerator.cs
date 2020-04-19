using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject Terrier;
    public GameObject PlayerInstance;
    public int TerrierNumber;
    public float CoalEaterSpawnFrequency = 5;
    public float ManEaterSpawnFrequency = 10;

    public List<GameObject> EatableCoalList;
    public GameObject CoalEater;
    public GameObject ManEater;

    private const float m_boundX = 80-10;
    private const float m_boundY = 60-10;
    private float m_coalEaterTimer;
    private float m_manEaterTimer;

    public static WorldGenerator Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EatableCoalList = new List<GameObject>();
        for (int i = 0; i < TerrierNumber; i++)
        {
            Instantiate(Terrier, new Vector3(Random.Range(-m_boundX, m_boundX), Random.Range(-m_boundY, m_boundY), 0), Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn CoalEater
        if (m_coalEaterTimer < CoalEaterSpawnFrequency + Random.Range(-1f,5f))
        {
            m_coalEaterTimer += Time.deltaTime;
        } else if (EatableCoalList.Count != 0)
        {
            m_coalEaterTimer = 0;
            
            Instantiate(CoalEater, GeneratePosition(), Quaternion.identity);
        }

        // Spawn ManEater
        if (m_manEaterTimer < ManEaterSpawnFrequency + Random.Range(-1f, 5f))
        {
            m_manEaterTimer += Time.deltaTime;
        } else
        {
            m_manEaterTimer = 0;

            Instantiate(ManEater, GeneratePosition(), Quaternion.identity);
        }
    }

    private Vector3 GeneratePosition()
    {
        float x, y;
        if (Random.Range(0f, 1f) > 0.5)
        {
            x = Random.Range(0f, 1f) > 0.5 ? -m_boundX - 20 : m_boundX + 20;
            y = Random.Range(-m_boundY - 20, m_boundY + 20);
        } else
        {
            y = Random.Range(0f, 1f) > 0.5 ? -m_boundY - 20 : m_boundY + 20;
            x = Random.Range(-m_boundX - 20, m_boundX + 20);
        }
        return new Vector3(x, y, 0);
    }
}
