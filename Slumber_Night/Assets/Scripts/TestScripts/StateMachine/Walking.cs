using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : BaseState
{
     public override void EnterState()
    {
        // Code to execute when entering the Walking state
        //Debug.Log("Entering Walking state");
        playerController = GameObject.FindObjectOfType<PlayerController>();
        //playerController.anim.SetBool("isWalking", true); 
        playerController.anim.Play("CrouchWalk");
    }
    void MovePlayer()
    {
                  
        playerController.currentMovement.x =  playerController.currentMovementInput.x;
        playerController.currentMovement.z =  playerController.currentMovementInput.y;

        playerController.currentRunMovement.x = playerController.currentMovementInput.x * 10;
        playerController.currentRunMovement.z = playerController.currentMovementInput.y * 10;

        playerController.characterController.Move(playerController.currentMovement * Time.deltaTime * playerController.runMultiplier);

        
    }
    void HandleRotation()
    {
        Vector3 positionToLookAt;        
        positionToLookAt.x = playerController.currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = playerController.currentMovement.z;

        Quaternion currentRotation = playerController.transform.rotation;

        if(playerController.isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            playerController.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, playerController.rotationFactorPerFrame );
        }
    }



    public override void UpdateState()
    {
        playerController.runMultiplier = 10; 

        // Code to execute while in the Walking state
      //  Debug.Log("Walking state");
        MovePlayer();
        HandleRotation();
    }
     public override void ExitState()
    {
        // Code to execute when exiting the Walking state
       // Debug.Log("Exiting Walking state");
    }
    
    
}
