using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool colliding = false;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
            colliding = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
            colliding = false;
    }
}
