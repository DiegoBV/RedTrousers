using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Interactuable))]
public class CheckPoint : MonoBehaviour {

	//1.START
    void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

	//2.COLISIÓN CON EL JUGADOR
    public void Activado (GameObject other)
    {
        if (other.GetComponent<PlayerController>() && Input.GetKey(KeyCode.Space))
        {
			//Interactúa
            GetComponent<Interactuable>().Interactuado();
            other.GetComponent<PlayerController>().enabled = false;

			//Guarda la partida
			GameManager.instance.GuardaPartida(false);
        }
    }
}
