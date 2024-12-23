using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The code in this script is based on a publically available YouTube tutorial produced by user Acacia Developer (2018)
 * For the full reference to this source, along with others used for this Design Challenge 02, see Documentation\Design Challenge 02\Sources\Design Challenge 02 Reference List.pdf
 * */

public class PlayerMove : MonoBehaviour
{
    private CharacterController m_charController; // Reference to the player character's CharacterController component

    [SerializeField]
    private float m_movementSpeed = 3.0f; // Controls how quickly the player moves in a direction
    [SerializeField]
    private string m_movementSound;

    private void Awake()
    {
        m_charController = GetComponent<CharacterController>(); // Gets and stores a reference to the player character's CharacterController component
    }

    private void Update()
    {
        PlayerMovement();
    }

    // Moves the player character in world based on user input
    void PlayerMovement()
    {
        // Gets and stores the player's vertical and horizontal input
        float _verticalInput = Input.GetAxis("Vertical");
        float _horizInput = Input.GetAxis("Horizontal");

        // Stores amount to move on player character's forward and right vectors
        Vector3 _forwardMovement = transform.forward * _verticalInput;
        Vector3 _rightMovement = transform.right * _horizInput;

        Vector3 _movementVector = Vector3.ClampMagnitude(_forwardMovement + _rightMovement, 1.0f);

        // Uses CharacterController component to move player based on forward and right vectors.
        m_charController.SimpleMove(_movementVector * m_movementSpeed);

        MovementSound(_movementVector);
    }

    void MovementSound(Vector3 _movementVector)
    {
        if (_movementVector != Vector3.zero)
        {
            AudioManager.instance.Play(m_movementSound);
        }
        else
        {
            AudioManager.instance.Stop(m_movementSound);
        }
    }
}
