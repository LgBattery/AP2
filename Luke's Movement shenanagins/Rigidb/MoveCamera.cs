using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    public Transform player;

    void Update()
    {
        transform.position = player.transform.position;
    }
    void FixedUpdate()
    {
        transform.parent = PlayerPrefs.GetInt("RotateCameraWithVehicle") == 1 && player.transform.parent ? player.transform.parent : null;

        if(!player.parent)
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }
}