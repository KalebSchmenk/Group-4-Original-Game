using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestManipulatableObjectController : MonoBehaviour
{
    [SerializeField] private float _pullSpellCooldown = 2.5f;
    [SerializeField] private float _pushSpellCooldown = 2.5f;
    [SerializeField] private float _levitateSpellCooldown = 2.5f;

    private bool _pushInCooldown = false;
    private bool _pullInCooldown = false;
    private bool _levitateInCooldown = false;


    private void Update()
    {
        if (Keyboard.current[Key.R].isPressed && !_levitateInCooldown)
        {
            LevitateObject();
        }

        if (Keyboard.current[Key.T].isPressed && !_pushInCooldown)
        {
            PushObject();
        }

        if (Keyboard.current[Key.Y].isPressed && !_pullInCooldown)
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

            StartCoroutine(LevitateSpellCooldown());
        }
    }

    private void PushObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<PushController>().Push(this.gameObject);

            StartCoroutine(PushSpellCooldown());
        }
    }

    private void PullObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<PullController>().Pull(this.gameObject);

            StartCoroutine(PullSpellCooldown());
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


    private IEnumerator PullSpellCooldown()
    {
        _pullInCooldown = true;

        yield return new WaitForSeconds(_pullSpellCooldown);

        _pullInCooldown = false;
    }

    private IEnumerator PushSpellCooldown()
    {
        _pushInCooldown = true;

        yield return new WaitForSeconds(_pushSpellCooldown);

        _pushInCooldown = false;
    }

    private IEnumerator LevitateSpellCooldown()
    {
        _levitateInCooldown = true;

        yield return new WaitForSeconds(_levitateSpellCooldown);

        _levitateInCooldown = false;
    }
}
