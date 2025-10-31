using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;   

public class FireBullet : MonoBehaviour
{

    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 20;


    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(FireBull);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBull(ActivateEventArgs arg)
    {
        GameObject spawnedBulled = Instantiate(bullet);
        spawnedBulled.transform.position = spawnPoint.position;
        spawnedBulled.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedBulled, 5);
    }
}
