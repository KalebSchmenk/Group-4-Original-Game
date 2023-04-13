using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    [Header("Plane Reference")]
    [SerializeField] private GameObject _planeRef;

    private GameObject _player;

    private bool _playerInSphere = false;

    void Start()
    {
        _planeRef.SetActive(false);

        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (_playerInSphere)
        {
            RotateToPlayer();
        }
    }
    private void RotateToPlayer()
    {
        var lookPos = _player.transform.position - _planeRef.transform.position;
        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        _planeRef.transform.rotation = Quaternion.Slerp(_planeRef.transform.rotation, rotation, Time.deltaTime * 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _planeRef.SetActive(true);
            _playerInSphere = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _planeRef.SetActive(false);
            _playerInSphere = false;
        }
    }
}
