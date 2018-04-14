using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	//GM
	public static GameManager instance;

	//MÚSICA
	public AudioClip [] Musica;
	private AudioSource compAudio;
	public static float volu = 1f;
	static int musicaActual;

	//STATS
	public static int estadoPersonaje;
	public static int hp, poder;
	[HideInInspector]
	public static int vidaMaxima;
	static public bool[] logros = new bool[10];
	static int tipoEnemigo;

	//GUARDADO
	[HideInInspector]
	public static int combateX, combateY;
	[HideInInspector]
	public static int guardadoX, guardadoY;
	public GameObject jugador;

	//CANVAS
	public GameObject canvas;
	Text textoVida, textoPoder;
	GameObject MensajeLogro;
	GameObject BotonReinicio;

    //LOGRO NPCS
    public static bool [] NPcsHablados = new bool [27];
   
    //1.AWAKE
    void Awake () 
	{
		instance = this;
		compAudio = GetComponent<AudioSource>();

		if (canvas != null)
			MensajeLogro = canvas.transform.FindChild("MensajeLogros").gameObject;
		
			
        ActualizaVol(volu);

		//MÚSICA A REPRODUCIR
		//1.Música normal
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Interacción"))
		{
			PlayMusic(musicaActual);
			textoVida = canvas.transform.GetChild(0).GetChild(1).GetComponent<Text>();
			textoPoder = canvas.transform.GetChild(0).GetChild(2).GetComponent<Text>();

			if(File.Exists(@"Red Trousers_Saves\partida"))
			{
				StreamReader entrada = new StreamReader(@"Red Trousers_Saves\combates");
				string s = "";
				while (!entrada.EndOfStream)
					s += entrada.ReadLine();
				entrada.Close();

				if ((s[4] == '1' && estadoPersonaje < 2) || (s[7] == '1' && estadoPersonaje < 4)
				    || (s[9] == '1' && estadoPersonaje < 5))
				{
					Invoke("AumentaEstado", 0.2f);
					Invoke("Guarda", 0.3f);
				}
			}
		}

		//2.Música del menú
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menú"))
		{
			Directory.CreateDirectory("Red Trousers_Saves");
			PlayMusic(0);
			if (File.Exists(@"Red Trousers_Saves\partida"))
				LeePartida();
			else
			{
				estadoPersonaje = 0;
				hp = 20;
				poder = 15;
			}
			vidaMaxima = hp;

			BotonReinicio = canvas.transform.FindChild("Menu").FindChild("Reinicio").gameObject;

			if (File.Exists(@"Red Trousers_Saves\combates"))
			{
				StreamReader entrada = new StreamReader(@"Red Trousers_Saves\combates");
				string s = "";
				for (int i = 0; i < 11; i++)
					s = entrada.ReadLine();
				entrada.Close();

				if (s == "0")
					BotonReinicio.SetActive(false);
				else
					BotonReinicio.SetActive(true);
			}
		    else
				BotonReinicio.SetActive(false);
		}

		//3.Música de créditos
		else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Créditos"))
			PlayMusic(0);
	}
	
	//2.UPDATE
	void Update () 
	{
		if (canvas != null)
			ActualizaGUI();

            ActualizaVol(volu);
	}

	//3.DEVUELVE EL PODER DEL PERSONAJE
	public int Poder() 
	{
		return poder;
	}

	//4.DEVUELVE LA VIDA DEL PERSONAJE
	public int Vida()
	{
		return hp;
	}

	//5.QUITA VIDA AL PERSONAJE
	public void QuitaVida(int vida) 
	{
		hp -= vida;
	}

	//6.REPRODUCE UNA CANCIÓN
	public void PlayMusic(int num) 
	{
		compAudio.Stop();
		compAudio.clip = Musica[num];
		compAudio.Play();
	}

	//7.DEVUELVE LA CANCIÓN ACTUAL
	public int MusicaActual() 
	{
		return musicaActual;
	}

	//8.SUMA VIDA AL JUGADOR
	public void SumaVida(int vida) 
	{
		hp += vida;
	}

	//9.DEVUELVE LA VIDA MÁXIMA ACTUAL DEL JUGADOR
	public int VidaMaxima()
	{
		return vidaMaxima;
	}

	//10.DEVUELVE EL ESTADO DEL PERSONAJE
	public int EstadoPersonaje() 
	{
		return estadoPersonaje;
	}


	//12.VUELVE AL ÚLTIMO CHECKPOINT VISITADO
	public void MuereEnCombate()
	{
		hp = vidaMaxima;
		combateX = 0;
		combateY = 0;
	}

	//13.ACTUALIZA EL GUI
	public void ActualizaGUI()
	{
		if (textoVida != null)
		{
			textoVida.text = "" + vidaMaxima;
			textoPoder.text = "" + poder;
		}
	}


	//14.LEE PARTIDA
	public void LeePartida() 
	{
		StreamReader entrada = new StreamReader(@"Red Trousers_Saves\partida");

		//Stats
		estadoPersonaje = int.Parse(entrada.ReadLine());
		hp = int.Parse(entrada.ReadLine());
		poder = int.Parse(entrada.ReadLine());

		//Posición
		guardadoX = int.Parse(entrada.ReadLine());
		guardadoY = int.Parse(entrada.ReadLine());

		//Logros
		string s = entrada.ReadLine();
		for (int i = 0; i < 10; i++)
		{
			if (s[i] == '1')
				logros[i] = true;
			else
				logros[i] = false;
		}
		musicaActual = int.Parse(entrada.ReadLine());
		entrada.Close();
	}

    //15.COMIENZA EL JUEGO
    public void StartGame()
    {
		SceneManager.LoadScene("Interacción");

		if (!File.Exists(@"Red Trousers_Saves\partida"))
		{
			//Creamos el erchivo de los combates
			StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\combates");
			for (int i = 0; i < 11; i++)
				salida.WriteLine("0");
			salida.Close();
		}
    }

    //16.SALIR DEL JUEGO
    public void Salir()
    {
        Application.Quit();
    }

    //17.CONTROLADOR BARRA VOLUMEN
    public void VariarVol()
    {
        FindObjectOfType<SliderAudio>().GetComponent<SliderAudio>().SubmitSliderSetting();
    }

	//18.GUARDA PARTIDA
	public void GuardaPartida(bool finJuego) 
	{
		if (!finJuego)
		{
			//Guardado de las posiciones por si seguimos jugando
			guardadoX = (int)jugador.transform.position.x;
			guardadoY = (int)jugador.transform.position.y;
		}

		//Escribimos los datos
		StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\partida");
		if (finJuego)
			salida.WriteLine("5");
		else
			salida.WriteLine(estadoPersonaje);

		salida.WriteLine(vidaMaxima);
		salida.WriteLine(poder);
		if (finJuego)
		{
			salida.WriteLine("0");
			salida.WriteLine("0");
		}

		else
		{
			salida.WriteLine(guardadoX);
			salida.WriteLine(guardadoY);
		}
		//Logros
		for (int i = 0; i < 10; i++)
		{
			if (logros[i])
				salida.Write(1);
			else
				salida.Write(0);
		}
		salida.WriteLine();

		if (finJuego)
			salida.Write("0");
		else
			salida.Write(musicaActual);
		salida.Close();
	}

    //19.ACTUALIZAR VOLUMEN
     public void ActualizaVol(float volumen)
    {
        if(this != null)
        GetComponent<AudioSource>().volume = volumen;
        volu = volumen;
    }

	//20.AUMENTA EL NIVEL
	public void AumentaEstado() 
	{
		estadoPersonaje++;
		vidaMaxima += 10;
		poder += 5;
		hp = vidaMaxima;
		if (estadoPersonaje == 5)
			ConsigueLogro(1);
		if(jugador!=null)
			jugador.GetComponent<PlayerController>().anim.SetInteger("Estado", estadoPersonaje);
	}

	//21.CONSIGUE UN LOGRO
	public void ConsigueLogro(int i) 
	{
		if (!logros[i])
		{
			MensajeLogro.SetActive(true);
			Invoke("QuitaMensaje", 2f);
			logros[i] = true;

			//Logro de diamante
			int j = 0;
			while (j < 9 && logros[i])
				j++;
			if (j == 8)
				logros[9] = true;
		}
	}

	//22.DEVUELVE EL TIPO DE ENEMIGO QUE ES
	public int TipoEnemigo() 
	{
		return tipoEnemigo;
	}

	//23.DICE EL TIPO DE ENEMIGO QUE ES
	public void SetEnemigo(int i) 
	{
		tipoEnemigo = i;
	}

	//24.CARGA LA ESCENA DEL MENÚ TRAS LOS CRÉDITOS
	public void VolverDeCreditos() 
	{
		SceneManager.LoadScene("Menú");
	}

	//25.ESTABLECE LA MÚSICA ACTUAL
	public void SetMusica(int i) 
	{
		musicaActual = i;
	}

	//26.QUITA EL MENSAJE DE LOGROS
	void QuitaMensaje() 
	{
		MensajeLogro.SetActive(false);
	}

	//27.GUARDA(PARA INVOKES)
	void Guarda() 
	{
		GuardaPartida(false);
	}

	//28.DA EL LOGRO DEL VOLUMEN
	public void LogroVolumen() 
	{
		ConsigueLogro(2);
	}

	//29.REINCIA LA PARTIDA
	public void ReiniciaPartida() 
	{
		//Reiniciamos archivos
		if (File.Exists(@"Red Trousers_Saves\partida"))
		{
			StreamReader entrada = new StreamReader(@"Red Trousers_Saves\partida");
			//Leemos logros
			string s = "";
			for (int i = 0; i < 6; i++)
				s = entrada.ReadLine();
			for (int j = 0; j < 10; j++)
				logros[j] = (s[j]=='1');
			entrada.Close();

			//Borramos la partida
			File.Delete(@"Red Trousers_Saves\partida");
		}

		StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\combates");
		for (int i = 0; i < 11; i++)
			salida.WriteLine("0");
		salida.Close();

		//Reiniciamos variables
		guardadoX = 0;
		guardadoY = 0;
		estadoPersonaje = 0;
		hp = 20;
		poder = 15;
	    vidaMaxima = hp;
		GuardaPartida(false);
		BotonReinicio.SetActive(false);
	}
}