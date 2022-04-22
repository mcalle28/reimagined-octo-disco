using Mirror;
using UnityEngine;
using TMPro;

public class PlayerControl : Player
{
    public bool isMoveable;

    [SyncVar]
    public float speed;

    [SerializeField]
    private float cameraSize = 2.5f;

    private Vector3 dir;
    public Animator animator;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (hasAuthority)
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.transform.SetParent(transform);
                cam.transform.localPosition = new Vector3(0f, 0f, -10f);
                cam.orthographicSize = cameraSize;
            }
        }
    }

    public void SetCamera(Camera cam)
    {
        if (hasAuthority)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        bool isMoving = false;
        if (hasAuthority && isMoveable)
        {
            dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
            transform.position += dir * speed * Time.deltaTime;
            isMoving = dir.magnitude != 0f;
        }

        if (isMoving)
        {
            animator.SetFloat("moveX", dir.x);
            animator.SetFloat("moveY", dir.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
}
