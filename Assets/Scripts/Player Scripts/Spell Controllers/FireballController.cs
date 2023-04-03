using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private int _damageOutput = 3;
    [SerializeField] float _fireballSpeed = 750f;
    
    [SerializeField] Rigidbody rb;

    [SerializeField] private GameObject _fireballImpactSound;
    
    private Vector3 impactPosition;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerFireBallImpactObject;
    [SerializeField] AudioClip  playerFireBallImpactClip;

    public void Fire(Vector3 lookAt)
    {
        this.transform.LookAt(lookAt);

        rb.AddForce(transform.forward * _fireballSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthInterface script = collision.transform.gameObject.GetComponent<EnemyHealthInterface>();

            if (script != null) script.TakeDamage(_damageOutput);
        }

        impactPosition = transform.position;
        Instantiate(_fireballImpactSound, impactPosition, Quaternion.identity);
        playerFireBallImpactObject.clip = playerFireBallImpactClip;
        playerFireBallImpactObject.Play();

        Destroy(this.gameObject);
    }

}
