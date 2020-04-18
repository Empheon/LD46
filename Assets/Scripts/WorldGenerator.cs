using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject Terrier;
    public int TerrierNumber;

    private const float m_boundX = 80-10;
    private const float m_boundY = 60-10;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < TerrierNumber; i++)
        {
            Instantiate(Terrier, new Vector3(Random.Range(-m_boundX, m_boundX), Random.Range(-m_boundY, m_boundY), 0), Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
