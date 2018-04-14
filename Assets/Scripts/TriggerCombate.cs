using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TriggerCombate : MonoBehaviour {

	public int enemigo;
	string archivo;

    void Start()
    {

		//Leemos el archivo
		StreamReader entrada = new StreamReader(@"Red Trousers_Saves\combates");
		//Leemos el archivo de combate
		for (int i = 0; i < 11; i++)
			archivo += entrada.ReadLine();

		entrada.Close();

		//Destruimos los triggers necesarios
		if (archivo[(int.Parse(gameObject.name) - 1)] == '1')
			Destroy(gameObject);

		//Por si se ha cerrado el juego abruptamente
		int j = 0;
		while (j < 11 && archivo[j] != 'X')
			j++;

		//EL JUEGO SE CERRÓ ABRUPTAMENTE
		if (j != 11) 
		{
			//Sobreescribimos
			StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\combates");

			int k = 0;
			while (k < 11)
			{
				if (k == j)
					salida.WriteLine("0");
				else
					salida.WriteLine(archivo[k]);
				k++;
			}

			salida.Close();

			//Hay que volver a leer el archivo
			archivo = "";
			StreamReader entrada2 = new StreamReader(@"Red Trousers_Saves\combates");
			//Leemos el archivo de combate
			for (int i = 0; i < 11; i++)
				archivo += entrada2.ReadLine();

			entrada2.Close();
		}
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
			StreamWriter salida = new StreamWriter(@"Red Trousers_Saves\combates");

			//Bucle para encontrar el nº que corresponde
			int i = 0;
			while (i < 11)
			{
				if(i == (int.Parse(gameObject.name) - 1))
					salida.WriteLine("X");
				else
					salida.WriteLine(archivo[i]);
				i++;
			}
			salida.Close();

			GameManager.combateX = (int)other.gameObject.transform.position.x;
			GameManager.combateY = (int)other.gameObject.transform.position.y;

			GameManager.instance.SetEnemigo(enemigo);
			SceneManager.LoadScene("Combate");
        }
    }
}
