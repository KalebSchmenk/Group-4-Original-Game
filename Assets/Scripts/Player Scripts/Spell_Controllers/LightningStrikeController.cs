using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightningStrikeController : MonoBehaviour
{
    [SerializeField] private int _damageOutput = 5;
    [SerializeField] private float _destroyIn = 2.5f;
    private void Awake()
    {
        StartCoroutine(DestroyIn());
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(_destroyIn);

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
