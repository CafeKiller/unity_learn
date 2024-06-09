using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // PlayerInputControl 是 unity 提供的全新的输入系统
    public PlayerInputControl inputControl;

    // rb 即角色 2D 刚体组件
    private Rigidbody2D rb;

    // playerAnimation 玩家动画组件
    private PlayerAnimation playerAnimation;

    // inputDirection 用于保存输入时产生的变量值
    public Vector2 inputDirection;

    // physicsCheck 自定义的物理碰撞检测组件
    public PhysicsCheck physicsCheck;

    private Collider2D coll;

    [Header("基本参数")]
    // speed 角色运动速度基数, 可以直接在 unity 编辑器中直接给定初始值
    // 此处默认设置为290
    public float speed;
    
    // jumpForce 跳跃力度
    public float jumpForce;
    public float hurtForce;

    [Header("物理材质")] 
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;

    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    // 生命周期函数，在 Start() 之前调用, 类似于初始化构建
    // 如果游戏对象在启动期间处于非活动状态，则在激活之后才会调用 Awake.
    private void Awake() 
    {
        // 在此处获取角色上的 2D 刚体组件
        rb = GetComponent<Rigidbody2D>();

        // 在此处获取角色上的 PhysicsCheck 组件
        physicsCheck = GetComponent<PhysicsCheck>();

        // 获取角色上的 PlayerAnimation 组件
        playerAnimation = GetComponent<PlayerAnimation>();
        
        coll = GetComponent<Collider2D>();
        
        // 创建 inputSystem
        inputControl = new PlayerInputControl();


        // 此处使用事件委托的机制 绑定一个 Jump 函数
        inputControl.Gameplayer.Jump.started += Jump;

        // 注意此处 其实可以使用语法糖的方式编写, 但为了可读性和模块清晰, 此处不使用
        // inputControl.Gameplayer.Jump.started += 
        //    (InputAction.CallbackContext cont) => {
        //      ......    
        //    };
        
        // TODO 强制走路
        
        // 攻击相关
        inputControl.Gameplayer.Attack.started += PlayerAttack;
    }

    // 生命周期函数, 在启动对象后立即调用
    private void OnEnable() 
    {
        // 启用 inputSystem
        inputControl.Enable();
    }

    // 生命周期函数, 在对象处于禁用或非激活状态时调用
    private void OnDisable() 
    {
        // 禁用 inputSystem
        inputControl.Disable();
    }

    private void Update() 
    {
        // 在玩家进行操作时, 将对应数组从输入系统取出
        inputDirection = inputControl.Gameplayer.Move.ReadValue<Vector2>();
        
        CheckState();
    }

    // 生命周期函数, 与 Update 类似, 但有一点不同 FixedUpdate 不受环境机器的帧率影响
    // 其是以一个固定帧率为周期执行调用的, 主要处理物理模拟或与时间相关的逻辑
    private void FixedUpdate()
    {
        if (!isHurt && !isAttack) Move();
        
    }

    /// <summary>
    /// Move 角色移动处理函数, 负责处理角色在左右移动时的逻辑处理
    /// </summary>
    private void Move()
    {
        // 此处通过角色中的 rb 对象进行移动操作
        // (speed * Time.deltaTime) * inputDirection.x 这样的写法可以减少向量的计算次数
        rb.velocity = new Vector2( (speed * Time.deltaTime) * inputDirection.x, rb.velocity.y );

        /* if (inputControl.GetKeyDown(KeyCode.LeftShift)) {
            rb.velocity = new Vector2( 0.25, rb.velocity.y );
        } */
        
        int faceDir = (int)transform.localScale.x;
        // 判断当前角色向哪个方向移动
        if (inputDirection.x != 0) faceDir = inputDirection.x > 0 ? 1 : -1;
        
        // 控制人物翻转
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    /* private void OnTriggerStay2D(Collider2D other) 
    {
        Debug.Log(other.name);
    } */

    /// <summary>
    /// Jump 用于控制角色跳跃时逻辑
    /// </summary>
    private void Jump(InputAction.CallbackContext content) 
    {
        // 判断角色当前是否处于地面   
        if (physicsCheck.isGround) 
        {
            // 为 rb 刚体添加一个向上的力, 力度模式设置为瞬时力
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
            
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!physicsCheck.isGround) return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    #region UnityEevnt
    public void GetHurt(Transform attacker) 
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead() 
    {
        isDead = true;
        inputControl.Gameplayer.Disable();
    }
    #endregion

    public void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround ? normal : wall;
    }

}
