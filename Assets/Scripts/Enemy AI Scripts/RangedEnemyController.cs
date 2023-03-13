using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RangedEnemyController : MonoBehaviour, EnemyHealthInterface
{
    private enum AIState
    {
        LookAround,
        Attack
    }
    private AIState _AIState;

    private bool _canSeePlayer = false;
    private bool _lookingAround = true;

    private GameObject _player;
    private PlayerController _playerScript;

    private int _health = 5;
    public int health { get { return _health; } set { _health = value; } }

    [SerializeField] protected Transform _lightningStrikeLocation;
    public Transform hitLocation { get { return _lightningStrikeLocation; } set { _lightningStrikeLocation = value; } }

    [SerializeField]
    [Range(0.0f, 75.0f)] private float _proximityRange;

    [SerializeField] private Transform _spellCastLocation;
    [SerializeField] private GameObject _fireballPrefab;
    [SerializeField] private GameObject _lightningStrikePrefab;

    private Vector3 _targetRot;
    private float damping = 1.0f;
    private bool _inAttackCooldown = false;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerScript = _player.GetComponent<PlayerController>();

        _targetRot = new Vector3(this.transform.rotation.x, Random.Range(0, 360), this.transform.rotation.z);

         _AIState = AIState.LookAround;

        StartCoroutine(LookForPlayer());
    }

    void Update()
    {
        CheckHealth();

        switch (_AIState)
        {
            case AIState.LookAround:
                LookAround();

                if (_canSeePlayer == true)
                {
                    _AIState = AIState.Attack;
                }

                break;

            case AIState.Attack:
                Attack();

                /*if (_canSeePlayer == false)
                {
                    _AIState = AIState.LookAround;
                }*/

                break;
        }
        
    }

    private void LookAround()
    {
        if (_lookingAround == false) return;

        var step = 15f * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_targetRot), step);

        if (transform.rotation.eulerAngles == _targetRot)
        {
            _lookingAround = false;
            StartCoroutine(LookAroundCooldown());
        }
    }

    private void Attack()
    {
        RotateToPlayer();

        if (!_inAttackCooldown)
        {
            CastSpellAttack();
        }
    }

    private void CastSpellAttack()
    {
        _inAttackCooldown = true;

        StartCoroutine(AttackCooldown());

        int randomNumber = Random.Range(1, 10);

        if (randomNumber >= 7)
        {
            CastLightningStrike();
        }
        else
        {
            CastFireball();
        }
    }

    private void CastFireball()
    {
        var fireball = Instantiate(_fireballPrefab, _spellCastLocation.position, Quaternion.identity);

        FireballController tempFireballControl = fireball.GetComponent<FireballController>();

        tempFireballControl.Fire(_player.transform.position);
    }

    private void CastLightningStrike()
    {
        Transform spawnAt = _playerScript._lightningSpawnLocation;
        Instantiate(_lightningStrikePrefab, spawnAt.position, Quaternion.identity);
    }

    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
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

    private IEnumerator LookForPlayer()
    {
        while (true)
        {
            RaycastHit hit;

            var rayOrigin = this.transform.position;
            var rayTarget = _player.transform.position - this.transform.position;
            rayOrigin.y += 2f;
            rayTarget.y -= 2;
            Debug.DrawRay(rayOrigin, rayTarget, Color.green);

            if (Physics.Raycast(rayOrigin, rayTarget, out hit, _proximityRange))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    _canSeePlayer = true;
                }
                else
                {
                    _canSeePlayer = false;
                }
            }
            else
            {
                _canSeePlayer = false;
            }

            yield return new WaitForSeconds(0.2f);
        }

    }

    private IEnumerator LookAroundCooldown()
    {
        yield return new WaitForSeconds(3.5f);

        _targetRot = new Vector3(this.transform.rotation.x, Random.Range(0, 360), this.transform.rotation.z);

        _lookingAround = true;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(5.0f);

        _inAttackCooldown = false;
    }
}
