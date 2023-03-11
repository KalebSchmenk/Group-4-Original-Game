using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingPlayerController : MonoBehaviour
{
    private CharacterController _char;
    public float speed;

    void Start()
    {
        _char = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnGUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void Update()
    {

        if (Keyboard.current[Key.W].isPressed)
        {
            _char.Move(Vector3.forward * speed);
        }
        if (Keyboard.current[Key.S].isPressed)
        {
            _char.Move(Vector3.back * speed);
        }
        if (Keyboard.current[Key.A].isPressed)
        {
            _char.Move(Vector3.left * speed);
        }
        if (Keyboard.current[Key.D].isPressed)
        {
            _char.Move(Vector3.right * speed);
        }

        if (!Keyboard.current[Key.W].isPressed && !Keyboard.current[Key.A].isPressed && !Keyboard.current[Key.S].isPressed && !Keyboard.current[Key.D].isPressed)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DamageCollider"))
        {
            Debug.Log("Took damage!");
        }
    }
}
