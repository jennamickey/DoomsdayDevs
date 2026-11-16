using UnityEngine;

public class VendingMachineSpeed : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Speed Up";
    [SerializeField] private int price = 2500;
    [SerializeField] private PlayerMovement playerMovement;

    public void Interact()
    {
        if (playerMovement.IsSpeedBoostActive())
        {
            Debug.Log("Speed Boost Already Active!");
            return;
        }

        playerMovement.ActivateSpeedBoost();

        Debug.Log("Purchased " + machineName);
    }

    public string GetPrompt()
    {
        if (playerMovement.IsSpeedBoostActive())
        {
            return "Speed Boost Active";
        }

        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}