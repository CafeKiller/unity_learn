using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;

    [HideInInspector]
    public Animator anima;
    
    [HideInInspector]
    public PhysicsCheck physicsCheck;
    
    [Header("基本参数")] 
    // 正常速度
    public float normalSpeed;
    
    // 追击速度
    public float chaseSpeed;

    // 当前速度
    [HideInInspector]
    public float currentSpeed;

    // 方向
    public Vector3 faceDir;

    // 记录将要攻击的对象
    public Transform attacker;

    // 受击力度
    public float hurtForce;

    [Header("检测")] 
    public Vector2 centerOffset;

    public Vector2 checkSize;

    public float checkDistance;

    public LayerMask attackLayer;
    
    [Header("计时器")] 
    public float waitTime;

    public float waitTimeCounter;
    public bool wait;

    public float losTime;
    public float lostTimeCounter;

    [Header("状态")] 
    public bool isHurt;

    public bool isDead;

    private BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;


    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
        lostTimeCounter = losTime;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        // 判断是否碰撞墙壁且面向墙壁
        /*if ((physicsCheck.touchLeftWall && faceDir.x < 0) 
            || (physicsCheck.touchRightWall && faceDir.x > 0))
        {
            // transform.localScale = new Vector3(faceDir.x, 1, 1);
            wait = true;
            anima.SetBool("walk", false);
        }*/
        
        currentState.LogicUpdate();
        
        TimeCounter();
    }

    private void FixedUpdate()
    {
        if(!isHurt && !isDead &&!wait)
            Move();
        
        currentState.PhysicsUpdate();
    }

    private void OnDisable()
    {
        currentState.OnExit();
    }

    protected virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    

    public void TimeCounter()
    {
        // 转身计时
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
        
        // 丢失目标计时器
        if (!FoundPlayer() && lostTimeCounter > 0)
        {
            lostTimeCounter -= Time.deltaTime;
        }
        /*else
        {
            lostTimeCounter = losTime;
        }*/
    }

    public bool FoundPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, 
                                    checkSize, 0, faceDir, checkDistance, attackLayer);
    }

    public void SwitchStatus(NPCStatus status)
    {
        var newStatus = status switch
        {
            NPCStatus.Patrol => patrolState,
            NPCStatus.Chase => chaseState,
            _ => null
        };

        currentState.OnExit();
        currentState = newStatus;
        currentState.OnEnter(this);
    }

    #region 事件执行方法
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        
        // 转身
        if (attackTrans.position.x - transform.position.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        if (attackTrans.position.x - transform.position.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        // 受伤被击退
        isHurt = true;
        anima.SetTrigger("hurt");
        var dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;
        rb.velocity = new Vector2(0, rb.velocity.y);
        StartCoroutine(OnHurt(dir));
    }
    
    private IEnumerator OnHurt(Vector2 dir)
    { 
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hurtForce / 10);
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 2;
        anima.SetBool("dead", true);
        isDead = true;
    }

    public void DestroyAfterAnimation()
    {
        // BUG : Animation Event 无法正确触发本方法
        // https://forum.unity.com/threads/animation-event-polymorphism-interpreted-as-duplication.1482300/
        Debug.Log("DestroyAfterAnimation BUG");
        Destroy(this.gameObject);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + (Vector3)centerOffset, 0.2f);
    }
}
