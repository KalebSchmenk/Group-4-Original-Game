using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestLightningStrike : MonoBehaviour
{

    [SerializeField] GameObject _testLightningStrikePrefab;
    [SerializeField] float _cooldownTime = 2.5f;
    private Camera _mainCam;
    private bool _lightningInCooldown = false;

    void Start()
    {
        _mainCam = Camera.main;    
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && !_lightningInCooldown)
        {
            CastLightning();
        }
    }

    private void CastLightning()
    {
        RaycastHit hit;
        Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            var spawnLightningAt = hit.point;

            Instantiate(_testLightningStrikePrefab, spawnLightningAt, Quaternion.identity);

            _lightningInCooldown = true;
        }
        
        StartCoroutine(LightningCooldown());
    }

    private IEnumerator LightningCooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _lightningInCooldown = false;
    }
}
