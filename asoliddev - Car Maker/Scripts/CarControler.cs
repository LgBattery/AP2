﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Car Controls
/// </summary>
public class CarControler : MonoBehaviour
{
    [SerializeField]
    private bool inCar = false;
    public Transform smoke;
    private bool inDriverSeat = false;
    public KeyCode exit;
    public List<WheelAxle> wheelAxleList;
    public CarSettings carSettings;
    public bool controlActive;
    private Rigidbody rbody;
    public float speed = 0;
    private float motor;
    private float steering;
    private float handBrake;
    public Transform[] seats;
    public Transform engine;
    private Transform seat;
    private Transform playerTransform;
    public float test;
    private bool broken = false;
    public Transform player;

    private void Start()
    {
        ///create rigidbody
        rbody = this.GetComponent<Rigidbody>();

        ///set mass of the car
        rbody.mass = carSettings.mass;

        ///set drag of the car
        rbody.drag = carSettings.drag;

        //set the center of mass of the car
        rbody.centerOfMass = carSettings.centerOfMass;
    }


    /// <summary>
    /// Visual Transformation of the car wheels.
    /// </summary>
    /// <param name="wheelCollider"></param>
    /// <param name="wheelMesh"></param>
    public void ApplyWheelVisuals(WheelCollider wheelCollider, GameObject wheelMesh)
    {
        if (!broken && !GetComponent<VheicleDestruction>().destroyed)
        {
            Vector3 position;
            Quaternion rotation;

            ///get position and rotation of the WheelCollider
            wheelCollider.GetWorldPose(out position, out rotation);

            ///calculate real rotation of the wheels
            Quaternion realRotation = rotation * Quaternion.Inverse(wheelCollider.transform.parent.rotation) * this.transform.rotation;

            ///set position of the wheel
            if (wheelMesh)
            {
                wheelMesh.transform.position = position;
            }

            ///set rotation of the wheel
            if (wheelMesh)
            {
                wheelMesh.transform.rotation = realRotation;

                //Stop wheels from being inverted
                wheelMesh.transform.Rotate(0, 180, 0);
            }
        }
    }

    public void GetInCar()
    {
        if (transform.GetComponent<RayInteractRecieve>())
        {
            player = transform.GetComponent<RayInteractRecieve>().passTransform;
        }
        else
        {
            print("THERE IS NOT RAY INTERACTRECIEVE SCRIPT ON " + gameObject.name);
        }
        // Choose the closest seat to the max number of seats, the last seat in the array should always be the driver seat
        bool chairAvalible = false;

        for (int i = 0; i < seats.Length; i++)
        {
            // Check to see if someone is sitting in the seat
            if (seats[i].childCount == 0)
            {
                // If no one is use it
                seat = seats[i];
                chairAvalible = true;
            }
        }
        if (!chairAvalible)
        {
            return;
        }

        Rigidbody rb = player.GetComponent<Rigidbody>();

        // Undo all of these when the player gets out
        player.transform.parent = seat;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.None;
        player.gameObject.layer = 6;

        inCar = true;
        playerTransform = player;

        // Get inside of the car and turn it on if in driver seat
        player.localEulerAngles = Vector3.zero;
        player.localPosition = Vector3.zero;
        if (seat == seats[seats.Length - 1]) // If it is driver seat
        {
            controlActive = true;
            inDriverSeat = true;
        }
    }
    public void EvecuateCar()
    {
        //Removes everything from seats
        foreach (Transform seat in seats)
        {
            if (seat.childCount > 0)
            {
                foreach (Transform passenger in seat.GetComponentsInChildren<Transform>())
                {
                    if(passenger.CompareTag("Player"))
                        GetOut(passenger);
                }
            }
        }
    }
    void GetOut(Transform passenger)
    {
        if(!passenger)
        {
            passenger = playerTransform;
        }
        print("GetOutMaybe");
        Vector3 exitPos = passenger.position;

        Rigidbody rb = passenger.GetComponent<Rigidbody>();

        //Restore player transform and controller settings
        passenger.transform.parent = null;
        passenger.GetComponent<PlayerMovementr>().enabled = true;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        passenger.gameObject.layer = 0;
        passenger.localEulerAngles = Vector3.zero;

        // Make sure player exits at point it started
        passenger.position = exitPos + Vector3.up;

        if (inDriverSeat)
        {
            controlActive = false;
            inDriverSeat = false;
        }

        inCar = false;
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(exit) && inCar && playerTransform)
        {
            StartCoroutine(GetOutOfCar());
        }

        if (controlActive)
        {
            GetComponent<RayInteractRecieve>().canInteract = false;

            ///get speed of the car
            speed = rbody.velocity.magnitude;

            ///calculate motor torque to move car as long an an engine is present
            if (engine)
            {
                motor = carSettings.motorTorque * Input.GetAxis("Vertical");
            }

            //calculate wheel steering
            steering = carSettings.steeringAngle * Input.GetAxis("Horizontal");

            ///calculate motor break
            handBrake = Input.GetKey(KeyCode.Space) == true ? carSettings.motorTorque * 1 : 0;
        }
        else
        {
            GetComponent<RayInteractRecieve>().canInteract = true;
            motor = 0;
        }
        ///iterate all wheel axles
        foreach (WheelAxle wheelAxle in wheelAxleList)
        {
            if (wheelAxle.wheelMeshLeft && wheelAxle.wheelMeshRight && !GetComponent<VheicleDestruction>().exploded)
            {
                ///this is a steering axle
                if (wheelAxle.steering)
                {
                    ///apply steering
                    wheelAxle.wheelColliderLeft.steerAngle = steering;
                    wheelAxle.wheelColliderRight.steerAngle = steering;
                }

                ///this is motor axle
                if (wheelAxle.motor)
                {
                    ///apply motor torque
                    wheelAxle.wheelColliderLeft.motorTorque = motor;
                    wheelAxle.wheelColliderRight.motorTorque = motor;
                    test = motor;
                }

                ///apply motor break
                wheelAxle.wheelColliderLeft.brakeTorque = handBrake;
                wheelAxle.wheelColliderRight.brakeTorque = handBrake;


                ///apply wheel visuals
                ApplyWheelVisuals(wheelAxle.wheelColliderLeft, wheelAxle.wheelMeshLeft);
                ApplyWheelVisuals(wheelAxle.wheelColliderRight, wheelAxle.wheelMeshRight);
            }
        }
        if (broken == false && !engine)
        {
            broken = true;
            Instantiate(smoke, transform);
        }
    }
    IEnumerator GetOutOfCar()
    {
        yield return new WaitForSeconds(0.1f);

        GetOut(null);
    }
}