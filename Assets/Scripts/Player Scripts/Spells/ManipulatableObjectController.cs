using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManipulatableObjectController : MonoBehaviour
{
    public PlayerInputActions _playerInput;
    private InputAction _levitate;
    private InputAction _push;
    private InputAction _pull;

    [SerializeField] private float _pullSpellCooldown = 2.5f;
    [SerializeField] private float _pushSpellCooldown = 2.5f;
    [SerializeField] private float _levitateSpellCooldown = 2.5f;

    private bool _pushInCooldown = false;
    private bool _pullInCooldown = false;
    private bool _levitateInCooldown = false;

    [Header("Levitate Sounds")]
    [SerializeField] AudioSource playerLevitateCastObject;
    [SerializeField] AudioClip  playerLevitateCastClip;

    [Header("Push Sounds")]
    [SerializeField] AudioSource playerPushCastObject;
    [SerializeField] AudioClip  playerPushCastClip;
   // [SerializeField] AudioSource playerPushHoldObject;
    //[SerializeField] AudioClip playerPushHoldSound;


    [Header("Pull Sounds")]
    [SerializeField] AudioSource playerPullCastObject;
    [SerializeField] AudioClip  playerPullCastClip;
    // [SerializeField] AudioSource playerPullHoldObject;
    // [SerializeField] AudioClip playerPullHoldSound;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _levitate = _playerInput.Player.LevitateSpell;
        _push = _playerInput.Player.PushSpell;
        _pull = _playerInput.Player.PullSpell;

        _levitate.Enable();
        _pull.Enable();
        _push.Enable();
    }
    private void OnDisable()
    {
        _levitate.Disable();
        _pull.Disable();
        _push.Disable();
    }

    private void Update()
    {
        if (_levitate.triggered && !_levitateInCooldown)
        {
            LevitateObject();
        }

        if (_push.triggered && !_pushInCooldown)
        {
            PushObject();
        }

        if (_pull.triggered && !_pullInCooldown)
        {

            PullObject();
        }
    }

    private void LevitateObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            targetObj.GetComponent<LevitateController>().BeginLevitating();

            playerLevitateCastObject.clip = playerLevitateCastClip;
            playerLevitateCastObject.Play();
            

            StartCoroutine(LevitateSpellCooldown());
        }
    }

    private void PushObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            try
            {
                targetObj.GetComponent<PushController>().Push(this.gameObject);
            }
            catch
            {}
            
            playerPushCastObject.clip = playerPushCastClip;
            playerPushCastObject.Play();
            
            StartCoroutine(PushSpellCooldown());
        }
    }

    private void PullObject()
    {
        GameObject targetObj = CastRay();

        if (targetObj != null)
        {
            try
            {
                targetObj.GetComponent<PullController>().Pull(this.gameObject);
            }
            catch
            { }

            playerPullCastObject.clip = playerPullCastClip;
            playerPullCastObject.Play();

            StartCoroutine(PullSpellCooldown());
        }
    }

    private GameObject CastRay()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("CanManipulate"))
            {
                return hit.transform.gameObject;
            }
            else
            {
                return null;
            }
        }

        return null;
    }


    private IEnumerator PullSpellCooldown()
    {
        _pullInCooldown = true;

        yield return new WaitForSeconds(_pullSpellCooldown);

        _pullInCooldown = false;
    }

    private IEnumerator PushSpellCooldown()
    {
        _pushInCooldown = true;

        yield return new WaitForSeconds(_pushSpellCooldown);

        _pushInCooldown = false;
    }

    private IEnumerator LevitateSpellCooldown()
    {
        _levitateInCooldown = true;

        yield return new WaitForSeconds(_levitateSpellCooldown);

        _levitateInCooldown = false;
    }
}
