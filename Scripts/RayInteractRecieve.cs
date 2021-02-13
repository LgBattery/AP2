using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RayInteractRecieve : MonoBehaviour
{
    public UnityEvent onInteract;
    public bool canInteract = true;

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
