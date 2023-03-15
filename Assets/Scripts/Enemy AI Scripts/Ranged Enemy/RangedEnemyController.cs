using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RangedEnemyController : MonoBehaviour, EnemyHealthInterface
{
    private GameObject _player;
    private PlayerController _playerScript;

    private int _health = 5;
    public int health { get { return _health; } set { _health = value; } }

    [SerializeField] protected Transform _lightningStrikeLocation;
    public Transform hitLocation { get { return _lightningStrikeLocation; } set { _lightningStrikeLocation = value; } }

    [SerializeField] public Transform _spellCastLocation;
    [SerializeField] public GameObject _fireballPrefab;
    [SerializeField] public GameObject _lightningStrikePrefab;

    public RangedEnemyBaseState currentState;
    public RangedEnemyAttackState AttackState = new RangedEnemyAttackState();
    public RangedEnemyLookAroundState LookAroundState = new RangedEnemyLookAroundState();


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<PlayerController>();

        currentState = LookAroundState;
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


    public void CastFireball()
    {
        var fireball = Instantiate(_fireballPrefab, _spellCastLocation.position, Quaternion.identity);

        FireballController tempFireballControl = fireball.GetComponent<FireballController>();

        tempFireballControl.Fire(_player.transform.position);
    }

    public void CastLightningStrike()
    {
        Transform spawnAt = _playerScript._lightningSpawnLocation;
        Instantiate(_lightningStrikePrefab, spawnAt.position, Quaternion.identity);
    }

}
