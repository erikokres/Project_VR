using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image firePowerBar;
    [SerializeField] private Image extinguisherCapacityBar;
    [SerializeField] private FireController fireController;
    [SerializeField] private ExtinguisherController extinguisherController;
    void Update()
    {
        SetFirePowerBar();
        SetExtinguisherCapacityBar();
    }

    private void SetFirePowerBar()
    {
        firePowerBar.fillAmount = fireController.GetFirePower / fireController.MaxFirePower;
    }
    
    private void SetExtinguisherCapacityBar()
    {
        extinguisherCapacityBar.fillAmount = extinguisherController.GetCapacity / extinguisherController.MaxCapacity;
    }
}
