using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRunning : BaseState
{
     public override void EnterState()
    {
        // Code to execute when entering the SlowRunning state        
        playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.anim.Play("TiredRun");
    }
     public override void UpdateState()
        {
            // Code to execute while in the SlowRunning state
        //    Debug.Log("SlowRunning state");
             playerController.runMultiplier = 5; 

            
            playerController.currentMovement.x =  playerController.currentMovementInput.x;
            playerController.currentMovement.z =  playerController.currentMovementInput.y;

            playerController.currentRunMovement.x = playerController.currentMovementInput.x * 10;
            playerController.currentRunMovement.z = playerController.currentMovementInput.y * 10;
            playerController.characterController.Move(playerController.currentMovement * Time.deltaTime * playerController.runMultiplier);
            
            HandleRotation();

        }

        public override void ExitState()
        {
            // Code to execute when exiting the SlowRunning state
           //  Debug.Log("Exiting SlowRunning state");
           

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


}
