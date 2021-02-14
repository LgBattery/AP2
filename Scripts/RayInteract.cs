using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayInteract : MonoBehaviour
{
    private Transform cam;
    public float checkDistance;
    public KeyCode interact;
    public Transform player;

    [HideInInspector]
    public string text;

    public Text textObject;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(cam.position, cam.forward, out hit, checkDistance);

        // If it has required components and can interact
        if (hit.transform != null && hit.transform.GetComponent<RayInteractRecieve>() && hit.transform.GetComponent<RayInteractRecieve>().canInteract)
        {
            // Send data to object if one has been hit and the interact button has been pressed
            if (hit.transform != null && Input.GetKeyDown(interact))
            {
                hit.transform.GetComponent<RayInteractRecieve>().Interact();
                if(player)
                    hit.transform.GetComponent<RayInteractRecieve>().passTransform = player;
            }

            // If there is a possible interaction recieve possible interaction
            if (hit.transform != null)
            {
                hit.transform.GetComponent<RayInteractRecieve>().Recieve(this);
                if (player)
                    hit.transform.GetComponent<RayInteractRecieve>().passTransform = player;
                if (textObject)
                    textObject.text = text;
            }
        }
        else
        {
            if (textObject)
                textObject.text = " ";
        }
    }
}
