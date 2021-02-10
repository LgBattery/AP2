using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
   private Gun[] gun; 


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        
        
        {
            GameObject other = collision.gameObject; 
            Destroy(gameObject);

           gun = other.GetComponentsInChildren<Gun>();
           gun[0].CurrentStockpile += 20;
        }
    }
}
