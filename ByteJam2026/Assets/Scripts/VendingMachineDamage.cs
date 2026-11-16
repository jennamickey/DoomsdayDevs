using UnityEngine;

public class VendingMachineDamage : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Damage Multiplier";
    [SerializeField] private int price = 2000;

    public void Interact()
    {
        Debug.Log("Attempting to purchase " + machineName);
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}