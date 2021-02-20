using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VheicleDestruction : MonoBehaviour
{
    public List<Part> parts;

    // These are default values for part health that can change for different types of vheicles
    public int wheelHealth = 500;
    public int panelHealth = 1000;
    public int baseHealth = 2500;
    public int untaggedPartHealth = 500;

    // Test bools
    public bool testing = false;
    public bool destroy = false;

    // The minimum force required for a part to take damage
    public float minDamageForce = 5f;

    public Transform[] wheels;
    public Transform mainBody;
    // Use for destruction of entire car
    public Transform parent;
    public Transform healthBar;
    public Transform explosionParticle;
    public float explosionForce = 100f;
    public float explosionRadius = 5f;

    public float velocityDamageMultipleier = 10f;
    public float vheicleHealth = 0f;

    // The number divided by 10 is the % that health will be shown at for a part
    private int healthBarThreshold = 7;

    public bool destroyed = false;
    public bool exploded = false;


    // Start is called before the first frame update
    void Start()
    {
        parts = new List<Part>(); // Start list

        // Assign list with health for each part defined by what tag it has
        foreach (Transform part in transform.GetComponentsInChildren<Transform>())
        {
            int partHealth = 500;
            // Define health
            switch (part.tag)
            {
                case "Wheel":
                    partHealth = wheelHealth;
                    break;
                case "Panel":
                    partHealth = panelHealth;
                    break;
                case "Base":
                    partHealth = baseHealth;
                    break;
                case "Untagged":
                    partHealth = untaggedPartHealth;
                    break;
            }
            // Add the part
            parts.Add(new Part(part, partHealth, false));

            // Make sure the mesh colliders are convex
            if (part.GetComponent<MeshCollider>())
            {
                part.GetComponent<MeshCollider>().convex = true;
            }
        }
        if (testing)
        {
            InvokeRepeating("TestVoid", 1f, 5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mainBody && !destroyed)
        {
            DestroyCar();
        }
        if (destroy)
        {
            Destroy(mainBody.gameObject, 0);
            destroy = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // collision.gameObject is the object that hit the vheicle
        // parts[partIndex].part is the part that got hit

        Collider contactObj = collision.contacts[0].thisCollider;
        int partIndex = parts.FindIndex(a => a.part == contactObj.transform);

        // If the object is a wheel or player dont detect collision
        if (!ArrayContains(wheels, parts[partIndex].part) && !collision.gameObject.CompareTag("Player") && collision.gameObject.layer != 7)
        {
            print(parts[partIndex].part);

            float velocityDamage = 0;

            if (collision.gameObject.GetComponent<Rigidbody>())
            {
                if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude != 0)
                {
                    velocityDamage = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude * velocityDamageMultipleier;
                }
                else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > minDamageForce)
                {
                    print(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
                    velocityDamage = gameObject.GetComponent<Rigidbody>().velocity.magnitude * velocityDamageMultipleier;
                }
            }

            DoDamage(parts[partIndex].part, (int)velocityDamage);

            print(parts[partIndex].health);
        }

    }

    // Arry for convinience, DO NOT USE IN UPDATE
    bool ArrayContains(Transform[] array, Transform g)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == g) return true;
        }
        return false;
    }

    // For testing features
    void TestVoid()
    {
        print("broke something!");
        DoDamage(parts[Random.Range(0, parts.Count)].part, 500);
    }

    public void DoDamage(Transform part_, int damage)
    {
        // Get part index for convinience
        int partIndex = parts.FindIndex(a => a.part == part_);

        // Apply the damage to the part
        parts[partIndex].health = parts[partIndex].health - damage;

        // Check if part has no health to avoid checking in update
        if (parts[partIndex].health <= 0)
        {
            // Destroy the part if it has no health
            BreakPart(partIndex);
        }

        // Check if the part is below the health threshold to show healthbars
        if ((float)parts[partIndex].health / (float)parts[partIndex].baseHealth < (float)healthBarThreshold / (float)10 &&
            (parts[partIndex].healthBar == false))
        {
            Transform hb = Instantiate(healthBar, parts[partIndex].part);
            hb.GetComponent<HealthBar>().part = parts[partIndex];
            parts[partIndex].healthBar = true;
        }

        float healthCounter = 0f;

        for (int i = 0; i < parts.Count; i++)
        {
            healthCounter += parts[i].health;
        }

        vheicleHealth = healthCounter;

    }

    void DestroyCar()
    {
        if (!destroyed)
        {
            Instantiate(GetComponent<CarControler>().smoke, transform);
            Instantiate(GetComponent<CarControler>().smoke, transform);
            Invoke("ExplodeCar", 5f);

            destroyed = true;

            //Force player out of car so they dont get destroyed
            GetComponent<CarControler>().EvecuateCar();
        }
    }

    void ExplodeCar()
    {
        foreach (Part part in parts)
        {
            if (part.part)
            {
                // Unparent and put physics on part
                EnablePhysics(part.part);

                // Shoot part out
                part.part.GetComponent<Rigidbody>().AddForce(Vector3.up * explosionForce);

                // Break every part in vehicle
                DoDamage(part.part, 999999999);
            }
        }
        exploded = true;

        // Add explosion effect
        Instantiate(explosionParticle, transform);

        // Add an explosion force
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.GetComponent<Rigidbody>())
                collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }

    void EnablePhysics(Transform piece)
    {
        // Get index to see if part already has physics enabled
        int partIndex = parts.FindIndex(a => a.part == piece);

        if (piece && !parts[partIndex].physics)
        {
            // Detatch part from vheicle
            Vector3 partPostition = piece.position;
            Quaternion partRotation = piece.rotation;

            piece.parent = null;

            piece.position = partPostition;
            piece.rotation = partRotation;

            // If there is no rigidbody, add one
            if (piece.GetComponent<Rigidbody>() == null)
            {
                piece.gameObject.AddComponent<Rigidbody>();
            }

            // If there is no meshCollider, add one
            if (piece.GetComponent<MeshCollider>() == null)
            {
                piece.gameObject.AddComponent<MeshCollider>();
            }

            // Set the parts rigidbody and collider to active
            piece.GetComponent<Rigidbody>().isKinematic = false;
            piece.GetComponent<MeshCollider>().convex = true;
            piece.GetComponent<MeshCollider>().enabled = true;
        }
    }

    void BreakPart(int partIndex)
    {
        // Set part to dead
        parts[partIndex].dead = true;
        print(parts[partIndex].part + " has been broken!");

        // If the part is the main body destroy car instead
        if (parts[partIndex].part == mainBody)
        {
            DestroyCar();
            return;
        }
        // Turn on physics
        EnablePhysics(parts[partIndex].part);

        // CHANGE TIME TO SOMETHING CHANGEABLE LATER
        Destroy(parts[partIndex].part.gameObject, 10f + Random.Range(0f, 3f));
    }
}

// Part class definition
public class Part
{
    public Transform part;
    public int health;
    public bool dead;
    public bool physics;
    public int baseHealth;
    public bool healthBar;

    public Part(Transform part_, int health_, bool dead_)
    {
        baseHealth = health_;
        part = part_;
        health = health_;
        dead = dead_;
        physics = false;
        healthBar = false;
    }
}
