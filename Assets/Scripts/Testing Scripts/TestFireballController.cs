using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireballController : MonoBehaviour
{
    [SerializeField] float _fireballSpeed = 500.0f;
    
    [SerializeField] Rigidbody rb;


    public void Fire(Vector3 lookAt)
    {
        this.transform.LookAt(lookAt);

        rb.AddForce(transform.forward * _fireballSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
