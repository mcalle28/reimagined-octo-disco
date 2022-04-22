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


    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public Vector3 dir;

    public virtual void Start()
    {
        animator = this.GetComponent<Animator>();
        rigidbody2d = this.GetComponent<Rigidbody2D>();
        if (hasAuthority)
        {
            Camera cam = Camera.main;
            if (cam != null)
                SetCamera(cam);
        }
    }

    private void FixedUpdate()
    {
        if (hasAuthority)
            rigidbody2d.MovePosition(rigidbody2d.position + new Vector2(dir.x, dir.y) * speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
            AnimateMove();
    }

    private void AnimateMove()
    {
        bool isMoving = false;
        if (hasAuthority && isMoveable )
        {
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.y = Input.GetAxisRaw("Vertical");
            dir = dir.normalized;
            isMoving = dir.magnitude != 0f;

            if(dir.x != default)
                animator.SetFloat("moveX", dir.x);
            if(dir.y != default)
                animator.SetFloat("moveY", dir.y);
            animator.SetBool("moving", isMoving);
        }
    }

    private void SetCamera(Camera cam)
    {
        if (hasAuthority)
        {
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
        }
    }

}
