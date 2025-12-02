using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image firePowerBar;
    [SerializeField] private Image extinguisherCapacityBar;
    [SerializeField] private FireController fireController;
    [SerializeField] private FinalScripApar extinguisherController;

    [SerializeField] private GameObject endCanvas;
    [SerializeField] private GameObject leftRay;
    [SerializeField] private GameObject rightRay;
    void Update()
    {
        SetFirePowerBar();
        SetExtinguisherCapacityBar();
        EndScene();
    }

    private void EndScene()
    {
        if (fireController.GetFirePower <= 0 || extinguisherController.GetCapacity <= 0)
        {
            endCanvas.SetActive(true);
            leftRay.SetActive(true);
            rightRay.SetActive(true);
        }
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
