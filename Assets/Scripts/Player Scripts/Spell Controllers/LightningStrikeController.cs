using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightningStrikeController : MonoBehaviour
{
    [SerializeField] private int _damageOutput = 4;
    [SerializeField] private float _destroyIn = 2.5f;

    [SerializeField] private GameObject _lightningSoundPrefab;
    [SerializeField] private GameObject _explosionEffect;

    private GameObject particleRef;

    private void Awake()
    {
        Instantiate(_lightningSoundPrefab, transform.position, Quaternion.identity);
        particleRef = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(_destroyIn);

        Destroy(particleRef);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthInterface enemyHealthScript = other.gameObject.GetComponent<EnemyHealthInterface>();

            if (enemyHealthScript != null)
            {
                enemyHealthScript.TakeDamage(_damageOutput);
            }
            else
            {
                Debug.LogError("Enemy script not found!");
            }
        }


    }
}
