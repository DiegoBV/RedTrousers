using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class Traslado : MonoBehaviour {

    public bool salida;
    public int numCasa;
    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
