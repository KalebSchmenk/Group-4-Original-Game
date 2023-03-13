using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGuardSpell : MonoBehaviour
{
    [SerializeField] GameObject _guardSphere;

    [SerializeField] private float _cooldownTime = 2.5f;

    private bool _inCooldown = false;


    void Update()
    {
        if (Keyboard.current[Key.H].isPressed == true && !_inCooldown)
        {
            _inCooldown = true;
            GuardSphere();
        }
    }

    private void GuardSphere()
    {
        Instantiate(_guardSphere, this.transform.position, Quaternion.identity);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _inCooldown = false;
    }
}
