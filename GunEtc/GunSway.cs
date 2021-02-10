using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
  
    public float amount;
    public float SmoothAmount;
    public float MaxAmount;

   

    private Vector3 InitialPosition;

  
    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;

        Vector3 finalPosition = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + InitialPosition, Time.deltaTime * SmoothAmount); 
    }
}
