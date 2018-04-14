using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof(Interactuable))]
public class BotonLogros : MonoBehaviour {

    public Text texto;
    //public GameObject panel	
	void ActivarTexto()
    {
        //panel.SetActive(true);
        this.GetComponent<Interactuable>().Interactuado();
    }
}
