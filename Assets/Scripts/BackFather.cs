using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//añade el componente deathzone a sus hijos
public class BackFather : MonoBehaviour
{

    public int backLayer, frontLayer;

    private void Awake()
    {

        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxCollider2D>() != null)
            {
                child.gameObject.AddComponent<BackZone>();
                child.gameObject.GetComponent<BackZone>().backLayer = backLayer;
                child.gameObject.GetComponent<BackZone>().frontLayer = frontLayer;
                child.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }

}
