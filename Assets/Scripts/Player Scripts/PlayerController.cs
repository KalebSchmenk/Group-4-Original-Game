using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction pause;
    private InputAction changeSpellSelection;
    public Vector3 moveInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private CharacterController controller;
    private CursorMode _cursorMode = CursorMode.Auto;
    //[SerializeField] float rotateSpeed = 4f;
    private Transform cameraMainTransform;
    private float gravityValue = -9.81f;
    private bool onGround;
    private float verticalVelocity;
    private float impactTimer;
    private float jumpSpeed;
    private float currentHealth;
    float normSize;
    bool isInvincible;
    bool isJumping = false;
    
    public Animator _anim;

    private List<MonoBehaviour> _listOfCombatSpells = new List<MonoBehaviour>();
    private ManipulatableObjectController _telekensisSpellContainer;

    [SerializeField] bool _isHubLevel = false;

    [Header("Player Info")]
    [SerializeField] float invincibilityFramesDuration = 1.5f;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] float jumpForce = 1.0f;
    [SerializeField] float playerSpeed = 10.0f;
    public bool _onPlatform = false;
    [SerializeField] float slopeRayDistance = 0.2f;
    public bool _isMoving;
    public bool _gameOver;
    public bool _win;

    [Header("Healthbar References")]
    public Image healthMask;

    [Header("Misc")]
    public Transform _lightningSpawnLocation;
    [SerializeField] private Texture2D _cursorTexture;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerHurtObject;
    [SerializeField] AudioClip playerHurtClip;
    [SerializeField] GameObject walkingSound;
    [SerializeField] AudioSource jumpSoundObject;
    [SerializeField] AudioClip jumpSoundClip;
    [SerializeField] AudioSource jumpLandObject;
    [SerializeField] AudioClip jumpLandClip;
    bool playLandingSound = false;


    [Header("Menu Items")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] TMP_Text spellActive;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        currentHealth = maxHealth;
        normSize = healthMask.rectTransform.rect.width;
        Cursor.visible = false;

        _listOfCombatSpells.Add(this.gameObject.GetComponent<LightningStrike>());
        _listOfCombatSpells.Add(this.gameObject.GetComponent<FireballSpell>());
        _listOfCombatSpells.Add(this.gameObject.GetComponent<PlayerGuardSpell>());

        _telekensisSpellContainer = this.gameObject.GetComponent<ManipulatableObjectController>();

        if (_telekensisSpellContainer != null) _telekensisSpellContainer.enabled = false;
        Time.timeScale = 1f;
}

    void Awake()
    {
        playerControls = new PlayerInputActions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_cursorTexture, Vector2.zero, _cursorMode);
    }


    void Update()
    {
        // Switches spell from combat to telekenesis and back again
        if (changeSpellSelection.triggered && _isHubLevel == false)
        {
            if (_telekensisSpellContainer.enabled == true)
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = true;
                }

                spellActive.text = "Active Spell Type: Combat";
                _telekensisSpellContainer.enabled = false;
            }
            else
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = false;
                }

                spellActive.text = "Active Spell Type: Telekinesis ";
                _telekensisSpellContainer.enabled = true;
            }
        }


        //Walking Stuff
        moveInput = move.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
        moveDirection.y = 0f;

        _anim.SetFloat("Xaxis", moveInput.x, 0.1f, Time.deltaTime);
        _anim.SetFloat("Yaxis", moveInput.y, 0.1f, Time.deltaTime);

        if (!_onPlatform)
        {
            if (onGround == true && verticalVelocity < 0)
            {
                verticalVelocity = 0f;
            }
            verticalVelocity += gravityValue * Time.deltaTime;
        }
        

        //moveDirection.y = verticalVelocity;


        Quaternion rotation = Quaternion.Euler(0f, cameraMainTransform.eulerAngles.y, 0f);
        transform.rotation = rotation;

        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * playerSpeed;


        //Jumping Stuff
        onGround = controller.isGrounded;
        if(onGround){
            impactTimer = 0.2f;
            if(playLandingSound){
                jumpLandObject.clip = jumpLandClip;
                jumpLandObject.Play();
                playLandingSound = false;
            }
            
        }

        if(impactTimer > 0){
            impactTimer -= Time.deltaTime;
        }

        if(jump.triggered && !isJumping){
            if(impactTimer > 0){

                _anim.SetTrigger("Jump");
                StartCoroutine(JumpDelay());

            }
        }

        
 

        //What actually moves the player
        moveDirection = moveDirection.normalized;
        Vector3 moving = moveDirection * magnitude;
        
        moving = ChangeVelocityToSlope(moving);
        moving.y += verticalVelocity;

        
        
        //controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        controller.Move(moving * playerSpeed * Time.deltaTime);


        //Updates Health UI element
        HealthVisual(currentHealth/maxHealth);

        if(currentHealth <= 0){
            _gameOver = true;
            //transform.position = CheckpointManager._currentCheckpoint.position;
            //currentHeath = maxHealth;
        }

        
        if(moving.x != 0){
            _isMoving = true;
        }
        else{
            _isMoving = false;
        }

        if(onGround && _isMoving){
            walkingSound.SetActive(true);
        }
        else{
            walkingSound.SetActive(false);
        }

    }


    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
 
        jump.Enable();
        move.Enable();
   


        changeSpellSelection = playerControls.Player.SwitchSpell;
        changeSpellSelection.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        changeSpellSelection.Disable();
 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Implement different damage values for each type of incoming damage. Damage collider is melee damage and should do least damage,
        // fireball damage is from a fireball and should do midrange damage, and lightning strike damage is from a lightning strike and should do a decent amount of damage
        if (other.gameObject.CompareTag("DamageCollider") || other.gameObject.CompareTag("LightningStrike"))
        {
            //Debug.Log("Took damage!");
            if(other.gameObject.CompareTag("DamageCollider"))
            {
                if(isInvincible == false){
                    currentHealth -= 10;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());

                }
            }
            if(other.gameObject.CompareTag("LightningStrike"))
            {
                if(isInvincible == false){
                    currentHealth -= 30;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());
                }
            }

        }
        //REMOVE
        if(other.gameObject.CompareTag("InstaKill")){
            currentHealth = 0;
        }

        if(other.gameObject.CompareTag("Win")){
            _win = true;
        }
        //REMOVE
    }

    // May eventually change firball to be a big trigger on explosion
    // So this may go back in to trigger enter 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            if (isInvincible == false)
            {
                currentHealth -= 15;
                playerHurtObject.clip = playerHurtClip;
                playerHurtObject.Play();
                StartCoroutine(InvincibilityFrames());
            }
        }
    }

    //Controls health UI element
    public void HealthVisual(float amount){
        healthMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normSize * amount);
    }

    private IEnumerator InvincibilityFrames(){
        Debug.Log("Invincible");
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityFramesDuration);
        isInvincible = false;
        Debug.Log("Not Invincible");
    }

    private Vector3 ChangeVelocityToSlope(Vector3 velocity){
        var slopeRay = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(slopeRay, out RaycastHit hitInfo, slopeRayDistance)){
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var slopeVelocity = slopeRotation * velocity;

            if(slopeVelocity.y < 0){
                return slopeVelocity;
            }
        }

        return velocity;
    }
    

    public void Heal()
    {
        currentHealth += 45;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    private IEnumerator JumpDelay()
    {
        isJumping = true;

        yield return new WaitForSeconds(0.5f);

        isJumping = false;
        impactTimer = 0;
        jumpSoundObject.clip = jumpSoundClip;
        jumpSoundObject.Play();
        playLandingSound = true;
        verticalVelocity += Mathf.Sqrt(jumpForce * 1.75f * Mathf.Abs(gravityValue));
    }
}
