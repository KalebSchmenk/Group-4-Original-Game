using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LightningStrike : MonoBehaviour
{

    [SerializeField] GameObject _lightningStrikePrefab;
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
            var randomRot = new Vector3(0, Random.Range(0, 360), 0);

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Transform spawnAt = hit.transform.gameObject.GetComponent<EnemyHealthInterface>().hitLocation;
                Instantiate(_lightningStrikePrefab, spawnAt.position, Quaternion.Euler(randomRot));
            }
            else
            {
                Instantiate(_lightningStrikePrefab, spawnLightningAt, Quaternion.Euler(randomRot));
            }

            _lightningInCooldown = true;
        }
        
        StartCoroutine(LightningCooldown());
    }

    private IEnumerator LightningCooldown()
    {
        Debug.Log("Lightning cooling down");
        yield return new WaitForSeconds(_cooldownTime);

        _lightningInCooldown = false;
    }
}
