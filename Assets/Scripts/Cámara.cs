using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cámara : MonoBehaviour {

	public Transform jugador;

	//Update
	void Update ()
	{
		transform.position = new Vector3(jugador.position.x, jugador.position.y, -10);
	}
}
