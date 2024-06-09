using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 物理碰撞检查组件
public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    
    [Header("检测参数")] 
    public bool manual;
    // 底部碰撞检测偏移值
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;

    // 碰撞检测范围
    public float checkRaduis;

    // 碰撞层级
    public LayerMask groundLayer;

    [Header("状态")]
    // 是否处于地面状态
    public bool isGround;

    public bool touchLeftWall;
    
    public bool touchRightWall;

    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();

        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x/2) + coll.offset.x, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    private void Update() 
    {
        Check();
    }

    private void Check()
    {
        // 检查是否碰撞地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);

        // 墙体判断
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // 绘制碰撞检测线, 方便调试
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
        
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }

}
