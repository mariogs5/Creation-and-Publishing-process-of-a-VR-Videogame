using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class StickyGrabInteractable : XRGrabInteractable
{
    public bool _isStuck = false;
    IXRSelectInteractor _stuckInteractor = null;

    public List<Vector3> positionList = new List<Vector3>();

    protected override void Awake()
    {
        base.Awake();
        selectMode = InteractableSelectMode.Single;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (!_isStuck)
        {
            _isStuck = true;
            _stuckInteractor = args.interactorObject;
        }
    }

    // Function called when the user release the grip button
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (_isStuck && args.interactorObject == _stuckInteractor)
        {
            interactionManager.SelectEnter(_stuckInteractor, this);
            return;
        }

        base.OnSelectExited(args);
    }

    // Use this function when you want to release the object
    public void ReleaseStickyGrab(int nextScene)
    {
        if (_isStuck && _stuckInteractor != null)
        {
            _isStuck = false;

            interactionManager.SelectExit(_stuckInteractor, this);

            _stuckInteractor = null;

            gameObject.transform.position = positionList[nextScene];
        }
    }
}