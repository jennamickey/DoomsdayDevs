using UnityEngine;

public class VendingMachineDamage : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Damage Multiplier";
    [SerializeField] private int price = 2000;

    public void Interact()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.IsDoubleDamageActive())
        {
            Debug.Log("Double Damage Already Active!");
            return;
        }

        if (GameManager.Instance.SpendPoints(price))
        {
            GameManager.Instance.ActivateDoubleDamage();

            Debug.Log("Purchased " + machineName);
        }
    }
    
    public string GetPrompt()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsDoubleDamageActive())
        {
            return "Double Damage Active";
        }

        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}