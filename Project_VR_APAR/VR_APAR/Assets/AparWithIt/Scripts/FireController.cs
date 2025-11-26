using System;
using UnityEngine;

public class FireController : MonoBehaviour
{
    #region Fields

    [SerializeField] private ParticleSystem flamesParticles;
    [SerializeField] private Light fireLight;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private float firePower = 6.0f;
    [SerializeField] private float recoveryMultiplier = 0.5f;
    [SerializeField] private float minEmission = 10.0f;
    [SerializeField] private float maxEmission = 50.0f;
    [SerializeField] private float minFlameScale = 1.5f;
    [SerializeField] private float maxFlameScale = 4.0f;
    public float GetFirePower => firePower;
    public float MaxFirePower { get; private set; }

    private float _maxVolume;
    private float _maxLightIntensity;
    private float _damageMultiplier;
    private bool _extinguishedFire;
    private bool _getDamage;

    #endregion

    #region Private Methods

    private void Start()
    {
        MaxFirePower = firePower;
        _maxVolume = audioSource.volume;
        _maxLightIntensity = fireLight.intensity;
    }

    private void Update()
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        if(_extinguishedFire) return;
        
        //Calculate firePower
        firePower += (_getDamage ? -_damageMultiplier : recoveryMultiplier) * Time.deltaTime;
        firePower = Math.Clamp(firePower, 0, MaxFirePower);

        #region SetFireParameters

        //EmissionRate
        float emissionRate = CalculateRelativeValue(firePower, MaxFirePower, minEmission, maxEmission);
        ParticleSystem.EmissionModule emission = flamesParticles.emission;
        emission.rateOverTime = emissionRate;
        
        //FlameScale
        float flameScale = CalculateRelativeValue(firePower, MaxFirePower, minFlameScale, maxFlameScale);
        flamesParticles.transform.localScale = new Vector3(flameScale, flameScale, flameScale);

        //Light
        fireLight.intensity = CalculateRelativeValue(firePower, MaxFirePower, 0, _maxLightIntensity);
        
        //volume
        audioSource.volume = CalculateRelativeValue(firePower, MaxFirePower, 0, _maxVolume);

        #endregion
        
        //extinguishedFire
        if (firePower <= 0)
        {
            _extinguishedFire = true;
            flamesParticles.Stop();
            audioSource.Stop();
        }
    }

    private float CalculateRelativeValue(float x, float maxX, float minClamp, float maxClamp)
    {
        return minClamp + (x / maxX) * (maxClamp - minClamp);
    }

    #endregion

    #region Public Methods

    public void SetDamage(bool getDamage, float multiplier)
    {
        _damageMultiplier = multiplier;
        _getDamage = getDamage;
    }

    #endregion
}