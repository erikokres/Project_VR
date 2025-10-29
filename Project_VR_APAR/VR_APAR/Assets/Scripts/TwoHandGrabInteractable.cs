using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachinitialRotation;
    [System.Obsolete]
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEnter.AddListener(OnSecondHandGrab);
            item.onSelectExit.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Obsolete]
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            //Compute the rotation
            selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }
        base.ProcessInteractable(updatePhase);
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Enter");
        secondInteractor = interactor;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        Debug.Log("Second Hand Exit");
        secondInteractor = null;
    }

    [System.Obsolete]
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        Debug.Log("First Hand Grab");
        base.OnSelectEntering(interactor);
        attachinitialRotation = interactor.attachTransform.localRotation;
    }

    [System.Obsolete]
    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        Debug.Log("First Hand Release");
        base.OnSelectExiting(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachinitialRotation;
    }

    [System.Obsolete]
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
