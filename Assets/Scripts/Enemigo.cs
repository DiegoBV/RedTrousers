using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour 
{
	//STATS
	[HideInInspector]
	public int saludInicial;
	[HideInInspector]
	public int hp, poder;

	//TIPO DE ENEMIGO
	public enum TipoEnemigo { Polilla, Polillo, Dragón, Snowman , Robot, RobotBoss, Boss};
	[HideInInspector]public int numEnemigo;
	public TipoEnemigo enemigo;

	//CANVAS
	Transform barraVida;
	float longInicial;
	[HideInInspector]
	public TextMesh dañorec, numVida;
	[HideInInspector]
	public AudioSource sonido;


	//1.START
	void Awake () 
	{
		//Le ponemos stats dependiendo del enemigo que sea
		switch (enemigo) 
		{
			case TipoEnemigo.Polilla:
				numEnemigo = 0;
				hp = 42;
				poder = 15;
				break;
			case TipoEnemigo.Polillo:
				numEnemigo = 1;
				hp = 65;
				poder = 12;
				break;
			case TipoEnemigo.Dragón:
				numEnemigo = 2;
				hp = 65;
				poder = 14;
				break;
			case TipoEnemigo.Snowman:
				numEnemigo = 3;
				hp = 80;
				poder = 18;
				break;
			case TipoEnemigo.Robot:
				numEnemigo = 4;
				hp = 75;
				poder = 22;
				break;
			case TipoEnemigo.RobotBoss:
				numEnemigo = 5;
				hp = 115;
				poder = 20;
				break;
			default:
				numEnemigo = 6;
				hp = 200;
				poder = 45;
				break;
		}
		
		dañorec = transform.GetChild(2).gameObject.GetComponent<TextMesh>();
		barraVida = transform.GetChild(0).GetChild(0);
		longInicial = barraVida.localScale.x;
		saludInicial = hp;

		numVida = transform.GetChild(3).gameObject.GetComponent<TextMesh>();
		numVida.text = hp + "/" + saludInicial;
		sonido = GetComponent<AudioSource>();
	}
	
	//2.UPDATE
	void Update () 
	{
		//Renderizado de la vida
		if (hp > 0)
		{
			float longActual = barraVida.localScale.x;
			barraVida.localScale = new Vector3(longInicial / saludInicial * hp, barraVida.localScale.y, 1f);

			barraVida.transform.Translate(new Vector3((barraVida.localScale.x - longActual) / 2, 0f, 0f));
		}
		else
			barraVida.localScale = new Vector3(0f, 0f, 1f);
	}
}
