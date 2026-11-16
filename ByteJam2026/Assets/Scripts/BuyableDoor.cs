
using UnityEngine;

public class BuyableDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private string doorName = "Open Door";
    [SerializeField] private int price = 1500;

    [Header("Door Objects")]
    [SerializeField] private GameObject doorObject;

    private bool isPurchased = false;

    public void Interact()
    {
        if (isPurchased) return;

        if (GameManager.Instance == null) return;

        if (GameManager.Instance.SpendPoints(price))
        {
            isPurchased = true;

            if (doorObject != null)
            {
                doorObject.SetActive(false);
            }

            Debug.Log(doorName + " purchased!");
        }
    }

    public string GetPrompt()
    {
        if (isPurchased)
        {
            return "";
        }

        return "Press E to " + doorName + " - " + price + " Points";
    }
}
