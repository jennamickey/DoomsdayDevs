using UnityEngine;

public class VendingMachineDoublePoints : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Double Points";
    [SerializeField] private int price = 750;

    public void Interact()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.IsDoublePointsActive())
        {
            Debug.Log("Double Points Already Active!");
            return;
        }

        if (GameManager.Instance.SpendPoints(price))
        {
            GameManager.Instance.ActivateDoublePoints();

            Debug.Log("Purchased " + machineName);
        }
    }

    public string GetPrompt()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsDoublePointsActive())
        {
            return "Double Points Active";
        }

        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}