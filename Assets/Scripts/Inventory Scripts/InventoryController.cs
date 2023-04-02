using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static int _mushroomCount = 0;
    public static int _cherryCount = 0;
    public static int _leafCount = 0;
    public static int _pebbleCount = 0;

    [Header("Objects to disable and enable")]
    [SerializeField] GameObject _canCraftHealth;
    [SerializeField] GameObject _canNotCraftHealth;
    //[SerializeField] GameObject _canCraftGreanade;
    //[SerializeField] GameObject _canNotCraftGreanade;


    void Start()
    {
        
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
