using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCameraControls : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float moveSpeed = 10f;
    public Transform controlledbody;

    float xRotation = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool moveCamera = Input.GetMouseButton(1);

        if (moveCamera)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            controlledbody.Rotate(Vector3.up * mouseX);
        }

        controlledbody.position += move * moveSpeed * Time.deltaTime;
    }
}
