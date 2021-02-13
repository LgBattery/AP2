using UnityEngine;

public class Target : MonoBehaviour {


    public float health = 50f; // Public health float
    // Select which bool your object represents
    public bool NPC;
    public bool Explosive;
    public bool MiscObject;

    //EXPLOSIVE
    [SerializeField] private GameObject CrackedVersion;
    private float debrisLifetime = 15f;
    private float particleLifeTime = 4f;
    [SerializeField] private GameObject explosionEffect;
    

    public void TakeDamage (float amount) // Takes health
    {
        //NPC
        
        health -= amount; 
        if (health <= 0f && NPC == true) // Tells the computer to Die () if health is less than or equal to 0
        {
            Die();
        }

        // Explosive

         if (health <= 0f && Explosive == true) 
         {
              DieExplosive();
         }

    }

    void Die ()
    {
        Destroy(gameObject); 

        // death animation
        // Particle effect/shader perhaps
    }

    void DieExplosive()
    {
        GameObject ObjectFractured = Instantiate(CrackedVersion, transform.position, transform.rotation);
        Destroy(ObjectFractured, debrisLifetime);
        Destroy(gameObject);

        GameObject ExpClone = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(ExpClone, particleLifeTime);
    }

}
