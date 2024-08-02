using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float distance = 5.0f; // Initial distance from the player
    public float height = 2.0f; // Initial height above the player
    public float smoothSpeed = 0.125f; // Smoothing speed for following

    public float horizontalRotationSpeed = 70.0f; // Speed of horizontal rotation with A and D
    public float verticalRotationSpeed = 50.0f; // Speed of vertical rotation with W and S
    public float zoomSpeed = 2.0f; // Speed of zooming in/out
    public float minDistance = 2.0f; // Minimum zoom distance
    public float maxDistance = 10.0f; // Maximum zoom distance
    public float minVerticalAngle = -20.0f; // Minimum vertical angle
    public float maxVerticalAngle = 80.0f; // Maximum vertical angle

    private float currentDistance;
    private float currentHeight;
    private float currentHorizontalAngle;
    private float currentVerticalAngle;

    void Start()
    {
        // Initialize current values
        currentDistance = distance;
        currentHeight = height;
        currentHorizontalAngle = transform.eulerAngles.y;
        currentVerticalAngle = Mathf.Clamp(transform.eulerAngles.x, minVerticalAngle, maxVerticalAngle);
    }

    void LateUpdate()
    {
        // Handle horizontal rotation with A and D keys
        float horizontal = Input.GetAxis("Horizontal");
        currentHorizontalAngle += horizontal * horizontalRotationSpeed * Time.deltaTime;

        // Handle vertical rotation with W and S keys
        float vertical = -Input.GetAxis("Vertical"); // Inverted for typical camera control
        currentVerticalAngle += vertical * verticalRotationSpeed * Time.deltaTime;
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);

        // Handle zooming in and out with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * zoomSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Calculate the new position and offset
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
        Vector3 offset = new Vector3(0, 0, -currentDistance);
        Vector3 rotatedOffset = rotation * offset;

        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + rotatedOffset + new Vector3(0, height, 0);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Always look at the player
        transform.LookAt(player.position + Vector3.up * height);
    }
}