using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{

    public float speed;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;

    private Camera _playerCamera;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (isLocalPlayer)
        {
            _playerCamera = Camera.main;
        }
    }

    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        myRigidBody.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer || !_playerCamera) return;
        _playerCamera.transform.position = transform.position + 10 * Vector3.back;
    }
}