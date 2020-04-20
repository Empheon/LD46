using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainGenerator : MonoBehaviour
{
    
    public GameObject Bush1;
    public GameObject Bush2;
    public GameObject Bush3;
    public GameObject Stone1;
    public GameObject Stone2;
    public GameObject Stone3;
    public GameObject Stone4;
    public GameObject Leaf1;
    public GameObject Leaf2;
    public GameObject Leaf3;
    public GameObject Leaf4;
    public GameObject Leaf5;
    public GameObject Leaf6;
    public GameObject Flower;
    public GameObject Rock1;
    public GameObject Rock2;
    public GameObject Reed;
    public GameObject Tree1;
    public GameObject Tree2;
    public GameObject Tree3;
    public GameObject Tree4;
    public GameObject Tree5;
    public GameObject Tree6;
    public GameObject Trunk0;
    public GameObject Trunk1;
    public GameObject Trunk2;


    public int NumberTerrainObjects;
    private List<GameObject> AllTerrainObjects;

    private float m_boundX;
    private float m_boundY;



    // Start is called before the first frame update
    void Start()
    {
       

        
        
    }

    public void CreateTerrain(float m_boundX, float m_boundY){
        this.m_boundX = m_boundX;
        this.m_boundY = m_boundX;
        
         this.AllTerrainObjects = new List<GameObject>()
        {Bush1, Bush2, Bush3,
        Stone1, Stone2, Stone3, Stone4,
        Leaf1, Leaf2, Leaf3, Leaf4, Leaf5, Leaf6,
        Flower,
        Rock1, Rock2,
        Reed,
        Tree1, Tree2, Tree3, Tree4, Tree5, Tree6,
        Trunk0, Trunk1, Trunk2};


        
        for(int i = 0 ; i < NumberTerrainObjects ; i++){
            int chosenObject = Random.Range(0, AllTerrainObjects.Count);
            Instantiate(AllTerrainObjects[chosenObject], new Vector3(Random.Range(-m_boundX, m_boundX), Random.Range(-m_boundY, m_boundY), 0), Quaternion.identity, transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}