using UnityEngine;
using Random = UnityEngine.Random;


public class RangedEnemyLookAroundState : RangedEnemyBaseState
{
    private Vector3 _targetRot;

    private RangedEnemyController _rangedEnemyScript;

    private GameObject _player;

    private bool _lookingAround = true;
    private bool _canSeePlayer = false;

    private float _proximityRange = 35f;

    private float _rayTimeRemaining = 0.2f;
    private float _lookAroundTimeRemaining = 5.0f;

    public override void EnterState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        _player = GameObject.FindGameObjectWithTag("Player");

        _targetRot = new Vector3(_rangedEnemyScript.gameObject.transform.rotation.x, Random.Range(0, 360), _rangedEnemyScript.gameObject.transform.rotation.z);

        LookForPlayer();
    }

    public override void UpdateState(RangedEnemyController rangedEnemy)
    {
        LookAround();
        RayTimerCheck();

        if (_canSeePlayer)
        {
            rangedEnemy.AttackState.EnterState(rangedEnemy);
            rangedEnemy.currentState = rangedEnemy.AttackState;
        }
    }

    private void RayTimerCheck()
    {
        if (_rayTimeRemaining <= 0)
        {
            _rayTimeRemaining = 0.2f;

            LookForPlayer();
        }
        else
        {
            _rayTimeRemaining -= Time.deltaTime;
        }
    }

    private void LookAround()
    {
        if (_lookingAround == false)
        {
            LookAroundCooldown();
            return;
        }


        var step = 15f * Time.deltaTime;

        _rangedEnemyScript.gameObject.transform.rotation = Quaternion.RotateTowards(_rangedEnemyScript.gameObject.transform.rotation, Quaternion.Euler(_targetRot), step);

        if (_rangedEnemyScript.gameObject.transform.rotation.eulerAngles == _targetRot)
        {
            _lookingAround = false;
            LookAroundCooldown();
        }
    }

    private void LookAroundCooldown()
    {
        if (_lookAroundTimeRemaining <= 0)
        {
            _lookingAround = true;

            _lookAroundTimeRemaining = 5.0f;
        }
        else
        {
            _lookAroundTimeRemaining -= Time.deltaTime;
            return;
        }

        _targetRot = new Vector3(_rangedEnemyScript.gameObject.transform.rotation.x, Random.Range(0, 360), _rangedEnemyScript.gameObject.transform.rotation.z);

        _lookingAround = true;
    }

    private void LookForPlayer()
    {
        RaycastHit hit;

        var rayOrigin = _rangedEnemyScript.gameObject.transform.position;
        var rayTarget = _player.transform.position - _rangedEnemyScript.gameObject.transform.position;
        rayOrigin.y += 2f;
        rayTarget.y -= 2;

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
    }
}
