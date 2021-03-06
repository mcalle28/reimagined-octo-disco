using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using TMPro;


public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private TMP_Text text;

    public bool isMoveable;

    [SyncVar]
    public float speed;

    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public Vector3 dir;


    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        if (IsOwner && isMoveable)
        {
            Vector3 newScale = transform.localScale;
            if (dir.x < 0f && newScale.x > 0)
            {
                newScale.x *= -1;
                transform.localScale = newScale;
            }
            else if (dir.x > 0f && newScale.x < 0){
                newScale.x *= -1;
                transform.localScale = newScale;

            }
            rigidbody2d.MovePosition(rigidbody2d.position + new Vector2(dir.x, dir.y) * speed * Time.fixedDeltaTime);
        }
    }

    protected void Update()
    {
            AnimateMove();
    }

    private void AnimateMove()
    {
        if (IsOwner && isMoveable )
        {
            dir.x = Input.GetAxisRaw("Horizontal");
            dir.y = Input.GetAxisRaw("Vertical");
            dir = dir.normalized;
            bool isMoving = dir.magnitude != 0f;
            animator.SetBool("moving", isMoving);
        }
    }

}
