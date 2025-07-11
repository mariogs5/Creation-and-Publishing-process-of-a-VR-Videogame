using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MaceGrabInteractable : MonoBehaviour
{
    [Header("Reference to the Mace:")]
    [SerializeField] private GameObject mace;

    [Header("Reference to the other hand script:")]
    [SerializeField] private MaceGrabInteractable otherHand;

    [Header("Select the hand where the script is:")]
    [SerializeField] private bool isRightHand;

    [Header("Transform that the Mace will track:")]
    [SerializeField] private Transform rightAttachPoint;
    [SerializeField] private Transform leftAttachPoint;

    [HideInInspector]
    public bool isTracking = false; // Boolean to check if the Mace is tracking this hand

    // Reference to the XRInteractor script to disable it if the Mace is tracking
    private XRDirectInteractor handInteractor;

    private void Awake()
    {
        isTracking = false;
        handInteractor = GetComponent<XRDirectInteractor>();
    }

    private void Update()
    {
        if (isTracking)
        {
            if (isRightHand)
            {
                TrackRightHand();
            }
            else
            {
                TrackLeftHand();
            }
        }
    }

    public void StopTracking()
    {
        isTracking = false;
        handInteractor.enabled = true;
    }

    private void TrackRightHand()
    {
        mace.transform.position = rightAttachPoint.position;
        mace.transform.rotation = rightAttachPoint.rotation;
    }
    private void TrackLeftHand()
    {
        mace.transform.position = leftAttachPoint.position;
        mace.transform.rotation = leftAttachPoint.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Handle" && !otherHand.isTracking)
        {
            isTracking = true;
            handInteractor.enabled = false;
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Handle" && !otherHand.isTracking)
    //    {
    //        isTracking = true;
    //        handInteractor.enabled = false;
    //    }
    //}
}
