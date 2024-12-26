using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController m_charController;

    [SerializeField]
    private float m_movementSpeed = 3.0f;
    [SerializeField]
    private string m_movementSound;

    public bool isMovementBlocked = false; // Флаг блокировки движения

    private void Awake()
    {
        m_charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!isMovementBlocked) // Проверяем флаг блокировки движения
        {
            PlayerMovement();
        }
    }

    void PlayerMovement()
    {
        float _verticalInput = Input.GetAxis("Vertical");
        float _horizInput = Input.GetAxis("Horizontal");

        Vector3 _forwardMovement = transform.forward * _verticalInput;
        Vector3 _rightMovement = transform.right * _horizInput;

        Vector3 _movementVector = Vector3.ClampMagnitude(_forwardMovement + _rightMovement, 1.0f);

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
