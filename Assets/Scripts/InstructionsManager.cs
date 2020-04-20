using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public float TimeBeforeDisappear = 20f;
    private float m_timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (m_timer < TimeBeforeDisappear)
        {
            m_timer += Time.deltaTime;
        } else
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        var srs = GetComponentsInChildren<SpriteRenderer>();
        
        float d = 0;
        while (d < 30)
        {
            foreach (var sr in srs)
            {
                d += Time.deltaTime;
                var a = Mathf.Lerp(.75f, 0, d / 30);
                sr.color = new Color(1, 1, 1, a);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
