using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireballSpell : MonoBehaviour
{
    [SerializeField] private GameObject _testFireballPrefab;
    [SerializeField] private Transform _spellCastLocation;
    [SerializeField] float _cooldownTime = 2.5f;

    private Camera _mainCam;

    private bool _fireballCooldown = false;
    [Header("Player Sounds")]
    [SerializeField] AudioSource playerFireBallCastObject;
    [SerializeField] AudioClip playerFireBallCastClip;

    void Start()
    {
        _mainCam = Camera.main;
    }


    void Update()
    {
        if (Keyboard.current[Key.F].isPressed && !_fireballCooldown)
        {
            CastFireball();
        }
    }

    private void CastFireball()
    {
        RaycastHit hit;
        Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            _fireballCooldown = true;

            var fireball = Instantiate(_testFireballPrefab, _spellCastLocation.position, Quaternion.identity);

            FireballController tempFireballControl = fireball.GetComponent<FireballController>();

            tempFireballControl.Fire(hit.point);
        }

        playerFireBallCastObject.clip = playerFireBallCastClip;
        playerFireBallCastObject.Play();

        StartCoroutine(FireballCooldown());
    }

    private IEnumerator FireballCooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _fireballCooldown = false;
    }

}
