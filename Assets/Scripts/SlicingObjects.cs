using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using System.Threading;
using JetBrains.Annotations;
public class SlicingObjects : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    public Material crossSectionMaterial;
    public float cutForce = 2000;
    public float interval = 0.5f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = interval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

        if (hasHit && (timer <= 0))
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
            timer = interval;
        }
    }

    public void Slice(GameObject target)
    {

        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {

            crossSectionMaterial = target.GetComponent<Renderer>().material;
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
            SetupSlicedComponent(upperHull);

            crossSectionMaterial = target.GetComponent<Renderer>().material;
            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject)
    {
        slicedObject.layer = LayerMask.NameToLayer("Sliceable");
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
        PlayAudioOnTriggerEnter soundFX = slicedObject.AddComponent<PlayAudioOnTriggerEnter>();
        soundFX.soundName = "AxeChop1";
        soundFX.targetTag = "Axe";
        CapsuleCollider capsuleCollider = slicedObject.AddComponent<CapsuleCollider>();
        capsuleCollider.isTrigger = true;

    }
}
