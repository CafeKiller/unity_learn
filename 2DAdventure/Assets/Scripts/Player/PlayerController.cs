using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // PlayerInputControl 是 unity 提供的全新的输入系统
    public PlayerInputControl inputControl;

    // inputDirection 用于保存输入时产生的变量值
    public Vector2 inputDirection;

    // 生命周期函数，在 Start() 之前调用, 类似于初始化构建
    // 如果游戏对象在启动期间处于非活动状态，则在激活之后才会调用 Awake。
    private void Awake() 
    {
        // 创建 inputSystem
        inputControl = new PlayerInputControl();
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
    }

}
