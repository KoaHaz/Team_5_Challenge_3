using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
public class SlicingObjects : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform  endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public Material crossSectionMaterial; 
    public float cutForce = 2000;

    // Start is called before the first frame update
    void Start()
    {
        if (velocityEstimator == null)
        {
            velocityEstimator = GetComponent<VelocityEstimator>();
            if (velocityEstimator == null)
            {
                Debug.LogError("VelocityEstimator is not assigned or missing!");
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

        Debug.Log(hasHit ? $"Hit {hit.transform.name}" : "No object hit.");

        if(hasHit)
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }


    public void Slice(GameObject target)
    {
        Debug.Log($"Attempting to slice: {target.name}");

        MeshFilter meshFilter = target.GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError($"{target.name} is missing a MeshFilter or valid mesh!");
            return;
        }
        Debug.Log($"{target.name} has a valid mesh with {meshFilter.mesh.vertexCount} vertices.");

       // Vector3 velocity = velocityEstimator != null ? velocityEstimator.GetVelocityEstimate() : Vector3.zero;
       // Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity).normalized;

        // Simplify slicing plane
        Vector3 planeNormal = Vector3.up;
        Debug.Log($"Calculated Plane Normal: {planeNormal}");

        Debug.Log("Calculating slicing plane...");
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            Debug.Log("Slice successful!");
            Debug.Log($"Slice successful! Upper hull vertices: {hull.upperHull.vertexCount}, Lower hull vertices: {hull.lowerHull.vertexCount}");
            
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
        GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

        if (upperHull != null && lowerHull != null)
        {
            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);
            Destroy(target);
        }
        else
        {
            Debug.LogError("Failed to create sliced hulls!");
        }
        }
        else
        {
            Debug.LogError("Slice failed! Check the target's mesh or slicing plane.");
        }
    }


    public void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true; 
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}
