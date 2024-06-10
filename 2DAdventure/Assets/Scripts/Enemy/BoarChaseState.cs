using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // Debug.Log("BoarChaseState.OnEnter");
        currentEmeny = enemy;
        currentEmeny.currentSpeed = currentEmeny.chaseSpeed;
        currentEmeny.anima.SetBool("run", true);
    }

    public override void LogicUpdate()
    {
        if (currentEmeny.lostTimeCounter <= 0)
        {
            currentEmeny.SwitchStatus(NPCStatus.Patrol);
        }
        
        if (!currentEmeny.physicsCheck.isGround  
            || (currentEmeny.physicsCheck.touchLeftWall && currentEmeny.faceDir.x < 0)
            || (currentEmeny.physicsCheck.touchRightWall && currentEmeny.faceDir.x > 0))
        {
            currentEmeny.transform.localScale = new Vector3(currentEmeny.faceDir.x, 1, 1);
        }
    }

    public override void PhysicsUpdate()
    {
        // throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        currentEmeny.lostTimeCounter = currentEmeny.losTime;
        currentEmeny.anima.SetBool("run", false);
    }
}
