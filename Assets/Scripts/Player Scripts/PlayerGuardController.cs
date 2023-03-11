using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGuardController : MonoBehaviour
{
    [SerializeField] float _timeTillDestroy = 3.5f;


    private void Start()
    {
        StartCoroutine(DestroyIn());
    }


    void Update()
    {
        transform.localScale += new Vector3(0.025f, 0.025f, 0.025f);
    }

    private IEnumerator DestroyIn()
    {
        yield return new WaitForSeconds(_timeTillDestroy);

        Destroy(this.gameObject);
    }

}
