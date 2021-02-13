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

    public bool testing = false;

    // The minimum force required for a part to take damage
    public float minDamageForce = 5f;

    public Transform[] wheels;
    public Transform mainBody;
    // Use for destruction of entire car
    public Transform parent;
    public Transform healthBar;

    public float velocityDamageMultipleier = 10f;
    public float vheicleHealth = 0f;

    // The number divided by 10 is the % that health will be shown at for a part
    private int healthBarThreshold = 7;


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
        if(!mainBody)
        {
            Destroy(parent.gameObject, 0);
            foreach (Transform wheel in wheels)
            {
                Destroy(wheel, 0);
            }
            GetComponent<CarControler>().enabled = false;
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
                else if(gameObject.GetComponent<Rigidbody>().velocity.magnitude > minDamageForce)
                {
                    print(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
                    velocityDamage = gameObject.GetComponent<Rigidbody>().velocity.magnitude * velocityDamageMultipleier;
                }
            }
                
            DoDamage(parts[partIndex].part, (int) velocityDamage);

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
        if ((float)parts[partIndex].health / (float)parts[partIndex].baseHealth < (float) healthBarThreshold / (float) 10 &&
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

    void BreakPart(int partIndex)
    {
        // Set part to dead
        parts[partIndex].dead = true;
        print(parts[partIndex].part + " has been broken!");

        // Detatch part from vheicle
        Vector3 partPostition = parts[partIndex].part.position;
        Quaternion partRotation = parts[partIndex].part.rotation;

        parts[partIndex].part.parent = null;

        parts[partIndex].part.position = partPostition;
        parts[partIndex].part.rotation = partRotation;

        // If there is no rigidbody, add one
        if (parts[partIndex].part.GetComponent<Rigidbody>() == null)
        {
            parts[partIndex].part.gameObject.AddComponent<Rigidbody>();
        }

        // Set the parts rigidbody and collider to active
        parts[partIndex].part.GetComponent<Rigidbody>().isKinematic = false;
        parts[partIndex].part.GetComponent<MeshCollider>().enabled = true;

        // CHANGE TIME TO SOMETHING CHANGEABLE LATER
        Destroy(parts[partIndex].part.gameObject, 10);
    }
}

// Part class definition
public class Part
{
    public Transform part;
    public int health;
    public bool dead;
    public int baseHealth;
    public bool healthBar;

    public Part(Transform part_, int health_, bool dead_)
    {
        baseHealth = health_;
        part = part_;
        health = health_;
        dead = dead_;
        healthBar = false;
    }
}
