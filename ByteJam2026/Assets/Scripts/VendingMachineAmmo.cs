using UnityEngine;

public class VendingMachineAmmo : MonoBehaviour, IInteractable
{
    [Header("Vending Machine Settings")]
    [SerializeField] private string machineName = "Max Ammo";
    [SerializeField] private int price = 750;

    public void Interact()
    {
        Debug.Log("Attempting to purchase " + machineName);

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        if (GameManager.Instance.SpendPoints(price))
        {
            GunController[] guns = FindObjectsOfType<GunController>();

            foreach (GunController gun in guns)
            {
                gun.RefillAmmo();
            }

            Debug.Log("Max Ammo Purchased!");
        }
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + machineName + " - " + price + " Points";
    }
}