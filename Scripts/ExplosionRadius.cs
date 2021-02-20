using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRadius : MonoBehaviour
{
    [SerializeField]
    private float radius = 5f;
    [SerializeField]
    private float damage = 5f;
    [SerializeField]
    private LayerMask mask;

    void Start()
    {
       Invoke("Explode", 0.1f);    
    }

    // Call this to damage all nearby objects
    public void Explode()
    {
        // OverLap sphere vehicles nearby 
        Collider[] hitParts = Physics.OverlapSphere(transform.position, radius, mask);

        List<VheicleDestruction> vheicles = new List<VheicleDestruction>();
        // Find the part with the vehicle destruction script
        foreach(Collider collider in hitParts)
        {
            if(collider.transform.parent && collider.transform.parent.GetComponent<VheicleDestruction>())
            {
                vheicles.Add(collider.GetComponentInParent<VheicleDestruction>());
            }
        }
        // Do damage to all parts in all the vheicles
        foreach(VheicleDestruction vheicle in vheicles)
        {
            foreach(Collider collider in hitParts)
            {
                //if it the part is a part of the vheicle damage the part
                if (collider.GetComponentInParent<VheicleDestruction>() == vheicle)
                {
                    int damage_ = (int)(Vector3.Distance(transform.position, collider.transform.position) * damage);
                    print("did " + damage_ + " Damage to " + collider.name);
                    vheicle.DoDamage(collider.transform, damage_);
                }
            }
        }
    }
}
