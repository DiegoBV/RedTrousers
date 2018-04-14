using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BotonesInventario : MonoBehaviour {

    int i = 0;
    public GameObject Panel;
    public Text texto;
    public TextAsset archivoTexto;
    public string[] lineasDialogo;
    public char indicador;
    // Use this for initialization
    void Start ()
    {
        lineasDialogo = archivoTexto.text.Split('\n');
        Panel.SetActive(false);
    }
    public void Interactuado()
    {
        GameObject Player = FindObjectOfType<PlayerController>().gameObject;
        Player.GetComponent<PlayerController>().compAudio.PlayOneShot
		      (Player.GetComponent<PlayerController>().sonidos[1], GameManager.volu);
        i = 0;
            if (Panel != null)
                Panel.SetActive(true);       
            bool prueba = false;
            while (i < lineasDialogo.Length && !prueba)
            {
                if (lineasDialogo[i][0] != indicador)
                {
                    i++;
                }
                else
                {
                    prueba = true;
                }
            }
            texto.text = " ";
            //Invoke("Activar", 0.2f);
            texto.text = lineasDialogo[i + 1];
        }
    }

