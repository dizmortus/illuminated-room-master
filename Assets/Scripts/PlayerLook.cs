using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The code in this script is based on a publically available YouTube tutorial produced by user Acacia Developer (2018)
 * For the full reference to this source, along with others used for this Design Challenge 02, see Documentation\Design Challenge 02\Sources\Design Challenge 02 Reference List.pdf
 * */

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private float m_mouseSensitivity = 150.0f; // Controls the amount the persective moves when the player moves their cursor

    [SerializeField]
    private Transform m_playerBody; // Reference to the player character's Transform component

    private float m_xAxisClamp = 0.0f; // Stores players accumulated view rotation on the x-axis

    // Locks the player's cursor immediately
    private void Awake()
    {
        LockCursor();
    }

	void Update()
    {
        CameraRotation();
	}

    // Locks the player's cursor to the centre of the screen and hides it from view
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Rotates the player's view based on their mouse cursor's movement
    private void CameraRotation()
    {
        // Gets and stores input from the player's mouse, scaling it based on mouse sensitivy and frame rate
        float _mouseX = Input.GetAxisRaw("Mouse X") * m_mouseSensitivity * Time.deltaTime; 
        float _mouseY = Input.GetAxisRaw("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        // Accumlates to the player's mouse cursor y-axis movement. If amount exceeds certain values, calls method to prevent further rotation in that direction
        m_xAxisClamp += _mouseY; 
        if (m_xAxisClamp > 90.0f)
        {
            m_xAxisClamp = 90.0f;
            _mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (m_xAxisClamp < -90.0f)
        {
            m_xAxisClamp = -90.0f;
            _mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        // Rotates the player's view
        transform.Rotate(Vector3.left * _mouseY);
        m_playerBody.Rotate(Vector3.up * _mouseX);
    }

    // Clamps the rotation on the x axis, preventing the the player from rotating 360 degrees on that axis
    private void ClampXAxisRotationToValue(float _value)
    {
        Vector3 _eulerRoation = transform.eulerAngles;
        _eulerRoation.x = _value;
        transform.eulerAngles = _eulerRoation;
    }
}
