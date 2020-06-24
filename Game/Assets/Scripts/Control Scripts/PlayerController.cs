using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool fly = true;
    public float speed;
    public float jumpPower;
    public Transform cameraTransform;
    public float gravity = -9.81f;

    CharacterController controller;

    Vector3 velocity;

    bool grounded = false;
    bool jump = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fly)
        {
            runFlyControls();
        }
        else
        {
            runWalkControls();
        }
    }

    void runFlyControls()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = -transform.right * x + -transform.forward * z;

        float moveSpeed = speed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed *= 2;
        }

        if (x != 0 || z != 0)
        {
            transform.LookAt(new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z));
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            velocity.y = 1 * jumpPower;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            velocity.y = -1 * jumpPower;
        }
        else
        {
            velocity.y = 0;
        }

        controller.Move(velocity * Time.deltaTime);
    }
    
    void runWalkControls()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = -transform.right * x + -transform.forward * z;

        float moveSpeed = speed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed *= 2;
        }

        if (x != 0 || z != 0)
        {
            transform.LookAt(new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z));
            controller.Move(move * moveSpeed * Time.deltaTime);
        }

        if (!grounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) || jump)
            {
                velocity.y = jumpPower;
                jump = true;
            }
            else
            {
                velocity.y = 0;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Terrain")
        {
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        grounded = false;
        jump = false;
    }
}
