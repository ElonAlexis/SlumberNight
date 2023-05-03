using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Material glowMaterial;

    private PickupTestScript pickupTestScript; 
    public float glowIntensity = 15f;
    public bool G;

    private void Start()
    {
        pickupTestScript = FindObjectOfType<PickupTestScript>();
        if (glowMaterial == null)
        {
            // Get the material from the renderer
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                glowMaterial = renderer.material;
            }
        }
    }

    void Update()
    {        
            if(pickupTestScript.isHoldingObject)
            {
                glowIntensity = 15;
            }
            else
            {
                glowIntensity = 1;
            }
            glowMaterial.SetFloat("_GlowIntensity", glowIntensity);
    }
    
}
