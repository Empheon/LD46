using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarManager : MonoBehaviour
{

    public Sprite heartFull;
    public Sprite heartEmpty;
    public Image[] hearts;

    private int currentHP = 3;

    private float m_blinkSpeed = .1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoseHP(){
        
        if(currentHP > 0){
            currentHP--;
            hearts[currentHP].sprite = heartEmpty;
            StartCoroutine(Blink(hearts[currentHP]));
        }
    }

    IEnumerator Blink(Image currentHeart)
    {
        for (int i = 0; i < 8; i++)
        {
            currentHeart.color = new Color(1, .27f, .27f);
            yield return new WaitForSeconds(m_blinkSpeed);
            currentHeart.color = Color.white;
            yield return new WaitForSeconds(m_blinkSpeed);
        }
    }
}


