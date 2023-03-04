using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireballController : MonoBehaviour
{
    [SerializeField] float _fireballSpeed = 50000.0f;
    
    [SerializeField] Rigidbody rb;


    public void Fire(Vector3 lookAt)
    {
        this.transform.LookAt(lookAt);

        rb.AddForce(transform.forward * _fireballSpeed);
    }
}
