using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collection : MonoBehaviour
{
    public void Death()
    {
        FindObjectOfType<player>().Poopcount();
        Destroy(gameObject);
    }
}
