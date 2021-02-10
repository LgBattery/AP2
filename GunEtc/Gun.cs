
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float ImpactForce = 30f;
    public float FireRate = 15f;

    private bool isReloading = false;

    private float NextTimeToFire = 0f;

    public New_Weapon_Recoil_Script recoil;

    public int MaxAmmo = 10;
    private int CurrentAmmo;
    public int MaxStockpile;
    public int CurrentStockpile;
    public float ReloadTime = 1f;


    public GameObject gunPrefab;

    public Light Flashlight;


    [SerializeField] private Text ammoCount;
    [SerializeField] private Text gunName;
    [SerializeField] private Text StockpileUI;



    void Start()
    {
        CurrentAmmo = MaxAmmo;
        CurrentStockpile = MaxStockpile;
    }

    // Update is called once per frame
    void Update()
    {
        //Reloading toggle
        if (Input.GetKeyDown("r"))

            if (isReloading == false)

                if (CurrentAmmo <= MaxAmmo - 1)

                    StartCoroutine(Reload());

        //DO NOT DELETE! I DONT KNOW WHAT IT DOES BUT IT WILL CATASTROPHICALLY DESTROY THE SCRIPT IN WAYS UNSEEN BY MAN IF DELETED. TRUST ME
        if (isReloading)
            return;


        if (CurrentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        //Recoil toggle
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            recoil.Fire();
        }
       
        //FireRate !
        if (Input.GetButtonDown("Fire1") && Time.time >= NextTimeToFire)
        {
            NextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }


        IEnumerator Reload()

        {
            isReloading = true;


            //How much ammo do we need?
            int AmmoNeeded = MaxAmmo - CurrentAmmo;

            //Check if we have enough ammo in our stockpile
            // Mathf.Min() returns the smaller number
            int AmmoTaken = Mathf.Min(CurrentStockpile, AmmoNeeded);

            //Remove the ammo from the stockpile
            CurrentStockpile -= AmmoNeeded;

            if (CurrentStockpile <= -1)

            {
                CurrentStockpile = 0;  //Makes sure it dosen't go into negative numbers
            }

            yield return new WaitForSeconds(ReloadTime);
            CurrentAmmo = CurrentAmmo + AmmoTaken;

            isReloading = false;

        }
        //Ammo UI
        ammoCount.text = CurrentAmmo.ToString();
        gunName.text = gunPrefab.name;
        StockpileUI.text = CurrentStockpile.ToString();
       
        //Flashlight toggle
        if (Input.GetKeyDown("f"))

        {
            Flashlight.enabled = !Flashlight.enabled;
        }

    }

    
    void Shoot()
    {

        CurrentAmmo--;  //subtracts 1 from each new currentAmmo() state

        //Shooting logic
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            //Damage system
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);

            } //Impact effect

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * ImpactForce);
            }

            //Muzzle Flash
            muzzleFlash.Play();
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(impactGO, 2f);

        }
    }
}