using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestFireballSpell : MonoBehaviour
{
    [SerializeField] private GameObject _testFireballPrefab;
    [SerializeField] private Transform _spellCastLocation;
    [SerializeField] float _cooldownTime = 2.5f;

    private Camera _mainCam;

    private bool _fireballCooldown = false;


    void Start()
    {
        _mainCam = Camera.main;
    }


    void Update()
    {
        if (Keyboard.current[Key.F].isPressed && !_fireballCooldown)
        {
            RaycastHit hit;
            Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                var fireball = Instantiate(_testFireballPrefab, _spellCastLocation.position, Quaternion.identity);

                TestFireballController tempFireballControl = fireball.GetComponent<TestFireballController>();

                tempFireballControl.Fire(hit.point);

                _fireballCooldown = true;
            }

            StartCoroutine(FireballCooldown());
        }
    }


    private IEnumerator FireballCooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _fireballCooldown = false;
    }
}
