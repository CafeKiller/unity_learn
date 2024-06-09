using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected Animator anima;

    private PhysicsCheck physicsCheck;
    
    [Header("基本参数")] 
    // 正常速度
    public float normalSpeed;
    
    // 追击速度
    public float chaseSpeed;

    // 当前速度
    public float currentSpeed;

    // 方向
    public Vector3 faceDir;

    [Header("计时器")] 
    public float waitTime;

    public float waitTImeCounter;
    public bool wait;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        
        currentSpeed = normalSpeed;
        waitTImeCounter = waitTime;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // 判断是否碰撞墙壁且面向墙壁
        if ((physicsCheck.touchLeftWall && faceDir.x < 0) 
            || (physicsCheck.touchRightWall && faceDir.x > 0))
        {
            // transform.localScale = new Vector3(faceDir.x, 1, 1);
            wait = true;
            anima.SetBool("walk", false);
        }
        
        TimeCounter();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }

    public void TimeCounter()
    {
        // 转身计时
        if (wait)
        {
            waitTImeCounter -= Time.deltaTime;
            if (waitTImeCounter <= 0)
            {
                wait = false;
                waitTImeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }
}
