using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;
    private InputAction move;
    Vector3 moveInput;
    Vector3 moveDirection;
    Rigidbody rb;
    private CharacterController controller;

    [SerializeField] private Texture2D _cursorTexture;
    private CursorMode _cursorMode = CursorMode.Auto;

    [SerializeField] float playerSpeed = 10.0f;
    [SerializeField] float rotateSpeed = 4f;
    private Transform cameraMainTransform;

    public Transform _lightningSpawnLocation;

    private float gravityValue = -9.81f;
    private bool onGround;
    


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
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

        onGround = controller.isGrounded;

        moveInput = move.ReadValue<Vector2>();
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
        moveDirection.y = 0f;

        /*if(moveInput != Vector3.zero){
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg +cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        }*/

        float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

        moveDirection.y += gravityValue * Time.deltaTime;

        controller.Move(moveDirection.normalized * Time.deltaTime * playerSpeed);
        

        
    }

    void FixedUpdate()
    {
        //controller.Move(moveDirection.normalized * Time.deltaTime * playerSpeed);
        
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {

        // Implement different damage values for each type of incoming damage. Damage collider is melee damage and should do least damage,
        // fireball damage is from a fireball and should do midrange damage, and lightning strike damage is from a lightning strike and should do a decent amount of damage
        if (other.gameObject.CompareTag("DamageCollider") || other.gameObject.CompareTag("Fireball") || other.gameObject.CompareTag("LightningStrike"))
        {
            Debug.Log("Took damage!");
        }
    }
}
