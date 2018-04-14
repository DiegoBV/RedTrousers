using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackZone : MonoBehaviour {

    [HideInInspector]
    public int backLayer;
    [HideInInspector]
    public int frontLayer;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            SpriteRenderer sprite = other.GetComponentInChildren<SpriteRenderer>();
            sprite.sortingOrder = backLayer;
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        SpriteRenderer sprite = other.GetComponentInChildren<SpriteRenderer>();
        sprite.sortingOrder = frontLayer;
    }

    }
