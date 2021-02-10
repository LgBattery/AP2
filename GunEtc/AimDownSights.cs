using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimDownSights : MonoBehaviour
{
    public Vector3 aimDownSights;
    public Vector3 hipFire;
    public float aimspeed;
    public Image Crosshair;
    GameObject gun;
    public New_Weapon_Recoil_Script Recoil;
    public GunSway sway;

    [SerializeField] [Range(0, 1)] private float ZoomSpeed;

    void Start()
    {
        gun = GameObject.FindWithTag("Gun");
         Recoil = gun.GetComponent<New_Weapon_Recoil_Script>();
         sway = gameObject.GetComponent<GunSway>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSights, aimspeed * Time.deltaTime);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40, ZoomSpeed);
            Crosshair.enabled = false;
            sway.enabled = false;
            Recoil.aim = true;

        }
        else
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, aimspeed * Time.deltaTime);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, ZoomSpeed);
            Recoil.aim = false;
              sway.enabled = true;
        }
    }
}