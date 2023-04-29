using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction pause;
    private InputAction sprint;
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
    [SerializeField] float currentHealth;
    float normSize;
    bool isInvincible;
    bool isJumping = false;
    
    public Animator _anim;
    public CinemachineFreeLook cinemachineFL;
    private float normalFOV;
    [SerializeField] float sprintFOV;
    

    private List<MonoBehaviour> _listOfCombatSpells = new List<MonoBehaviour>();
    private ManipulatableObjectController _telekensisSpellContainer;

    [SerializeField] bool _isHubLevel = false;
    [SerializeField] bool _isCombatLevel = false;

    [Header("Player Info")]
    [SerializeField] float invincibilityFramesDuration = 1.5f;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] float jumpForce = 1.0f;
    float playerSpeed = 2.2f;
    float playerMoveSpeed = 2.2f;
    [SerializeField] float playerSprintSpeed = 4.4f;
    [SerializeField] float playerWalkSpeed = 2.2f;

    public bool _onPlatform = false;
    [SerializeField] float slopeRayDistance = 0.2f;
    public bool _isMoving;
    public bool _gameOver;
    public bool _win;
    bool isSprinting;

    [Header("Healthbar References")]
    public Image healthMask;

    [Header("Misc")]
    public Transform _lightningSpawnLocation;
    [SerializeField] private Texture2D _cursorTexture;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerHurtObject;
    [SerializeField] AudioClip playerHurtClip;
    [SerializeField] GameObject walkingSound;
    [SerializeField] AudioSource walkingSoundObject;
    [SerializeField] AudioClip walkingSoundClip;
    [SerializeField] AudioClip sprintingSoundClip;
    [SerializeField] GameObject sprintingSound;
    [SerializeField] AudioSource sprintingSoundObject;
    [SerializeField] AudioSource jumpSoundObject;
    [SerializeField] AudioClip jumpSoundClip;
    [SerializeField] AudioSource jumpLandObject;
    [SerializeField] AudioClip jumpLandClip;
    bool playLandingSound = false;
    [SerializeField] AudioSource healSound;
    [SerializeField] AudioClip healClip;


    [Header("Menu Items")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] GameObject winMenu;

    [Header("Spell Type Displays")]
    [SerializeField] GameObject _combatDisplay;
    [SerializeField] GameObject _telekinesisDisplay;
    bool respawn;
    private GameOverController goController;
    private PauseMenuController pauseController;
    bool _goTrigger = true;
    float goDelay = 5.0f;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        goController = this.gameObject.GetComponent<GameOverController>();
        pauseController = this.gameObject.GetComponent<PauseMenuController>();
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
        walkingSoundObject.clip = walkingSoundClip;
        sprintingSoundObject.clip = sprintingSoundClip;
        playerMoveSpeed = playerWalkSpeed;
        
}

    void Awake()
    {
        playerControls = new PlayerInputActions();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_cursorTexture, Vector2.zero, _cursorMode);
        normalFOV = cinemachineFL.m_Lens.FieldOfView;
    }


    void Update()
    {

        //Debug.Log(currentHealth);
        sprint.started += Sprinting;
        sprint.canceled += NotSprinting;
        // Switches spell from combat to telekenesis and back again
        if (changeSpellSelection.triggered && _isHubLevel == false && _isCombatLevel == false)
        {
            if (_telekensisSpellContainer.enabled == true)
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = true;
                }

                _combatDisplay.SetActive(true);
                _telekinesisDisplay.SetActive(false);
                _telekensisSpellContainer.enabled = false;
            }
            else
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = false;
                }

                _telekinesisDisplay.SetActive(true);
                _combatDisplay.SetActive(false);
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

            _anim.SetBool("IsGrounded", true);
            _anim.SetBool("IsJumping", false);
            isJumping = false;
            _anim.SetBool("IsFalling", false);

            if(playLandingSound){
                jumpLandObject.clip = jumpLandClip;
                jumpLandObject.Play();
                playLandingSound = false;
            }
            
        }
        else
        {
            _anim.SetBool("IsGrounded", false);

            if((isJumping && verticalVelocity < 0) || verticalVelocity < -2)
            {
                _anim.SetBool("IsFalling", true);
            }
        }

        if(impactTimer > 0){
            impactTimer -= Time.deltaTime;
        }

        if(jump.triggered && !isJumping){
            if(impactTimer > 0){

                //StartCoroutine(JumpDelay());
                _anim.SetBool("IsJumping", true);
                isJumping = true;
                impactTimer = 0;
                jumpSoundObject.clip = jumpSoundClip;
                jumpSoundObject.Play();
                playLandingSound = true;
                verticalVelocity += Mathf.Sqrt(jumpForce * 1.75f * Mathf.Abs(gravityValue));

            }
        }

        if(goDelay > 0){
            goDelay -= Time.deltaTime;
        }
 

        //What actually moves the player
        moveDirection = moveDirection.normalized;
        Vector3 moving = moveDirection * magnitude * playerMoveSpeed;
        
        moving = ChangeVelocityToSlope(moving);
        moving.y += verticalVelocity;

        
        
        //controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        controller.Move(moving * playerSpeed * Time.deltaTime);


        //Updates Health UI element
        HealthVisual(currentHealth/maxHealth);

        if(currentHealth <= 0){
            if(_goTrigger == true && goDelay <= 0){
                goController.GameOver();
                _goTrigger = false;
                //currentHealth = maxHealth;
            }

        }

        
        if(moving.x != 0){
            _isMoving = true;
        }
        else{
            _isMoving = false;
        }

        if(onGround && _isMoving){
            if(isSprinting){
                walkingSound.SetActive(false);
                sprintingSound.SetActive(true);
                
            }
            else{
                walkingSound.SetActive(true);
                sprintingSound.SetActive(false);
            }

        }
        else{
            walkingSound.SetActive(false);
            sprintingSound.SetActive(false);
            //walkingSoundObject.Stop();
        }

        if(isSprinting == true){
            if(cinemachineFL.m_Lens.FieldOfView <= sprintFOV){
                cinemachineFL.m_Lens.FieldOfView += 10 * Time.deltaTime;
            }
        }
        else{
            if(cinemachineFL.m_Lens.FieldOfView >= normalFOV){
                cinemachineFL.m_Lens.FieldOfView -= 20 * Time.deltaTime;;
            }
        }

        if(respawn){
            transform.position = CheckpointManager._currentCheckpoint.position;
            currentHealth = maxHealth;
            respawn = false;
        }

    }


    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        sprint = playerControls.Player.Sprint;
 
        jump.Enable();
        move.Enable();
        sprint.Enable();
   


        changeSpellSelection = playerControls.Player.SwitchSpell;
        changeSpellSelection.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        changeSpellSelection.Disable();
        sprint.Disable();
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CanManipulate")) // Rock falling on player damage
        {
            Debug.Log(collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 4.5f)
            {
                if (isInvincible == false)
                {
                    currentHealth -= 10;
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Implement different damage values for each type of incoming damage. Damage collider is melee damage and should do least damage,
        // fireball damage is from a fireball and should do midrange damage, and lightning strike damage is from a lightning strike and should do a decent amount of damage
        if (other.gameObject.CompareTag("DamageCollider") || other.gameObject.CompareTag("LightningStrike") || other.gameObject.CompareTag("Fireball") || other.gameObject.CompareTag("BossDamageCollider"))
        {
            // RIGHT HERE!!!

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
            if (other.gameObject.CompareTag("BossDamageCollider"))
            {
                if (isInvincible == false)
                {
                    currentHealth -= 25;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());

                }
            }
            if (other.gameObject.CompareTag("LightningStrike"))
            {
                if(isInvincible == false){
                    currentHealth -= 30;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());
                }
            }
            if (other.gameObject.CompareTag("Fireball"))
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
        //REMOVE
        if(other.gameObject.CompareTag("InstaKill")){
             if(_goTrigger == true && goDelay <= 0){
                currentHealth = 0;
             }
        }

        if(other.gameObject.CompareTag("Win")){
            _win = true;
        }
        //REMOVE
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
        healSound.PlayOneShot(healClip);
        currentHealth += 45;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    /*private IEnumerator JumpDelay()
    {
        isJumping = true;

        yield return new WaitForSeconds(0.5f);

        isJumping = false;
        impactTimer = 0;
        jumpSoundObject.clip = jumpSoundClip;
        jumpSoundObject.Play();
        playLandingSound = true;
        verticalVelocity += Mathf.Sqrt(jumpForce * 1.75f * Mathf.Abs(gravityValue));
    }*/

    private void Sprinting(InputAction.CallbackContext context){
        if(onGround){
            playerMoveSpeed = playerSprintSpeed;
            
            isSprinting = true;
        }
    }

    private void NotSprinting(InputAction.CallbackContext context){
        playerMoveSpeed = playerWalkSpeed;
        isSprinting = false;
        
    }

    /*private void SprintingFOVChange(string sprint){
        if(sprint == "sprinting"){
            cinemachineFL.m_Lens.FieldOfView = sprintFOV;
        }

        if(sprint == "not sprinting"){
            cinemachineFL.m_Lens.FieldOfView = normalFOV;
        }
    }*/


    public void Restart(){
        pauseController.ButtonPressSound();
        currentHealth = maxHealth;
        respawn = true;
        _goTrigger = true;  
        goDelay = 5f;
    }

    
}
