using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class VRAxeSlicing : MonoBehaviour
{
    public Material crossSectionMaterial; 
    public float sliceThreshold = 0.5f; // Minimum swing velocity
    public LayerMask sliceableLayer;
    private Vector3 lastPosition;
    private Vector3 velocity;

    void Start()
    {
        lastPosition = transform.position;
    }

    
    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        Debug.Log($"Axe position: {transform.position}");
        Debug.Log($"Axe velocity magnitude: {velocity.magnitude}");
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Detected collision with {other.gameObject.name}");
        Debug.Log($"Tree Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        Debug.Log($"Axe Velocity: {velocity.magnitude}");        
        
        if (other.gameObject.layer == LayerMask.NameToLayer("SliceableLayer"))
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * 2, Color.red, 2.0f);

            if (velocity.magnitude >= sliceThreshold)
            {
                Debug.Log($"Slicing {other.gameObject.name}");
                Slice(other.gameObject);
            }
            else
            {
                Debug.Log("Swing too slow to slice!");
            }
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

        Vector3 planePoint = transform.position; // Axe position
        Vector3 planeNormal = transform.forward; // Axe forward direction

        Debug.Log($"Slicing plane origin: {planePoint}, normal: {planeNormal}");

        SlicedHull hull = target.Slice(planePoint, planeNormal);

        if (hull != null)
        {
            Debug.Log("Slice successful!");
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

            SetupSlicedComponent(upperHull);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
        else
        {
            Debug.LogError("Slice failed! Check the mesh or slicing plane.");
        }
    }


    private void SetupSlicedComponent(GameObject slicedObject)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true; 
    }
}




/*using System.Collections;
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
*/