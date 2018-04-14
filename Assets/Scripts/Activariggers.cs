using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(Interactuable))]
[RequireComponent(typeof(BoxCollider2D))]

public class Activariggers : MonoBehaviour {

    public int numTrigger;
    public bool Finish;
    public bool tuCasa;
    static bool[] destruir = new bool[10];

    void Start()
    {
        for (int i = 0; i < 11; i++)
        {
            if (destruir == null)
                destruir[i] = false;
        }

        if (destruir[int.Parse(this.gameObject.name)])
        {
            Destroy(this.gameObject);
        }    
    }
    /*void Update()
    {
        if (numTrigger == GameManager.estadoPersonaje)
        {
            this.GetComponent<BoxCollider2D>().enabled = true;          //esto no me mola nada pero no cambiamos de escena tiio
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }*/
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() && numTrigger == GameManager.estadoPersonaje)
        {
            //PARA QUE SOLO SE ACTIVE EN TU CASA Y NO EN LAS DEMÁS
            if (tuCasa)
            {
                if(other.GetComponent<PlayerController>().numCasa == 0)
                {
                    other.GetComponent<PlayerController>().vel = other.GetComponent<PlayerController>().velOr; //Restaurar la velocidad normal del jugador
                    this.GetComponent<Interactuable>().Interactuado();
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    destruir[int.Parse(this.gameObject.name)] = true;
                    Destroy(this);
                }
            }

            else
            {
                other.GetComponent<PlayerController>().vel = other.GetComponent<PlayerController>().velOr; //Restaurar la velocidad normal del jugador
                this.GetComponent<Interactuable>().Interactuado();
                if (!Finish)
                {
                    destruir[int.Parse(this.gameObject.name)] = true;
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    Destroy(this);
                }
            }
        }
    }
}
