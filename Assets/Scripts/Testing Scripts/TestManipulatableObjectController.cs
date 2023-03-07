using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestManipulatableObjectController : MonoBehaviour
{



    private void Update()
    {
        if (Keyboard.current[Key.R].isPressed)
        {
            LevitateObject();
        }

        if (Keyboard.current[Key.T].isPressed)
        {
            PushObject();
        }

        if (Keyboard.current[Key.Y].isPressed)
        {
            PullObject();
        }
    }

    private void LevitateObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<LevitateController>().BeginLevitating();
        }
    }

    private void PushObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<PushController>().Push();
        }
    }

    private void PullObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<PullController>().Pull();
        }
    }

    private GameObject CastRay()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("CanManipulate"))
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }

        return null;
    }
}
