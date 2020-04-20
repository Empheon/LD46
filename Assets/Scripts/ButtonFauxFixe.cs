using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFauxFixe : MonoBehaviour
{
    public List<Sprite> ButtonSprites;
    private static float Lapse = .2f;
    private float m_timer = 0;
    private int idx;

    private Image m_btn;
    // Start is called before the first frame update
    void Start()
    {
        m_btn = GetComponent<Image>();
        idx = Random.Range(0, ButtonSprites.Count);
        m_btn.sprite = ButtonSprites[idx];
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer < Lapse)
        {
            m_timer += Time.deltaTime;
            if (Time.timeScale == 0)
            {
                m_timer += 1 / 60f;
            }
        } else
        {
            m_timer = 0;
            idx++;
            m_btn.sprite = ButtonSprites[idx % (ButtonSprites.Count - 1)];
        }
    }
}
