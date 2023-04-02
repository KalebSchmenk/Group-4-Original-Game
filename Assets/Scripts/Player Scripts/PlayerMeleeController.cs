using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeController : MonoBehaviour
{
    private bool inMeleeCooldown = false;

    [SerializeField] private float _meleeCooldownTime = 1.0f;

    [SerializeField] private GameObject _playerAttackSphere;

    [SerializeField] Transform _attackLocation;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerMeleeObject;
    [SerializeField] AudioClip playerMeleeClip;


    void Update()
    {
        if (Keyboard.current[Key.V].isPressed == true && !inMeleeCooldown) 
        {
            Melee();
        }
    }

    private void Melee()
    {
        // Melee Anim

        Instantiate(_playerAttackSphere, _attackLocation.position, Quaternion.identity);
        playerMeleeObject.clip = playerMeleeClip;
        playerMeleeObject.Play();
        StartCoroutine(MeleeCooldownTimer());
    }

    private IEnumerator MeleeCooldownTimer()
    {
        inMeleeCooldown = true;

        yield return new WaitForSeconds(_meleeCooldownTime);

        inMeleeCooldown = false;
    }
}
