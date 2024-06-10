using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEmeny = enemy;
        
    }

    public override void LogicUpdate()
    {
        // TODO: 发现 player 就切换到 chase 状态
        
        if (!currentEmeny.physicsCheck.isGround  
            || (currentEmeny.physicsCheck.touchLeftWall && currentEmeny.faceDir.x < 0)
            || (currentEmeny.physicsCheck.touchRightWall && currentEmeny.faceDir.x > 0))
        {
            // transform.localScale = new Vector3(faceDir.x, 1, 1);
            currentEmeny.wait = true;
            currentEmeny.anima.SetBool("walk", false);
        }
        else
        {
            currentEmeny.anima.SetBool("walk", true);
        }
    }

    public override void PhysicsUpdate()
    {
        // throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        currentEmeny.anima.SetBool("walk", false);
    }
}
