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

    private const float m_boundX = 80 - 10;
    private const float m_boundY = 60 - 10;
    private const float m_boundAroundFire = 15;
    private float m_coalEaterTimer;
    private float m_manEaterTimer;

    public TerrainGenerator terrainGenerator;

    public static WorldGenerator Instance;

    public List<GameObject> HittableDecorations;
    public List<Sprite> Decorations;
    public GameObject TerrainTemplate;

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

        

        var positions = GenerateStuff(Terrier, 100, TerrierNumber);
        foreach (var h in HittableDecorations)
        {
            h.transform.localScale = Vector3.one * 3;
            positions = GenerateStuff(h, 40, 2, positions);
        }

        SpriteRenderer sr = TerrainTemplate.GetComponent<SpriteRenderer>();
        TerrainTemplate.transform.localScale = Vector3.one * 2.5f;
        foreach (var d in Decorations)
        {
            sr.sprite = d;
            positions = GenerateStuff(TerrainTemplate, 10, 4, positions);
            sr.sortingOrder = -32700;
        }

        //terrainGenerator.CreateTerrain(m_boundX, m_boundY);
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn CoalEater
        if (m_coalEaterTimer < CoalEaterSpawnFrequency + Random.Range(-1f, 3f))
        {
            m_coalEaterTimer += Time.deltaTime;
        } else if (EatableCoalList.Count != 0)
        {
            m_coalEaterTimer = 0;

            Instantiate(CoalEater, GeneratePosition(), Quaternion.identity);
        }

        // Spawn ManEater
        if (m_manEaterTimer < ManEaterSpawnFrequency + Random.Range(-1f, 3f))
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

    private List<Vector3> GenerateStuff(GameObject obj, float interDistance, int numberOfObjects, List<Vector3> positions = null)
    {
        var squaredFireBound = m_boundAroundFire * m_boundAroundFire;
        List<Vector3> terriers;
        if (positions == null)
        {
            terriers = new List<Vector3>();
        } else
        {
            terriers = positions;
        }
        var pos = Vector3.zero;
        bool isValid;
        float x, y;
        for (int i = 0; i < numberOfObjects; i++)
        {
            isValid = false;
            while (!isValid)
            {
                isValid = true;
                x = Random.Range(-m_boundX, m_boundX);
                y = Random.Range(-m_boundY, m_boundY);

                while (new Vector2(x, y).sqrMagnitude < squaredFireBound)
                {
                    x = Random.Range(-m_boundX, m_boundX);
                    y = Random.Range(-m_boundY, m_boundY);
                }

                pos = new Vector3(x, y, 0);

                foreach (var v in terriers)
                {
                    if ((v - pos).sqrMagnitude < interDistance)
                    {
                        isValid = false;
                    }
                }
            }
            terriers.Add(pos);
            obj.GetComponent<SpriteRenderer>().sortingOrder = (int)-(pos.y * 100);
            Instantiate(obj, pos, Quaternion.identity, transform);
        }
        return terriers;
    }
}
