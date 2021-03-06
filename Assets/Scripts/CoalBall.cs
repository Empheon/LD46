﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalBall : MonoBehaviour
{
    public static float Speed = 4;
    private Vector3 m_direction;

    public List<CoalEater> Predators;
    private bool m_isUnregistered = false;
    public SpriteRenderer AnimSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Predators = new List<CoalEater>();
        Init();
    }

    public void Init()
    {
        m_isUnregistered = false;
        Predators.Clear();
        WorldGenerator.Instance.EatableCoalList.Add(gameObject);
        m_direction = Vector3.Normalize(FireManager.GOInstance.transform.position - transform.position);
        if (m_direction.x < 0)
        {
            AnimSpriteRenderer.flipX = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(m_direction * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("LightFire"))
        {
            Unregister();
        }

        //FireManager fm = collision.gameObject.GetComponent<FireManager>();
        if (collision.gameObject.CompareTag("Fire"))
        {
            collision.gameObject.GetComponent<FireManager>().GrowFire();
            DestroyBall();
        }
    }

    private void Unregister()
    {
        WorldGenerator.Instance.EatableCoalList.Remove(gameObject);
        foreach (var p in Predators)
        {
            p.CoalBallDisappear();
        }
        Predators.Clear();
        m_isUnregistered = true;
    }

    public void DestroyBall()
    {
        if (!m_isUnregistered)
        {
            Unregister();
        }
        TerrierSpawner.ReleaseBall(gameObject);
        //Destroy(gameObject);
    }

    public void RegisterPredator(CoalEater predator)
    {
        Predators.Add(predator);
    }

    public void UnregisterPredator(CoalEater predator)
    {
        if (!m_isUnregistered)
        {
            Predators.Remove(predator);
        }
    }
}
