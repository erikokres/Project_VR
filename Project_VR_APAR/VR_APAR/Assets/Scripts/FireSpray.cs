using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireSpray : MonoBehaviour
{

    [SerializeField] private ParticleSystem testSprayFire = default;
    // Start is called before the first frame update
    void Start()
    {
        testSprayFire.Stop();
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(Fire);
        grabInteractable.deactivated.AddListener(noFire);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(ActivateEventArgs args)
    {
        testSprayFire.Play();
    }

    public void noFire(DeactivateEventArgs args)
    {
        testSprayFire.Stop();
    }
}
