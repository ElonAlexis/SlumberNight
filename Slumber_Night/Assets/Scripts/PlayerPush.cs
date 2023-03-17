using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    float distance = 9f; 
    float speed = 3f;
    Vector3 increasedY = new Vector3(0,2,0);

    [SerializeField] LayerMask layermask; 
    PlayerScript playerScript; 

    public GameObject playerPart; 
    public GameObject player; 

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
    }


    // Update is called once per frame
    void Update()
    {
        
       if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo,  distance, layermask ))
       {
            Debug.Log("HitSomething");
            Debug.DrawRay(transform.position + increasedY, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);

           if(playerScript.isInterracting && playerScript.isMovementPressed && hitInfo.collider != null)
            {
                 Debug.Log("Grabbing & Moving Object!");
                 // Stop Player rotation
                 playerScript.rotationFactorPerFrame = 0;
                 playerScript.playerSpeed = 3f;

                 // Play heavy pull animation 
                 
                 // Add force to object so it is pulled wa
                 float pullingSpeed = speed * Time.deltaTime;
                 hitInfo.collider.gameObject.transform.position = Vector3.MoveTowards(hitInfo.collider.gameObject.transform.position, playerPart.transform.position, pullingSpeed);

            }
            else{
                playerScript.rotationFactorPerFrame = 0.1f;
                 playerScript.playerSpeed = 7f;

            }
       }
       else
       {
            Debug.Log("HitNothing");          
            Debug.DrawRay(transform.position + increasedY, transform.TransformDirection(Vector3.forward) * hitInfo.distance, Color.red);
       }
        
    }

}
