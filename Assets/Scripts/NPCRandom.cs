using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPCRandom : MonoBehaviour {

    public Text texto;
    public GameObject Panel;
    public string[] lineasDialogo;
    public TextAsset archivoTexto;
    bool interactuado = false;
    public GameObject Player;
    static int cont = 0;
    // Use this for initialization
    void Start () {

        lineasDialogo = archivoTexto.text.Split('\n');
        if (this.GetComponent<BoxCollider2D>())
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
       
    }
	
	// Update is called once per frame
	void Update () {


        if (interactuado && Input.GetKeyDown(KeyCode.Space))
        {
            Panel.SetActive(false);
            Player.GetComponent<PlayerController>().enabled = true;
            Invoke("Activar", 0.1f);
        }
		
	}

    public void Activado()
    {
        Player.GetComponent<PlayerController>().vel = Player.GetComponent<PlayerController>().velOr; //Restaura vel original del jugador
        Player.GetComponent<PlayerController>().compAudio.PlayOneShot
                  (Player.GetComponent<PlayerController>().sonidos[1], GameManager.volu);
        Panel.SetActive(true);
        texto.text = lineasDialogo[Random.Range(0, 30)];
        Invoke("Activar", 0.01f);
        cont++;
        //LOGRO CANSINO
        if(cont >= 30)
        {
            GameManager.instance.ConsigueLogro(6);
        }
    }

    void Activar()
    {
        interactuado = !interactuado;
        CancelInvoke();
    }
}
