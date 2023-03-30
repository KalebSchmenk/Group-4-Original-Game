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
    Vector3 moveInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private CharacterController controller;

    [SerializeField] private Texture2D _cursorTexture;
    private CursorMode _cursorMode = CursorMode.Auto;

    [SerializeField] float playerSpeed = 10.0f;
    //[SerializeField] float rotateSpeed = 4f;
    private Transform cameraMainTransform;

    public Transform _lightningSpawnLocation;

    private float gravityValue = -9.81f;
    private bool onGround;

    private float verticalVelocity;
    [SerializeField] float jumpForce = 1.0f;
    private float impactTimer;
    private float jumpSpeed;
    [SerializeField] private float maxHealth = 100;
    private float currentHeath;
    public Image healthMask;
    float normSize;
    bool isInvincible;
    [SerializeField] float invincibilityFramesDuration = 1.5f;


    public bool _onPlatform = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        currentHeath = maxHealth;
        normSize = healthMask.rectTransform.rect.width;
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
        // DELETE ON MAIN BUILD! REPLACE WITH PAUSE MENU!
        if (Keyboard.current[Key.Escape].isPressed)
        {
            Application.Quit();
            Debug.Log("GAME QUIT");
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
        
        moving.y = verticalVelocity;
        
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
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Enable();
    }

    private void OnTriggerEnter(Collider other)
    {

        // Implement different damage values for each type of incoming damage. Damage collider is melee damage and should do least damage,
        // fireball damage is from a fireball and should do midrange damage, and lightning strike damage is from a lightning strike and should do a decent amount of damage
        if (other.gameObject.CompareTag("DamageCollider") || other.gameObject.CompareTag("Fireball") || other.gameObject.CompareTag("LightningStrike"))
        {
            //Debug.Log("Took damage!");
            if(other.gameObject.CompareTag("DamageCollider")){
                if(isInvincible == false){
                    currentHeath -= 10;
                    StartCoroutine(InvincibilityFrames());
                }
            }
            if(other.gameObject.CompareTag("Fireball")){
                if(isInvincible == false){
                    currentHeath -= 30;
                    StartCoroutine(InvincibilityFrames());
                }
            }
            if(other.gameObject.CompareTag("LightningStrike")){
                if(isInvincible == false){
                    currentHeath -= 50;
                    StartCoroutine(InvincibilityFrames());
                }
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

    
}
