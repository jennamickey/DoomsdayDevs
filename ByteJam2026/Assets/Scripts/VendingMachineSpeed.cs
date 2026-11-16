using UnityEngine;

public class VendingMachineSpeed : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Speed Up";
    [SerializeField] private int price = 2500;

    public void Interact()
    {
        Debug.Log("Attempting to purchase " + machineName);
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}