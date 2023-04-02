using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    private InputAction jump;
    private InputAction pause;
    private InputAction changeSpellSelection;
    Vector3 moveInput;
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
    private float currentHeath;
    float normSize;
    bool isInvincible;

    private List<MonoBehaviour> _listOfCombatSpells = new List<MonoBehaviour>();
    private ManipulatableObjectController _telekensisSpellContainer;


    [Header("Player Info")]
    [SerializeField] float invincibilityFramesDuration = 1.5f;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] float jumpForce = 1.0f;
    [SerializeField] float playerSpeed = 10.0f;
    public bool _onPlatform = false;
    [SerializeField] float slopeRayDistance = 0.2f;
    public bool _isMoving;
    

    [Header("Healthbar References")]
    public Image healthMask;

    [Header("Misc")]
    public Transform _lightningSpawnLocation;
    [SerializeField] private Texture2D _cursorTexture;

    [Header("Player Sounds")]
    [SerializeField] AudioSource playerHurtObject;
    [SerializeField] AudioClip playerHurtClip;
    [SerializeField] GameObject walkingSound;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        currentHeath = maxHealth;
        normSize = healthMask.rectTransform.rect.width;
        Cursor.visible = false;

        _listOfCombatSpells.Add(this.gameObject.GetComponent<LightningStrike>());
        _listOfCombatSpells.Add(this.gameObject.GetComponent<FireballSpell>());
        _listOfCombatSpells.Add(this.gameObject.GetComponent<PlayerGuardSpell>());

        _telekensisSpellContainer = this.gameObject.GetComponent<ManipulatableObjectController>();

        _telekensisSpellContainer.enabled = false;
}

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(_cursorTexture, Vector2.zero, _cursorMode);
    }

    void Update()
    {
        // Switches spell from combat to telekenesis and back again
        if (changeSpellSelection.triggered)
        {
            if (_telekensisSpellContainer.enabled == true)
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = true;
                }

                _telekensisSpellContainer.enabled = false;
            }
            else
            {
                foreach (MonoBehaviour script in _listOfCombatSpells)
                {
                    script.enabled = false;
                }

                _telekensisSpellContainer.enabled = true;
            }
        }

        // DELETE ON MAIN BUILD! REPLACE WITH PAUSE MENU!
            if (Keyboard.current[Key.Escape].isPressed)
        {
            Application.Quit();
            //Debug.Log("GAME QUIT");
        }


        //Walking Stuff
        moveInput = move.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
        moveDirection.y = 0f;

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
            
        }

        if(impactTimer > 0){
            impactTimer -= Time.deltaTime;
        }

        if(jump.triggered){
            if(impactTimer > 0){
                impactTimer = 0;
                verticalVelocity += Mathf.Sqrt(jumpForce * 1.75f * Mathf.Abs(gravityValue));
                

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
        HealthVisual(currentHeath/maxHealth);

        if(currentHeath <= 0){
            Debug.Log("Dead");

            transform.position = CheckpointManager._currentCheckpoint.position;
            currentHeath = maxHealth;
        }

        //Debug.Log("Current HP: " +currentHeath +"/" + maxHealth);
        
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

        if(pause.triggered){
            
        }


    }

    void FixedUpdate()
    {
        //controller.Move(moveDirection.normalized * Time.deltaTime * playerSpeed);
        
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        jump.Enable();
        move.Enable();
        pause = playerControls.Player.Pause;
        pause.Enable();

        changeSpellSelection = playerControls.Player.SwitchSpell;
        changeSpellSelection.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        pause.Disable();
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
                    currentHeath -= 10;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());

                }
            }
            if(other.gameObject.CompareTag("LightningStrike"))
            {
                if(isInvincible == false){
                    currentHeath -= 50;
                    //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
                    playerHurtObject.clip = playerHurtClip;
                    playerHurtObject.Play();
                    StartCoroutine(InvincibilityFrames());
                }
            }
        }
    }

    // May eventually change firball to be a big trigger on explosion
    // So this may go back in to trigger enter 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Fireball"))
        {
            if (isInvincible == false)
            {
                currentHeath -= 30;
                //playerHurtObject.PlayOneShot(playerHurtClip, 1f);
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

    
}
