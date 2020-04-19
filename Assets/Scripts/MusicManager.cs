using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class MusicManager : MonoBehaviour
{
    public AudioMixer master;
    public FireManager fire;

    private string[] varVol;

    void Start(){
        varVol = new string[] {"bass1Vol", "bass2Vol", "bass3Vol", "ostinato1Vol", "ostinato2Vol", "themeVol"};
    }


    void Update(){
        SetVolume();
    }

    

    public void SetVolume (){

        
        float fireSize = fire.GetFireSize();

        float a;
        float b;
        float linearVol;
        float threshold_low;

        master.SetFloat(varVol[0], Mathf.Log10(1)*20.0f);

        for (int i = 1 ; i < varVol.Length ; i++){
            
            threshold_low = (i + 2f)/10f;         

            if(fireSize >= threshold_low){  // Linear function from threshold_low to thresholh_low+0.2, from 0.0001 to 1.0
                a = (1.0f - 0.0001f) / (0.2f);
                b = 0.0001f - a * threshold_low;
                linearVol = a * fireSize + b;            
                linearVol = (linearVol >= 1.0f) ? 1.0f : linearVol;
                master.SetFloat(varVol[i], Mathf.Log10(linearVol)*20.0f);
            }

            else{
                master.SetFloat(varVol[i], Mathf.Log10(0.0001f)*20.0f);
            }
        }

        
    }
}
