using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersMusica : MonoBehaviour {

	public int zona1, zona2;

	void OnTriggerExit2D() 
	{
		if (GameManager.instance.MusicaActual() == zona1)
		{
			GameManager.instance.PlayMusic(zona2);
			GameManager.instance.SetMusica(zona2);
		}
		else 
		{
			GameManager.instance.PlayMusic(zona1);
			GameManager.instance.SetMusica(zona1);
		}
	}
}
