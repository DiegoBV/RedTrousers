using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Combate : MonoBehaviour 
{
	
	//Canvas
	Transform puntero;
	int elección = 0;

	//Dependencias
	public GameObject jugador, enemigos;
	GameObject enemigo;

	JugadorCombate compJug;
	Enemigo compEnem;
	AudioSource efectosJug;

	public GameObject Fondos;

	//Variables de combate
	bool bufoDefensa, bufoAtaque, invulnerable, envenenado, atacando = false;
	bool logroVeneno = true;
	float porcVeneno;

	//Sonido
	AudioSource compAudio;

	//1.START
	void Start ()
	{
		compAudio = GetComponent<AudioSource>();
		puntero = transform.GetChild(5);
		for (int i = 0; i < 5; i++) 
			for (int j = 0; j < 2; j++)
				transform.GetChild(i).GetChild(j).gameObject.SetActive(false);


		for (int i = 0; i < 5; i++) 
		{
			if(GameManager.instance.EstadoPersonaje()>=i)
				transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
			else
				transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
		}

		if (GameManager.instance.EstadoPersonaje() == 5)
		{
			transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
			transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
		}

		else 
		{
			transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
			transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
		}

		compJug = jugador.GetComponent<JugadorCombate>();
		enemigo = enemigos.transform.GetChild(GameManager.instance.TipoEnemigo()).gameObject;
		enemigo.SetActive(true);

		compEnem = enemigo.gameObject.GetComponent<Enemigo>();
		efectosJug = jugador.GetComponent<AudioSource>();


		//Música y fondos
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Combate"))
		{
			//Contra el boss
			if (compEnem.numEnemigo==6)
			{
				GameManager.instance.PlayMusic(1);
				Fondos.transform.GetChild(1).gameObject.SetActive(true);
			}
			//Normal 
			else
			{
				GameManager.instance.PlayMusic(0);
				Fondos.transform.GetChild(0).gameObject.SetActive(true);
			}
		}
	}
	
	//2.UPDATE
	void Update () 
	{
		if (GameManager.instance.Vida() > 0 && enemigo.GetComponent<Enemigo>().hp > 0 && !atacando)
		{
			if (elección == 5)
				elección = 0;
			
			if (Input.GetKeyDown(KeyCode.A) && elección > 0)
			{
				elección--;
				puntero.transform.position = new Vector3(transform.GetChild(elección).position.x, puntero.position.y, -1f);
			}

			else if (Input.GetKeyDown(KeyCode.D) && elección < 4)
			{
				elección++;
				puntero.transform.position = new Vector3(transform.GetChild(elección).position.x, puntero.position.y, -1f);
			}

			else if (Input.GetKeyDown(KeyCode.Space))
			{
				atacando = true;
				if (GameManager.instance.EstadoPersonaje() == 5 && elección == 0)
					elección=5;
				AtacaJug();
			}
		}

		else if(!atacando)
		{
			bufoAtaque = false;
			bufoDefensa = false;
			invulnerable = false;
			envenenado = false;

			//El PJ muere
			if (GameManager.instance.Vida() <= 0)
				GameManager.instance.MuereEnCombate();

			//El enemigo muere
			else 
			{
				string partida="";
				StreamReader entrada = new StreamReader(@"Red Trousers_Saves\combates");
				while (!entrada.EndOfStream)
					partida += entrada.ReadLine();
				entrada.Close();

				StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\combates");
				int k = 0;
				while (k<11)
				{
					if (partida[k] == 'X')
					{
						salida.WriteLine("1");
						Debug.Log("ee");
					}

					else
						salida.WriteLine(partida[k]);
					k++;
				}
				salida.Close();

				//Logros y esas cosas
				if (logroVeneno)
				{
					//Insecticida
					GameManager.instance.ConsigueLogro(7);
				}

                if ((compEnem.numEnemigo == 2) || (compEnem.numEnemigo == 5) || (compEnem.numEnemigo == 6))
                    GameManager.instance.AumentaEstado();

				if (compEnem.numEnemigo == 6)
				{
					//Moth-illa
					GameManager.instance.ConsigueLogro(5);
					//Maaazo pro
					if (File.Exists(@"Red Trousers_Saves\partida"))
					{
						StreamReader entrada2 = new StreamReader(@"Red Trousers_Saves\combates");
						string s = "";
						for (int i = 0; i < 4; i++)
							s = entrada2.ReadLine();
						entrada2.Close();

						if(s=="0")
							GameManager.instance.ConsigueLogro(3);
					}
					else
						GameManager.instance.ConsigueLogro(3);
					GameManager.instance.GuardaPartida(true);
				}

				//Restauramos al completo la vida
				GameManager.instance.SumaVida(GameManager.instance.VidaMaxima() - GameManager.instance.Vida());
			}

			SceneManager.LoadScene("Interacción");
        }
	}

	//3.EL JUGADOR ATACA
	void AtacaJug()
	{
		if (elección <= GameManager.instance.EstadoPersonaje() && (!bufoDefensa || elección!=4))
		{
			int vidaEnem = compEnem.hp;
			invulnerable = false;
			switch (elección)
			{
				//Pantalón blanco
				case 0:
					logroVeneno = false;
					efectosJug.PlayOneShot(compJug.Sonidos[3], GameManager.volu * 1.5f);
					jugador.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
					Invoke("QuitaEfecto", 1f);
					if (bufoAtaque)
						compEnem.hp -= (int)(GameManager.instance.Poder() * 1.25);
					else
						compEnem.hp -= GameManager.instance.Poder();
					break;
				//Guantes	
				case 1:
					logroVeneno = false;
					efectosJug.PlayOneShot(compJug.Sonidos[4], GameManager.volu * 0.9f);
					jugador.transform.GetChild(1).GetChild(4).gameObject.SetActive(true);
					Invoke("QuitaEfecto", 1f);

					if (bufoAtaque)
						compEnem.hp -= GameManager.instance.Poder();
					else 
					{
						compEnem.hp -= (int)(GameManager.instance.Poder() * 0.85);
						compJug.dañorec.color = Color.yellow;
						compJug.dañorec.text = "↑ATK";
					}

					bufoAtaque = true;
					break;
				//Sombrero	
				case 2:
					efectosJug.PlayOneShot(compJug.Sonidos[0], GameManager.volu);
					jugador.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
					compJug.dañorec.color = Color.green;
					compJug.dañorec.text = "+"+GameManager.instance.VidaMaxima() / 2 + "HP";
					Invoke("QuitaEfecto", 1f);
					//Logro nº1
					if (GameManager.instance.Vida() == GameManager.instance.VidaMaxima())
						GameManager.instance.ConsigueLogro(0);
					
					GameManager.instance.SumaVida(GameManager.instance.VidaMaxima() / 2);

					if (GameManager.instance.Vida() > GameManager.instance.VidaMaxima())
						GameManager.instance.QuitaVida(GameManager.instance.Vida() - GameManager.instance.VidaMaxima());
					
					compJug.numVida.text = GameManager.instance.Vida() + "/" + GameManager.instance.VidaMaxima();
					break;
				//Botas	
				case 3:
					efectosJug.PlayOneShot(compJug.Sonidos[2], GameManager.volu);
					jugador.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
					if (!envenenado)
					{
						envenenado = true;
						porcVeneno = 0.05f;
					}
					else
						porcVeneno += 0.05f;

					compEnem.dañorec.color = Color.magenta;
					compEnem.dañorec.text = "ENV "+porcVeneno*100+"%";
					Invoke("QuitaEfecto", 1f);
					break;
				//Camiseta	
				case 4:
					efectosJug.PlayOneShot(compJug.Sonidos[1], GameManager.volu);
					jugador.transform.GetChild(1).GetChild(1).gameObject.SetActive(true);

					compJug.dañorec.color = Color.yellow;
					compJug.dañorec.text = "↑DEF";

					invulnerable = true;
					bufoDefensa = true;
					transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
					transform.GetChild(4).GetChild(1).gameObject.SetActive(false);
					break;
				//Pantalones rojos	
				case 5:
					logroVeneno = false;
					efectosJug.PlayOneShot(compJug.Sonidos[5], GameManager.volu*1.3f);
					jugador.transform.GetChild(1).GetChild(5).gameObject.SetActive(true);
					Invoke("QuitaEfecto", 1f);
					if (bufoAtaque)
						compEnem.hp -= (int)(GameManager.instance.Poder() * 2);
					else
						compEnem.hp -= (int)(GameManager.instance.Poder()*1.5);
					break;
			}
			if (vidaEnem - compEnem.hp > 0)
			{
				compEnem.dañorec.text = (vidaEnem - compEnem.hp).ToString();
				if(compEnem.hp>=0)
					compEnem.numVida.text = compEnem.hp+"/"+compEnem.saludInicial;
				else
					compEnem.numVida.text = "0/" + compEnem.saludInicial;
			}

			if (compEnem.hp > 0)
				Invoke("AtacaEnem", 2f);
			else 
			{
				Invoke("MataEnemigo", 1f);
				compAudio.PlayOneShot(compAudio.clip, GameManager.volu * 2.5f);
			}
		}
		else
			FinTurno();
	}

	//4.EL ENEMIGO ATACA
	void AtacaEnem()
	{
		int vidaJug=GameManager.instance.Vida();
		compEnem.transform.GetChild(1).gameObject.SetActive(true);
		compEnem.sonido.PlayOneShot(compEnem.sonido.clip, GameManager.volu);
		compJug.dañorec.text = "";
		compEnem.dañorec.text = "";
		compEnem.dañorec.color = Color.red;
		compJug.dañorec.color = Color.red;

		if (compEnem.hp > 0)
		{
			//Vulnerabilidad
			if (!invulnerable)
			{
				//El enemigo ataca
				if (!bufoDefensa)
					GameManager.instance.QuitaVida(enemigo.GetComponent<Enemigo>().poder);
				else
					GameManager.instance.QuitaVida((int)(enemigo.GetComponent<Enemigo>().poder * 0.75));

				//Mostramos la vida restante
				if (vidaJug - GameManager.instance.Vida() > 0)
				{
					//Comprobamos que no hay salud negativa
					if (GameManager.instance.Vida() < 0)
						GameManager.instance.SumaVida(0 - GameManager.instance.Vida());
					
					compJug.dañorec.text = (vidaJug - GameManager.instance.Vida()).ToString();
					compJug.numVida.text = GameManager.instance.Vida() + "/" + GameManager.instance.VidaMaxima();
				}
			}
			else
				efectosJug.PlayOneShot(compJug.Sonidos[1], GameManager.volu);

			//Comprobamos si el jugador ha muerto
			if (GameManager.instance.Vida() > 0)
				Invoke("FinTurno", 2f);
			else 
			{
				Invoke("FinTurno", 0.8f);
				compAudio.PlayOneShot(compAudio.clip, GameManager.volu*2.5f);
			}

		}
		else 
		{
			FinTurno();
		}
	}

	//5.TERMINA EL TURNO
	void FinTurno() 
	{
		compEnem.transform.GetChild(1).gameObject.SetActive(false);
		jugador.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
		compJug.dañorec.text = "";


		//Veneno
		if (envenenado && (!bufoDefensa || elección != 4) && (GameManager.instance.EstadoPersonaje()!=3 || elección!=4))
		{
			efectosJug.PlayOneShot(compJug.Sonidos[2], GameManager.volu);
			jugador.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
			compEnem.dañorec.text = ((int)(compEnem.saludInicial * porcVeneno)).ToString();
			compEnem.hp -= (int)(compEnem.saludInicial * porcVeneno);

			if (compEnem.hp >= 0)
				compEnem.numVida.text = compEnem.hp + "/" + compEnem.saludInicial;
			else
				compEnem.numVida.text = "0/" + compEnem.saludInicial;
			
			Invoke("QuitaVeneno", 1f);
		}
		else
			atacando = false;

	}

	//6.ELIMINA EL VENENO CUANDO VA A MATAR AL ENEMIGO
	void QuitaVeneno() 
	{
		jugador.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
		compEnem.dañorec.text = "";
		atacando = false;
	}

	//7.EL ENEMIGO MUERE
	void MataEnemigo()
	{ 
		enemigo.SetActive(false);
		atacando = false;
	}

	//8.QUITA LOS EFECTOS DEL JUGADOR
	void QuitaEfecto() 
	{
		for (int i = 0; i < 6;i++)
			if(i!=1)
				jugador.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
	}

}