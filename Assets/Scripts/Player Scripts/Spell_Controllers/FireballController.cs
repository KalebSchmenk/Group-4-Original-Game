using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField] private int _damageOutput = 3;
    [SerializeField] float _fireballSpeed = 750f;
    
    [SerializeField] Rigidbody rb;


    public void Fire(Vector3 lookAt)
    {
        this.transform.LookAt(lookAt);

        rb.AddForce(transform.forward * _fireballSpeed);
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthInterface script = other.transform.gameObject.GetComponent<EnemyHealthInterface>();

            if (script != null) script.TakeDamage(_damageOutput);
        }
        Destroy(this.gameObject);
    }
}
