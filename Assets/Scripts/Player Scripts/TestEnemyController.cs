using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : MonoBehaviour
{
    [SerializeField] private Transform _lightningStrikeLocation;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("LightningStrike"))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Destroy(this.gameObject);
        }
    }

    public Transform GetStrikeLocation()
    {
        return _lightningStrikeLocation;
    }
}
