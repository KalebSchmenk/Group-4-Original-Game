using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVineController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            Destroy(this.gameObject);
        }
    }
}
