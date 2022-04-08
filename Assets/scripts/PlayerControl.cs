using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{

    public float speed;
    public Camera _playerCamera;
    private Rigidbody2D myRigidBody;
    private Vector3 change;
    private Animator animator;



    private void Start()
    {
        if (isLocalPlayer)
        {
            myRigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            _playerCamera = Camera.main;
            _playerCamera.orthographicSize=5; //Change in the future for mouse wheel zoom
        }
    }

    private void Update()
    {
        if (isLocalPlayer){
            change = Vector3.zero;
            change.x = Input.GetAxis("Horizontal");
            change.y = Input.GetAxis("Vertical");

            if (change != Vector3.zero){
                animator.SetFloat("moveX", change.x);
                animator.SetFloat("moveY", change.y);
                animator.SetBool("moving", true);
            }
            else{
                animator.SetBool("moving", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
            myRigidBody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (!isLocalPlayer || !_playerCamera) return;
        _playerCamera.transform.position = transform.position + 10 * Vector3.back;
    }
}
