using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{    

    public override void EnterState()
    {
        // Code to execute when entering the Idle state
       // Debug.Log("Entering Idle state");    
        //Play Animation 
        playerController = GameObject.FindObjectOfType<PlayerController>();
        //playerController.anim.SetBool("isIdle", true); 
        playerController.anim.Play("ScaredIdle");

        
    }

    public override void UpdateState()
    {
        // Code to execute while in the Idle state
      //  Debug.Log("Idle state");
    }

    public override void ExitState()
    {
        // Code to execute when exiting the Idle state
     //   Debug.Log("Exiting Idle state");
    }
}
