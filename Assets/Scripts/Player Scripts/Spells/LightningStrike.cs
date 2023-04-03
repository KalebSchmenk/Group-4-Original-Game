using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class LightningStrike : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _lightningStrike;

    [SerializeField] GameObject _lightningStrikePrefab;
    [SerializeField] float _cooldownTime = 2.5f;
    private Camera _mainCam;
    private bool _lightningInCooldown = false;

    

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerLightningImpactObject;
    [SerializeField] AudioClip  playerLightningImpactClip;
    [SerializeField] private GameObject _lightningImpactSound;
    [SerializeField] AudioSource playerLightningCastObject;
    [SerializeField] AudioClip playerLightningCastClip;

    void Start()
    {
        _mainCam = Camera.main;    
    }
    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _lightningStrike = _playerInput.Player.LightningStrike;
        _lightningStrike.Enable();
    }
    private void OnDisable()
    {
        _lightningStrike.Enable();
    }

    void Update()
    {
        if (_lightningStrike.triggered && !_lightningInCooldown)
        {
            CastLightning();
        }
    }

    private void CastLightning()
    {
        RaycastHit hit;
        Ray ray = _mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit, 50))
        {
            var spawnLightningAt = hit.point;
            var randomRot = new Vector3(0, Random.Range(0, 360), 0);

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                Transform spawnAt = hit.transform.gameObject.GetComponent<EnemyHealthInterface>().hitLocation;
                Instantiate(_lightningStrikePrefab, spawnAt.position, Quaternion.Euler(randomRot));
            }
            else
            {
                Instantiate(_lightningStrikePrefab, spawnLightningAt, Quaternion.Euler(randomRot));
            }

            Instantiate(_lightningImpactSound, hit.point, Quaternion.identity);
            playerLightningImpactObject.clip = playerLightningImpactClip;
            playerLightningImpactObject.Play();

            StartCoroutine(LightningCooldown());
        }
    }

    private IEnumerator LightningCooldown()
    {
        Debug.Log("Lightning cooling down");

        _lightningInCooldown = true;

        yield return new WaitForSeconds(_cooldownTime);

        _lightningInCooldown = false;
    }
}
