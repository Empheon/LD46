using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public float GrowFactor = 0.1f;
    public float ReduceFactor = 0.01f; // percent lost
    public float GameOverThreshold = 0.005f;

    public float MaxSize = 1.5f;

    public static GameObject GOInstance;

    private void Awake()
    {
        if (GOInstance == null)
        {
            GOInstance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (transform.localScale.x < GameOverThreshold)
        {
            Debug.Log("Game over");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScaleFire(-ReduceFactor * transform.localScale.x);
    }

    public void GrowFire()
    {
        ScaleFire(GrowFactor);
    }

    private void ScaleFire(float amount)
    {
        transform.localScale += Vector3.one * amount;
    }

    public float GetFireSize(){
        return transform.localScale.x / MaxSize;
    }
}
