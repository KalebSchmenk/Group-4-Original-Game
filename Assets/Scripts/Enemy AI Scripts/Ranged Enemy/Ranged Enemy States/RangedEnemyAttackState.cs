using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RangedEnemyAttackState : RangedEnemyBaseState
{
    private RangedEnemyController _rangedEnemyScript;

    private GameObject _player;
    
    private bool _inAttackCooldown = true;

    private float _attackCooldownTime = 5.0f;

    public override void EnterState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        this._player = rangedEnemy._player;

        StartCoroutine(AttackCooldown());
    }

    public override void UpdateState(RangedEnemyController rangedEnemy)
    {
        this._rangedEnemyScript = rangedEnemy;

        RotateToPlayer();

        if (!_inAttackCooldown)
        {
            CastSpellAttack();
        }
    }

    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - _rangedEnemyScript.gameObject.transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        _rangedEnemyScript.gameObject.transform.rotation = Quaternion.Slerp(_rangedEnemyScript.gameObject.transform.rotation, rotation, Time.deltaTime * 5.0f);
    }


    private void CastSpellAttack()
    {
        StartCoroutine(AttackCooldown());

        int randomNumber = Random.Range(1, 10);

        if (randomNumber >= 8)
        {
            CastLightningStrike();
        }
        else
        {
            CastFireball();
        }
    }

    private IEnumerator AttackCooldown()
    {
        _inAttackCooldown = true;
        yield return new WaitForSeconds(_attackCooldownTime);
        _inAttackCooldown = false;
    }

    public void CastFireball()
    {
        var fireball = Instantiate(_rangedEnemyScript._fireballPrefab, _rangedEnemyScript._spellCastLocation.position, Quaternion.identity);

        FireballController tempFireballControl = fireball.GetComponent<FireballController>();

        tempFireballControl.Fire(_player.transform.position);
    }

    public void CastLightningStrike()
    {
        Transform spawnAt = _player.GetComponent<PlayerController>()._lightningSpawnLocation.transform;
        var randomRot = new Vector3(0, Random.Range(0, 360), 0);
        Instantiate(_rangedEnemyScript._lightningStrikePrefab, spawnAt.position, Quaternion.Euler(randomRot));
    }

}
