using System.Collections.Generic;
using UnityEngine;

public class ObstacleFader : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform target; // Drag your player or turret here
    [SerializeField] private LayerMask obstacleLayer; // Set this to your 'Wall' or 'Environment' layer

    [Header("Shader Properties")]
    [SerializeField] private string ditherPropertyName = "_DitherAmount"; // The Reference name in your Shader Graph
    [SerializeField] private float ditherOnValue = 1f;
    [SerializeField] private float ditherOffValue = 0f;

    private List<Renderer> currentlyHidden = new List<Renderer>();
    private List<Renderer> previouslyHidden = new List<Renderer>();

    private void Update()
    {
        if (target == null) return;

        // Cast FROM the target TO the camera to ensure we hit objects the camera is inside of
        Vector3 direction = transform.position - target.position;
        float distance = direction.magnitude;

        // Raycast to find all obstacles between player and camera
        RaycastHit[] hits = Physics.RaycastAll(target.position, direction.normalized, distance, obstacleLayer);

        previouslyHidden.Clear();
        previouslyHidden.AddRange(currentlyHidden);
        currentlyHidden.Clear();

        // Process current hits
        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                currentlyHidden.Add(hitRenderer);
                previouslyHidden.Remove(hitRenderer);

                // Apply dither effect
                hitRenderer.material.SetFloat(ditherPropertyName, ditherOnValue);
            }
        }

        // Restore objects that are no longer blocking the camera
        foreach (Renderer renderer in previouslyHidden)
        {
            if (renderer != null)
            {
                // Remove dither effect
                renderer.material.SetFloat(ditherPropertyName, ditherOffValue);
            }
        }
    }
}