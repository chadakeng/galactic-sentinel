using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float mouseSensitivity = 3.0f;  // Mouse sensitivity
    public Transform playerCamera;  // Drag Main Camera here

    private float xRotation = 0f; // Tracks up/down rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // Locks cursor
        Cursor.visible = false; // Hides cursor
    }

    void Update()
    {
        RotateView();
    }

    void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate Player left/right (Y-Axis)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate Camera up/down (X-Axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}