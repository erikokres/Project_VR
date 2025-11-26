using System;
using System.Collections;
using UnityEngine;

public class ExtinguisherController : MonoBehaviour
{
    #region Fields

    [SerializeField] private Rigidbody bolt;
    
    [SerializeField] private Transform boltTarget;
    [SerializeField] private Transform nozzle;
    [SerializeField] private Transform nozzleTarget;
    [SerializeField] private Transform valve;
    [SerializeField] private Transform foamRayPoint;

    [SerializeField] private MouseTrigger boltTrigger;
    [SerializeField] private MouseTrigger nozzleTrigger;
    [SerializeField] private MouseTrigger valveTrigger;
    [SerializeField] private MouseTrigger nozzleDragTrigger;
    
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip valveSfx;
    
    [SerializeField] private float capacity = 10.0f;
    [SerializeField] private float boltAnimDuration = 0.1f;
    [SerializeField] private float nozzleAnimDuration = 0.1f;
    [SerializeField] private float valveAnimDuration = 0.1f;
    [SerializeField] private float openedValveAngle;
    [SerializeField] private float foamRayLength = 40;
    public float GetCapacity => capacity;
    public float MaxCapacity { get; private set; }
    private float _closedValveAngle;
    
    private FireZoneTrigger _currentFireZoneTrigger;
    private Coroutine _valveOpenRoutine;
    private Coroutine _changeCapacityRoutine;

    #endregion

    #region Private Methods

    private void Start()
    {
        MaxCapacity = capacity;
        _closedValveAngle = valve.localEulerAngles.z;
    }
    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(foamRayPoint.position, foamRayPoint.position + foamRayPoint.forward * foamRayLength, Color.red);
    }

    #endregion

    #region Public Methods

    public void EnableExtinguisher()
    {
        //valve open animation
        if(_valveOpenRoutine != null) StopCoroutine(_valveOpenRoutine);
        _valveOpenRoutine = StartCoroutine(OpenValve(openedValveAngle));
        
        if(capacity > 0)
        {
            particles.Play();
            audioSource.Play();
            
            //re-enable the capacity reduction Coroutine
            if (_changeCapacityRoutine != null) StopCoroutine(_changeCapacityRoutine);
            _changeCapacityRoutine = StartCoroutine(ChangeCapacity());

            //fire hit check
            Ray ray = new Ray(foamRayPoint.position, foamRayPoint.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, foamRayLength))
            {
                if (hitInfo.collider.CompareTag("FireZoneTrigger"))
                {
                    _currentFireZoneTrigger = hitInfo.collider.GetComponent<FireZoneTrigger>();
                    _currentFireZoneTrigger.BeginTakeDamage();
                }
            }
        }
    }

    public void DisableExtinguisher()
    {
        particles.Stop();
        audioSource.Stop();

        //disable the capacity reduction Coroutine
        if (_changeCapacityRoutine != null) StopCoroutine(_changeCapacityRoutine);
        
        //valve close animation
        if(_valveOpenRoutine != null) StopCoroutine(_valveOpenRoutine);
        _valveOpenRoutine = StartCoroutine(OpenValve(_closedValveAngle));

        //stop taking health in fire
        if (_currentFireZoneTrigger != null)
        {
            _currentFireZoneTrigger.EndTakeDamage();
            _currentFireZoneTrigger = null;
        }
    }
    
    public void UnlockBolt()
    {
        StartCoroutine(UnlockBoltRoutine());
    }
    
    public void UnlockNozzle()
    {
        StartCoroutine(UnlockNozzleRoutine());
    }

    #endregion

    #region Coroutines

    private IEnumerator OpenValve(float angle)
    {
        audioSource.PlayOneShot(valveSfx);
        //valve open/close animation
        while (Math.Abs(valve.localEulerAngles.z - angle) > .1f)
        {
            valve.rotation = Quaternion.Lerp(valve.rotation, Quaternion.Euler(0f, 0f, angle), valveAnimDuration);
            yield return null;
        }
    }

    private IEnumerator UnlockBoltRoutine()
    {
        //Bolt extraction animation
        while (Vector3.Distance(bolt.transform.position, boltTarget.position) >= 0.1f)
        {
            bolt.transform.position = Vector3.Lerp(bolt.transform.position, boltTarget.position, boltAnimDuration);
            yield return null;
        }
        //activate rigidbody
        bolt.isKinematic = false;
        //set next MouseTrigger
        boltTrigger.gameObject.SetActive(false);
        nozzleTrigger.gameObject.SetActive(true);
    }

    private IEnumerator UnlockNozzleRoutine()
    {
        //fire extinguisher aiming animation
        while (Vector3.Distance(nozzle.position, nozzleTarget.position) >= 0.1f)
        {
            nozzle.position = Vector3.Lerp(nozzle.position, nozzleTarget.position, nozzleAnimDuration);
            nozzle.rotation = Quaternion.Lerp(nozzle.rotation, nozzleTarget.rotation, nozzleAnimDuration * .75f);
            yield return null;
        }
        //set next MouseTriggers
        nozzleTrigger.gameObject.SetActive(false);
        valveTrigger.gameObject.SetActive(true);
        nozzleDragTrigger.gameObject.SetActive(true);
    }

    private IEnumerator ChangeCapacity()
    {
        //emptying the fire extinguisher
        while (capacity > 0)
        {
            capacity -= Time.deltaTime;
            capacity = Math.Clamp(capacity, 0, MaxCapacity);
            yield return null;
        }
        //locking the fire extinguisher when it is empty
        if (capacity <= 0)
        {
            particles.Stop();
            audioSource.Stop();
            if (_currentFireZoneTrigger != null)
            {
                _currentFireZoneTrigger.EndTakeDamage();
                _currentFireZoneTrigger = null;
            }
        }
    }

    #endregion
}
