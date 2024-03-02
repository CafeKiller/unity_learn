using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 物理碰撞检查组件
public class PhysicsCheck : MonoBehaviour
{
    [Header("检测参数")]
    // 底部碰撞检测偏移值
    public Vector2 bottomOffset;

    // 碰撞检测范围
    public float checkRaduis;

    // 碰撞层级
    public LayerMask groundLayer;

    [Header("状态")]
    // 是否处于地面状态
    public bool isGround;

    private void Update() 
    {
        Check();
    }

    private void Check()
    {
        // 检查是否碰撞地面
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // 绘制碰撞检测线, 方便调试
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    }

}
