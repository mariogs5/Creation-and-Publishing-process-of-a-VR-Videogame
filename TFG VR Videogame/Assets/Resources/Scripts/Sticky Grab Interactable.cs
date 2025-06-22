using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
public class StickyGrabInteractable : XRGrabInteractable
{
    bool _isStuck = false;
    IXRSelectInteractor _stuckInteractor = null;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (!_isStuck)
        {
            _isStuck = true;
            _stuckInteractor = args.interactorObject;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        if (_isStuck && args.interactorObject == _stuckInteractor)
        {
            return;
        }

        base.OnSelectExited(args);
    }

    public void ReleaseStickyGrab()
    {
        if (_isStuck && _stuckInteractor != null)
        {
            _isStuck = false;

            interactionManager.SelectExit(_stuckInteractor, this);

            _stuckInteractor = null;
        }
    }
}