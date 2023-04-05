using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeController : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _melee;

    private Animator _anim;

    private bool inMeleeCooldown = false;

    [SerializeField] private float _meleeCooldownTime = 1.0f;

    [SerializeField] private GameObject _playerAttackSphere;

    [SerializeField] Transform _attackLocation;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerMeleeObject;
    [SerializeField] AudioClip playerMeleeClip;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void Start()
    {
        _anim = GetComponent<PlayerController>()._anim;
    }

    private void OnEnable()
    {
        _melee = _playerInput.Player.Melee;
        _melee.Enable();
    }
    private void OnDisable()
    {
        _melee.Enable();
    }

    void Update()
    {
        if (_melee.triggered == true && !inMeleeCooldown) 
        {
            Melee();
        }
    }

    private void Melee()
    {
        // Melee Anim
        _anim.SetTrigger("Attack");

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
