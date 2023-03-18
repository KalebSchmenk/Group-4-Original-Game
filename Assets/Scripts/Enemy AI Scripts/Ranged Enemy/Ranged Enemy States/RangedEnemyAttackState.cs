using UnityEngine;

public class RangedEnemyAttackState : RangedEnemyBaseState
{
    private RangedEnemyController _rangedEnemyScript;

    private Transform _spellCastLocation;
    private GameObject _fireballPrefab;
    private GameObject _lightningStrikePrefab;

    private GameObject _player;
    
    private bool _inAttackCooldown = false;

    private float _attackCooldownTimeRemaining = 5.0f;

    public override void EnterState(RangedEnemyController rangedEnemy)
    {
        this._spellCastLocation = rangedEnemy._spellCastLocation;
        this._fireballPrefab = rangedEnemy._fireballPrefab;
        this._lightningStrikePrefab = rangedEnemy._lightningStrikePrefab;

        _player = GameObject.FindGameObjectWithTag("Player");

        this._rangedEnemyScript = rangedEnemy;
    }

    public override void UpdateState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        RotateToPlayer();

        if (!_inAttackCooldown)
        {
            CastSpellAttack();
        }
        else
        {
            AttackCooldown();
        }
    }

    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - _rangedEnemyScript.gameObject.transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        _rangedEnemyScript.gameObject.transform.rotation = Quaternion.Slerp(_rangedEnemyScript.gameObject.transform.rotation, rotation, Time.deltaTime * 1.0f);
    }


    private void CastSpellAttack()
    {
        _inAttackCooldown = true;

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

    private void AttackCooldown()
    {
        if (_attackCooldownTimeRemaining <= 0)
        {
            _inAttackCooldown = false;

            _attackCooldownTimeRemaining = 5.0f;
        }
        else
        {
            _attackCooldownTimeRemaining -= Time.deltaTime;
            return;
        }
    }


    private void CastFireball()
    {
        _rangedEnemyScript.CastFireball();
    }

    private void CastLightningStrike()
    {
        _rangedEnemyScript.CastLightningStrike();
    }

}
