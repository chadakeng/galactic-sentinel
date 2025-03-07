using UnityEngine;

public class CrosshairRaycast : MonoBehaviour
{
    public Camera playerCamera;
    public float rayDistance = 100f;
    public LayerMask interactableLayers;

    private Outline lastOutlinedObject;

    private void Start()
    {
        // Find all objects with Outline and disable them at the start
        Outline[] allOutlines = FindObjectsOfType<Outline>();
        foreach (Outline outline in allOutlines)
        {
            outline.enabled = false; // Ensure all objects start without highlight
        }
    }

    private void Update()
    {
        DetectObjectInCrosshair();
    }

    void DetectObjectInCrosshair()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayers))
        {
            if (hit.distance < 1.5f) 
            {
                RemoveOutline();
                return;
            }

            Outline outline = hit.collider.GetComponent<Outline>();

            if (outline != null && outline != lastOutlinedObject)
            {
                RemoveOutline(); // Remove previous outline
                outline.enabled = true;
                lastOutlinedObject = outline;
            }
        }
        else
        {
            RemoveOutline();
        }
    }

    void RemoveOutline()
    {
        if (lastOutlinedObject != null)
        {
            lastOutlinedObject.enabled = false;
            lastOutlinedObject = null;
        }
    }
}