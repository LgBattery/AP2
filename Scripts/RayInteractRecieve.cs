using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RayInteractRecieve : MonoBehaviour
{
    public UnityEvent onInteract;
    public bool canInteract = true;
    public Transform passTransform; //This is the transform interacting with the object

    // this gets sent back to the rayInteract script
    public string textSend;

    public void Interact()
    {
        onInteract.Invoke();
        print("interacted");
    }
    public void Recieve(RayInteract rayInteract)
    {
        rayInteract.text = textSend;    
    }
}
