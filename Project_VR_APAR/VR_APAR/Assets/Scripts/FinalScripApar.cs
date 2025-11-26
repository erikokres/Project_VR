using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FinalScripApar : XRGrabInteractable
{

    #region Fields

    [SerializeField] private Transform foamRayPoint;

    [SerializeField] private ParticleSystem particles;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip valveSfx;

    [SerializeField] private float capacity = 10.0f;
    [SerializeField] private float valveAnimDuration = 0.1f;
    [SerializeField] private float openedValveAngle;
    [SerializeField] private float foamRayLenght = 40;

    public float GetCapacity => capacity;
    public float MaxCapacity { get; private set; }
    private float _closedValveAngle;

    private FireZoneTrigger _curentFireZoneTrigger;
    private Coroutine _changeCapacityRoutine;

    [System.Obsolete]
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachinitialRotation;



    #endregion

    #region Private Methods
    // Start is called before the first frame update
    void Start()
    {
        MaxCapacity = capacity;
        foreach(var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(foamRayPoint.position, foamRayPoint.position + foamRayPoint.forward * foamRayLenght, Color.red);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Public Methods

    public void EnableExtinguisher()
    {
        if(capacity > 0)
        {
            particles.Play();
            audioSource.Play();

            //re-enable the capacity reduction Coroutine
            if (_changeCapacityRoutine != null) StopCoroutine(_changeCapacityRoutine);
            _changeCapacityRoutine = StartCoroutine(ChangeCapacity());

            //Fire hit check
            Ray ray = new Ray(foamRayPoint.position, foamRayPoint.forward);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, foamRayLenght))
            {
                if (hitInfo.collider.CompareTag("FireZoneTrigger"))
                {
                    _curentFireZoneTrigger = hitInfo.collider.GetComponent<FireZoneTrigger>();
                    _curentFireZoneTrigger.BeginTakeDamage();
                }
            }


        }
    }

    public void DisableExtinguisher()
    {
        particles.Stop();
        audioSource.Stop();

        //disable the capacity reduction cooroutine
        if (_changeCapacityRoutine != null) StopCoroutine(_changeCapacityRoutine);

        //stop taking haealth in fire
        if(_curentFireZoneTrigger != null)
        {
            _curentFireZoneTrigger.EndTakeDamage();
            _curentFireZoneTrigger = null;
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {
            //compute the rotation
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
        return base.IsSelectableBy(interactor);
    }

    #endregion

    #region Coroutines

    private IEnumerator ChangeCapacity()
    {
        //emptying the fire extinguisher
        while(capacity > 0)
        {
            capacity -= Time.deltaTime;
            capacity = Mathf.Clamp(capacity, 0, MaxCapacity);
            yield return null;
        }

        //locking the fire extinguisher when it is empty
        if(capacity <= 0)
        {
            particles.Stop();
            audioSource.Stop();
            if(_curentFireZoneTrigger != null)
            {
                _curentFireZoneTrigger.EndTakeDamage();
                _curentFireZoneTrigger = null;
            }
        }
    }

    #endregion
}

