using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonImagenes : MonoBehaviour {

    public GameObject imagenActivar, imagenDesactivar;
    public bool ActivaLogros;
    public void ActivaImagen()
    {
        if (imagenActivar != null && imagenDesactivar != null)
        {
            imagenActivar.SetActive(true);
            imagenDesactivar.SetActive(false);
            if (ActivaLogros)
            {
                for(int i = 0; i < GameManager.logros.Length; i++)
                {
                    if (!GameManager.logros[i])
                    {
                        imagenActivar.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(true);
                        imagenActivar.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        imagenActivar.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
                        imagenActivar.transform.GetChild(i).transform.GetChild(1).gameObject.SetActive(true);
                        if(i == 6)
                        {
                            imagenActivar.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Habla 30 veces con Torolo";
                        }
                        else if(i == 8)
                        {
                            imagenActivar.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Rebusca en la basura de casas ajenas";
                        }
                        else if(i == 0)
                        {
                            imagenActivar.transform.GetChild(i).transform.GetChild(2).GetComponent<Text>().text = "Usa el sombrero teniendo la salud al maximo";
                        }
                    }
                }
            }
        } 
    }

   
}

