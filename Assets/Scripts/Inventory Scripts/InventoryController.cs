using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _craftHealth;

    private PlayerController _playerScript;

    public static int _mushroomCount = 0;
    public static int _cherryCount = 0;
    public static int _leafCount = 0;
    public static int _pebbleCount = 0;

    [Header("Objects to disable and enable")]
    [SerializeField] GameObject _canCraftHealth;
    [SerializeField] GameObject _canNotCraftHealth;
    //[SerializeField] GameObject _canCraftGreanade;
    //[SerializeField] GameObject _canNotCraftGreanade;


    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _craftHealth = _playerInput.Player.CraftHealthPotion;
        _craftHealth.Enable();
    }
    private void OnDisable()
    {
        _craftHealth.Disable();
    }

    void Start()
    {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    
    void Update()
    {
        CheckForHealthPotion();

        //CheckForGrenade();
    }

    private void CheckForHealthPotion()
    {
        if (_mushroomCount >= 3 && _cherryCount >= 3)
        {
            _canCraftHealth.SetActive(true);
            _canNotCraftHealth.SetActive(false);

            if (_craftHealth.triggered && _playerScript.GetHealth() != 100)
            {
                _playerScript.Heal();

                _mushroomCount -= 3;
                _cherryCount -= 3;
            }
        }
        else
        {
            _canCraftHealth.SetActive(false);
            _canNotCraftHealth.SetActive(true);
        }
    }

    /*private void CheckForGrenade()
    {
        if (_leafCount >= 3 && _pebbleCount >= 3)
        {
            _canCraftGreanade.SetActive(true);
            _canNotCraftGreanade.SetActive(false);
        }
        else
        {
            _canCraftGreanade.SetActive(false);
            _canNotCraftGreanade.SetActive(true);
        }
    }*/
}
