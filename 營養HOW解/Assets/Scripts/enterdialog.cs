using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterdialog : MonoBehaviour
{
    public GameObject enterDialog;  
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="player")
        {
            enterDialog.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="player")
        {
            enterDialog.SetActive(false);
        }
    }
}
