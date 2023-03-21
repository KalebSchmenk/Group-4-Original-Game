using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemyController : MonoBehaviour, EnemyHealthInterface
{
    [System.NonSerialized] public GameObject _player;
    private PlayerController _playerScript;

    private int _health = 5;
    public int health { get { return _health; } set { _health = value; } }

    [SerializeField] protected Transform _lightningStrikeLocation;
    public Transform hitLocation { get { return _lightningStrikeLocation; } set { _lightningStrikeLocation = value; } }

    public Transform _spellCastLocation;
    public GameObject _fireballPrefab;
    public GameObject _lightningStrikePrefab;

    public RangedEnemyBaseState currentState;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<PlayerController>();

        currentState = this.AddComponent<RangedEnemyLookAroundState>();
        currentState.EnterState(this);
    }

    void Update()
    {
        CheckHealth();

        currentState.UpdateState(this);
    }


    private void CheckHealth()
    {
        if (_health <= 0)
        {
            // Kill enemy
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        CheckHealth();
    }
}
