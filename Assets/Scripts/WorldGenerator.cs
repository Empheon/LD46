using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<GameObject> Moons;
    public Sprite SunSprite;
    public float DayTime = 120;
    private float m_dayTimer = 0;
    private float m_nextStep;
    private float m_dayStep;
    private int m_moonIdx = 0;

    private readonly static float m_wallsDispertion = 28;
    public int NumberOfWallTrees = 600;

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
        m_dayStep = DayTime / 5; // Number of moons
        m_nextStep = m_dayStep;

        var positions = GenerateStuff(Terrier, 100, TerrierNumber);
        foreach (var h in HittableDecorations)
        {
            h.transform.localScale = Vector3.one * 3;
            positions = GenerateStuff(h, 40, 2, positions);
        }

        SpriteRenderer sr = TerrainTemplate.GetComponent<SpriteRenderer>();
        TerrainTemplate.transform.localScale = Vector3.one * 2.5f;
        sr.sortingOrder = -32700;
        foreach (var d in Decorations)
        {
            sr.sprite = d;
            positions = GenerateStuff(TerrainTemplate, 10, 4, positions, false);
        }

        // Walls
        Vector3 pos;
        var boundX = m_boundX + 2;
        var boundY = m_boundY + 2;
        for (int i = 0; i < NumberOfWallTrees / 2; i++)
        {
            pos = new Vector3(Random.Range(0, 2) == 0 ? Random.Range(-boundX - m_wallsDispertion, -boundX) :
                                Random.Range(boundX, boundX + m_wallsDispertion),
                                Random.Range(-boundY - m_wallsDispertion/2, boundY + m_wallsDispertion/2), 0);
            //pos += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0);
            var gO = Instantiate(HittableDecorations[Random.Range(0, HittableDecorations.Count)], pos, Quaternion.identity, transform);
            gO.GetComponent<CapsuleCollider2D>().enabled = false;
            gO.GetComponent<SpriteRenderer>().sortingOrder = (int)-(pos.y * 100);
        }

        for (int i = 0; i < NumberOfWallTrees / 2; i++)
        {
            //pos = new Vector3(Random.Range(-m_boundX, m_boundX), Random.Range(0, 2) == 0 ? -m_boundY : m_boundY, 0);
            pos = new Vector3(Random.Range(-boundX - m_wallsDispertion/2, boundX + m_wallsDispertion/2),
                                Random.Range(0, 2) == 0 ? Random.Range(-boundY - m_wallsDispertion, -boundY) :
                                Random.Range(boundY, boundY + m_wallsDispertion), 0);
            //pos += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f), 0);
            var gO = Instantiate(HittableDecorations[Random.Range(0, HittableDecorations.Count)], pos, Quaternion.identity, transform);
            gO.GetComponent<CapsuleCollider2D>().enabled = false;
            gO.GetComponent<SpriteRenderer>().sortingOrder = (int)-(pos.y * 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Day management
        m_dayTimer += Time.deltaTime;
        if (m_dayTimer > m_nextStep && m_moonIdx < 5)
        {
            Moons[m_moonIdx].GetComponent<Image>().sprite = SunSprite;
            m_nextStep += m_dayStep;
            m_moonIdx++;
            Debug.Log("Win");
        }

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

    private List<Vector3> GenerateStuff(GameObject obj, float interDistance, int numberOfObjects, List<Vector3> positions = null, bool setOrder = true)
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
            if (setOrder)
            {
                obj.GetComponent<SpriteRenderer>().sortingOrder = (int)-(pos.y * 100);
            }
            Instantiate(obj, pos, Quaternion.identity, transform);
        }
        return terriers;
    }
}
