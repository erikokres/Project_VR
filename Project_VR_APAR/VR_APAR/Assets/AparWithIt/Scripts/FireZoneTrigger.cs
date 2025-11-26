using UnityEngine;

public class FireZoneTrigger: MonoBehaviour
{
    [SerializeField] private FireController fireController;
    [SerializeField] private float damageMultiplier = 1.0f;

    public void BeginTakeDamage()
    {
        fireController.SetDamage(true, damageMultiplier);
    }
    public void EndTakeDamage()
    {
        fireController.SetDamage(false, damageMultiplier);
    }
}