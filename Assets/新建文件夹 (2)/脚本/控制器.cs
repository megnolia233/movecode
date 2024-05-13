using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class 控制器 : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, jumping, down, downw;
    public string currentState;
    public float speed = 10f;
    public float movement;
    public Rigidbody2D rigidbody;
    public string currentAnimation;
    public float jumpSpeed = 10f;
    private Vector3 previousPosition;
    private bool isGrounded = false;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = "Idle";
        SetCharacterState(currentState);
        var state = skeletonAnimation.AnimationState;
        state.Complete += HandleAnimationComplete;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
        if (canMove)
        {
            Move();
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
            movement = 0;
        }
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        Spine.TrackEntry animationEntry = skeletonAnimation.state.SetAnimation(0, animation, loop);
        animationEntry.TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("walking"))
        {
            SetAnimation(walking, true, 2f);
        }
        else if (state.Equals("Jumping"))
        {
            SetAnimation(jumping, false, 2f);
        }
        else if (state.Equals("Down"))
        {
            SetAnimation(down, false, 2f);
        }
        else if (state.Equals("Downw"))
        {
            SetAnimation(downw, false, 1f);
            canMove = false;
        }
        else
        {
            SetAnimation(idle, true, 1f);
        }
        currentState = state;
    }

    public void Move()
    {
        movement = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(movement * speed, GetComponent<Rigidbody2D>().velocity.y);
        float verticalVelocity = rigidbody.velocity.y;
        Debug.Log(movement);
        Debug.Log(canMove);
        if (movement != 0)
        {
            if (!currentState.Equals("Jumping") && !currentState.Equals("Down") && !currentState.Equals("Downw"))
            {
                SetCharacterState("walking");
            }

            if (movement > 0.05)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if (movement < -0.05)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            else
            {
                SetCharacterState("idle");
                canMove = true;
            }
        }
        else
        {
            SetCharacterState("idle");
        }

        
       

        if (verticalVelocity > 0.1)
        {
            Debug.Log("不在地面上");
            SetCharacterState("Jumping");
        }
        else if (verticalVelocity < -0.1)
        {
            Debug.Log("不在地面上");
            SetCharacterState("Down");
        }
        else
        {
            Debug.Log("在地面上");
            if (isGrounded = true && Input.GetButtonDown("Jump"))
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // 触发与地面的碰撞
        {
            Debug.Log("pengdaodimianle");
            isGrounded = true; // 标记角色已落地

            if (currentState.Equals("Down"))
            {
                SetCharacterState("Downw");
                StartCoroutine(SetBoolAfterDelay(0.5f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground")) // 触发与地面的离开
        {
            Debug.Log("离开地面了");
            isGrounded = false; // 标记角色离开地面
        }
    }

    private void HandleAnimationComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name.Equals("Downw"))
        {
            canMove = true;
        }
    }
    IEnumerator SetBoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间

        canMove  = true; // 将值设置为0
        SetCharacterState("walking");
        Debug.Log("Value set to 0 after " + delay + " seconds."); // 打印消息以确认值已更改
    }

}
