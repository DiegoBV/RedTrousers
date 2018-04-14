using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JugadorCombate : MonoBehaviour {

	//Stats
	int saludInicial, hp;

	//Canvas
	[HideInInspector]public Transform barraVida;
	float longInicial;
	[HideInInspector]public TextMesh dañorec, numVida;

	//Animaciones y música
	public AudioClip[] Sonidos;
	Animator anim;

	//1.START
	void Start () 
	{
		barraVida = transform.GetChild(0).GetChild(0);
		longInicial = barraVida.localScale.x;
		dañorec = transform.GetChild(2).gameObject.GetComponent<TextMesh>();

		saludInicial = GameManager.instance.VidaMaxima();
		anim = GetComponent<Animator>();
		anim.SetInteger("Estado", GameManager.instance.EstadoPersonaje());

		numVida = transform.GetChild(3).gameObject.GetComponent<TextMesh>();
		numVida.text = GameManager.instance.Vida() + "/" + GameManager.instance.VidaMaxima();

		hp = GameManager.instance.Vida();
	}
	
	//2.UPDATE
	void Update () 
	{
		if (hp != GameManager.instance.Vida())
			ActualizaVida();
	}

	//3.ACTUALIZA VIDA
	void ActualizaVida()
	{
		float longActual = barraVida.localScale.x;
		hp = GameManager.instance.Vida();
		barraVida.localScale = new Vector3(longInicial / saludInicial * hp, barraVida.localScale.y, 1f);

		barraVida.transform.Translate(new Vector3((barraVida.localScale.x - longActual) / 2, 0f, 0f));
	}
}
