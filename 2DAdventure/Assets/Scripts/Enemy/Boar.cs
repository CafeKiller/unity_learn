using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Enemy
{
    /*protected override void Move()
    {
        base.Move();
        anima.SetBool("walk", true);
    }*/

    protected override void Awake()
    {
        base.Awake();
        patrolState = new BoarPatrolState();
        chaseState = new BoarChaseState();
    }
}
