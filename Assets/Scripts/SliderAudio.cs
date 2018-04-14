using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SliderAudio : MonoBehaviour {


    [HideInInspector] public Slider sld;
    
    void Awake()
    {
        sld = this.gameObject.GetComponent<Slider>();
        this.gameObject.GetComponent<Slider>().value = GameManager.volu;
    }
    public void SubmitSliderSetting()
    {
        GameManager.instance.ActualizaVol(sld.value);
    } 
       
}
