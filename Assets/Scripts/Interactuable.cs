using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Interactuable : MonoBehaviour {

    //SUPER SPAGUETTI LOOOKO
    public TextAsset archivoTexto;      //archivo a leer
    public string[] lineasDialogo;
    int i = 0;       //contador
    public char indicador;
    public Text texto;
    bool interactuado = false;
    public GameObject Player;
    public GameObject Panel;
    public bool esNPC;
    public int numNPC;
    public bool AumentaLevel;
    int indOr, j;
    //LogroPapelera
    public bool esPapelera;
    public int nivelAAumentar;

    //1.START
    void Start () {
 
        if(this.GetComponent<BoxCollider>())
        this.GetComponent<BoxCollider>().isTrigger = true;
        if(this.GetComponent<BoxCollider2D>())
        this.GetComponent<BoxCollider2D>().isTrigger = true;
        //separa por líneas y las guarda en el array
        lineasDialogo = archivoTexto.text.Split('\n');
        Panel.SetActive(false);
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
        j = i;
        indOr = j;
    }

	//2.UPDATE
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (interactuado)
            {
                Player.GetComponent<PlayerController>().compAudio.PlayOneShot
                        (Player.GetComponent<PlayerController>().sonidos[1], GameManager.volu);
                j++;
                texto.text = lineasDialogo[j];
                if (lineasDialogo[j][0] == '*')
                {
                    texto.text = " ";
                    if (Panel != null)
                        Panel.SetActive(false);
                    interactuado = false;
                    Invoke("Activar", 0.1f);
                    if (this.GetComponent<Activariggers>())
                    {
                        if (this.GetComponent<Activariggers>().Finish)
                        {
                            SceneManager.LoadScene("Créditos");
                        }
                    }
                }
                /*else
                {
                    texto.text = lineasDialogo[j];
                }*/
                
            }
        }
    }

	//3.MÉTODO PARA SER INTERACTUADO
    public void Interactuado()
    {
        if (!interactuado)
        {
            Player.GetComponent<PlayerController>().vel = Player.GetComponent<PlayerController>().velOr; //Restaura vel original del jugador
            Player.GetComponent<PlayerController>().compAudio.PlayOneShot
				  (Player.GetComponent<PlayerController>().sonidos[1], GameManager.volu);
            j = indOr;
            Player.GetComponent<PlayerController>().enabled = false;
			Player.GetComponent<PlayerController>().anim.SetBool("Andando", false);
			Player.GetComponent<PlayerController>().anim.SetBool("MismaDir", false);
				
            if(Panel != null)
            Panel.SetActive(true);
            //indicador = numero de texto que corresponda
            j++;        
            texto.text = lineasDialogo[j];
            Invoke("Interact", 0.1f);
        }

        //LogroPapelera
        if (esPapelera)
        {
            GameManager.instance.ConsigueLogro(8);
        }
        if (AumentaLevel && GameManager.instance.EstadoPersonaje() < nivelAAumentar)
        {
            GameManager.instance.AumentaEstado();        
        }
    }

	//4.ACTIVAR
    void Activar()
    {
        Player.GetComponent<PlayerController>().enabled = true;
        CancelInvoke();
    }

    //5.INTERACT
    void Interact()
    {
        interactuado = true;
        CancelInvoke();
    }
      
}
