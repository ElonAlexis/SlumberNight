using UnityEngine;
using System.Collections;


public class PickupTestScript : MonoBehaviour
{
    [SerializeField] private float maxPickupDistance = 5f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Camera attachedCamera;
    [SerializeField] private PlayerController playerController;


    private Transform cam;
    public bool isHoldingObject = false;
    private GameObject objectToPickup;
    private Rigidbody objectRigidbody;
    private Vector3 objectLocalPosition;
    private Quaternion objectLocalRotation;

    bool canceled; 

    private void Start()
    {
        cam = attachedCamera.transform;       
    }

    private void Update()
    {    
        if (playerController.isPushPressed)
        {
            if (!isHoldingObject)
            {
                Debug.Log("Something");
                // Try to pick up an object
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxPickupDistance, pickupLayer))
                {
                    objectToPickup = hit.collider.gameObject;
                    objectRigidbody = objectToPickup.GetComponent<Rigidbody>();
                    if (objectRigidbody != null)
                    {
                        // Disable physics and save object's local position and rotation
                        objectRigidbody.isKinematic = true;
                        objectLocalPosition.y = objectToPickup.transform.localPosition.y + 10;
                        objectLocalRotation = objectToPickup.transform.localRotation;
                        // Parent object to player
                        objectToPickup.transform.SetParent(transform);
                        isHoldingObject = true;
                        playerController.isPushPressed = false; 
                    }
                }
            }
            else
            {
                // Drop the object
                isHoldingObject = false;
                objectRigidbody.isKinematic = false;
                objectToPickup.transform.SetParent(null);
                objectToPickup = null;
                objectRigidbody = null;
                playerController.isPushPressed = false; 

            }
        }           
    
        
        // Look at the nearest object with the desired pickup layer
        if (isHoldingObject) return;
        var closestObject = FindClosestObject();
        if (closestObject != null)
        {
            cam.LookAt(closestObject.transform);
        }
    }

    private GameObject FindClosestObject()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Liftable");
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        // Loop through array to check show which items are being added to the array 
        // for (int i = 0; i < objects.Length; i++)
        // {
        //     Debug.Log("Element " + i + " is " + objects[i]);
        // }

        return closestObject;
    }



}














