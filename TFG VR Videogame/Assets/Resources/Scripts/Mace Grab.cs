using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceGrab : MonoBehaviour
{
    [SerializeField] private GameObject mace;
    [SerializeField] private bool isRightHand;
    private bool isTracking;

    [SerializeField] private GameObject rightAttachPoint;
    [SerializeField] private GameObject leftAttachPoint;

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
    }

    private void TrackRightHand()
    {
        mace.transform.position = rightAttachPoint.transform.position;
        mace.transform.rotation = rightAttachPoint.transform.rotation;
    }
    private void TrackLeftHand()
    {
        mace.transform.position = leftAttachPoint.transform.position;
        mace.transform.rotation = leftAttachPoint.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Handle")
        {
            isTracking = true;
        }
    }
}
