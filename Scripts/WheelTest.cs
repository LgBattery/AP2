using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTest : MonoBehaviour
{
    public Transform[] wheels;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w"))
        {
            for(int i = 0; i < wheels.Length; i++)
            {
                wheels[i].transform.localEulerAngles = Vector3.Lerp(wheels[i].transform.localEulerAngles, new Vector3(100000000, wheels[i].transform.localEulerAngles.y, wheels[i].transform.localEulerAngles.z), Time.deltaTime * speed);
            }
        }
    }
}
