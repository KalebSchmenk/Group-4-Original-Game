using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    bool isOpened = false;
    

    void OnTriggerStay(Collider other)
    {
      if (other.CompareTag("Boulder"))
        if (!isOpened)
        {
            isOpened = true;
            door.transform.position += new Vector3(0, 4, 0);
        }
    }
}


