using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{   
    // Fields
    [SerializeField] float moveSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource stepSound;
    
    // Movement
    private CharacterController controller;
    private bool isMoving = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Get input values
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed * Time.deltaTime;

        // Move the character using CharacterController
        controller.Move(movement);
        controller.Move(9.81f * Vector3.down * Time.deltaTime);

        // Rotate player to face movement direction
        if (movement != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10.0f);
            isMoving = true;
            animator.SetBool("Walking", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("Walking", false);
        }
        // Audio
        if (isMoving) {
            if (!stepSound.isPlaying) {
                stepSound.Play();
            }
        } else {
            stepSound.Stop();
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
