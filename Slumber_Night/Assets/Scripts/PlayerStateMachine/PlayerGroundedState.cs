using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Debug.Log("Hello From Grounded State");

    }

    public override void Updatestate() 
    {
        CheckSwitchState();
    }

    public override void ExitState() 
    {

    }

    public override void InitializeSubstate() 
    { 

    }

    public override void CheckSwitchState() 
    {

    }
}
