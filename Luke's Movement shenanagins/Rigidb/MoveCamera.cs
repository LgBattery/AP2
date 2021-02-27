using UnityEngine;
using Mirror;

public class MoveCamera : NetworkBehaviour
{
    private Transform player;
    private bool foundPlayer = false;

    void FixedUpdate()
    {
        if(GameObject.Find("LocalPlayer") && !foundPlayer)
        {
            player = GameObject.Find("LocalPlayer").transform;
        }

        if (player)
        {
            foundPlayer = true;
            transform.position = player.transform.position;
            transform.parent = PlayerPrefs.GetInt("RotateCameraWithVehicle") == 1 && player.transform.parent ? player.transform.parent : null;

            if (!player.parent)
            {
                transform.localEulerAngles = Vector3.zero;
            }
        }
    }
}