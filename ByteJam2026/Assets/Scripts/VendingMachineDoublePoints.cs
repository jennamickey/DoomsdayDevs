using UnityEngine;

public class VendingMachineDoublePoints : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Double Points";
    [SerializeField] private int price = 750;

    public void Interact()
    {
        Debug.Log("Attempting to purchase " + machineName);
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}