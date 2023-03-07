using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLightningStrikeController : MonoBehaviour
{
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
}
