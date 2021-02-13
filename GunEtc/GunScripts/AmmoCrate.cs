using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : MonoBehaviour
{
   private Gun[] gun; 
   public int ammoCapacity;
   public GameObject crackedVersion;
  


    void Update()
    {
        
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player");
        
        
        {
            Debug.Log("poggers !");
           
        GameObject brokenCrate = Instantiate(crackedVersion, transform.position, transform.rotation);
        Destroy(brokenCrate, 8);
        Destroy(gameObject);

       
           gun = other.GetComponentsInChildren<Gun>();
           if (gun[0])
        {
            gun[0].CurrentStockpile += ammoCapacity;
        }  
        }
    }
}
