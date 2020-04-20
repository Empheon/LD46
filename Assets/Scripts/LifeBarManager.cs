using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarManager : MonoBehaviour
{

    public GameObject heartFull;
    public GameObject heartEmpty;
    private List<GameObject> hearts;

    public Transform mainCamera;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void createBarLife(int health){
        this.health = health;
        hearts = new List<GameObject>();

        for(int i = 0 ; i < health ; i++){
            hearts.Add(Instantiate(heartFull, mainCamera.position + new Vector3(23 + i*2, 13, 10), Quaternion.identity, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0 ; i < health ; i++){
            hearts[i].transform.position = mainCamera.position + new Vector3(23 + i*2, 13, 10);
        }
    }
}
