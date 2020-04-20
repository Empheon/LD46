using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public Transform mainCamera;

    public void Play()
    {
        StartCoroutine(LoadScene());
        // StartCoroutine(DecreaseVolume());
        
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AudioSource m_AudioSource = mainCamera.GetComponent<AudioSource>();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            m_AudioSource.volume -= 0.05f;

            if (asyncOperation.progress >= 0.9f)
            {
                while(m_AudioSource.volume > 0)
                    {
                        m_AudioSource.volume -= 0.01f;
                        yield return new WaitForSeconds(.01f);
                    }
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    public void Quit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        SceneManager.LoadScene("Main");
    }
}
