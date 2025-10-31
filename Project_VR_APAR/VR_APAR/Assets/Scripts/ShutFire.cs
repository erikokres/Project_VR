using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ShutFire : MonoBehaviour
{
    [SerializeField] VisualEffect m_VFX;
    [SerializeField] private float fireRate;

    [SerializeField] private GameObject FireEffect;
    Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0.1f);

    private static readonly int k_FireRateProperties = Shader.PropertyToID("FlameRate");

    // Start is called before the first frame update
    private void Awake()
    {
        if(m_VFX == null)
        {
            m_VFX = GetComponent<VisualEffect>();
            m_VFX.SetFloat(k_FireRateProperties, fireRate);
        }
        FireEffect.transform.localScale = new Vector3(1, 1, 1);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spray"))
        {
            FireEffect.transform.localScale -= scaleChange;
            Debug.Log("SPRAY");
        }
    }
}
