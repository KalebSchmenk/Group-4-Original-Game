using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVineController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fireball"))
        {
            Destroy(this.gameObject);
        }
    }
}
