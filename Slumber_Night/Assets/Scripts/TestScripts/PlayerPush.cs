using UnityEngine;

public class PlayerPush : MonoBehaviour
{

    public float pushPower = 10f; // the force applied to the object when pushed

    private CharacterController characterController; // the character controller component
    private PlayerController playerController; // the player controller script

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // check if the object is a PushPullObject
        if (!hit.gameObject.CompareTag("PushPullObject"))
        {
            return;
        }

        Rigidbody boxRigidbody = hit.gameObject.GetComponent<Rigidbody>();
        if (boxRigidbody == null)
        {
            return;
        }

        // calculate the direction to push or pull the object
        Vector3 pushDirection = new Vector3(playerController.currentMovementInput.x, 0, playerController.currentMovementInput.y).normalized;

      
        // apply the force to the object based on whether the player is pushing or pulling
        if (playerController.isPushPressed)
        {
            // apply push force
            boxRigidbody.AddForce(pushDirection * pushPower, ForceMode.Impulse);
        }
    }

}










