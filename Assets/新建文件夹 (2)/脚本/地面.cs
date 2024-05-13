using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 地面 : MonoBehaviour
{
    public LayerMask groundLayer;

    private void Update()
    {
        CheckGrounded();
    }

    void CheckGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;

        float distance = 1.5f; // 调整此值以匹配角色和地面之间的距离

        Debug.DrawRay(position, direction, Color.green); // 在场景中绘制检测射线

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);

        if (hit.collider != null)
        {
            // 角色在地面上
            Debug.Log("在地面上");
        }
        else
        {
            // 角色不在地面上
            Debug.Log("不在地面上");
        }
    }
}
