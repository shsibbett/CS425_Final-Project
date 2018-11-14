using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

// basic WASD-style movement control
public class FpsMovement : MonoBehaviour
{
    [SerializeField] private Camera player_cam;

    public float speed = 6.0f;
    public float gravity = -9.8f;

    public float horizontal_sensitivity = 9.0f;
    public float vertical_sensitivity = 9.0f;

    public float vertical_minimum = -45.0f;
    public float vertical_maximum = 45.0f;

    private float vertical_rotation = 0;

    private CharacterController player_controller;

    void Start()
    {
        player_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveCharacter();
        RotateCharacter();
        RotateCamera();
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        player_controller.Move(movement);
    }

    private void RotateCharacter()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * horizontal_sensitivity, 0);
    }

    private void RotateCamera()
    {
        vertical_rotation -= Input.GetAxis("Mouse Y") * vertical_sensitivity;
        vertical_rotation = Mathf.Clamp(vertical_rotation, vertical_minimum, vertical_maximum);

        player_cam.transform.localEulerAngles = new Vector3(
            vertical_rotation, player_cam.transform.localEulerAngles.y, 0
        );
    }
}
