using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloqueador : MonoBehaviour {

	int numBloq;
	void Start () 
	{
		numBloq = int.Parse(gameObject.name);
	}
	

	void Update () 
	{
		if (numBloq <= GameManager.instance.EstadoPersonaje())
			gameObject.SetActive(false);
	}
}
