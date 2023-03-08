using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    [SerializeField] private Animator doorAnimationController;

    void OnTriggerStay(Collider other)
    {
      if (other.CompareTag("Boulder"))
        {
            doorAnimationController.SetBool("OpenDoor", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
         if (other.CompareTag("Boulder"))
            {
                doorAnimationController.SetBool("OpenDoor2", false);
            }
        }
    }



