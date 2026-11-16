using UnityEngine;

public class WorldWeaponPickup : MonoBehaviour, IInteractable
{
    [Header("Weapon Purchase Settings")]
    [SerializeField] private string weaponName = "SCAR";
    [SerializeField] private int price = 1500;
    [SerializeField] private GameObject equippableGunPrefab;

    [Header("Optional Visual Spin")]
    [SerializeField] private bool rotateDisplay = true;
    [SerializeField] private float rotateSpeed = 45f;

    void Update()
    {
        if (rotateDisplay)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
        }
    }

    public void Interact()
    {
        if (GameManager.Instance == null || equippableGunPrefab == null) return;

        if (GameManager.Instance.score >= price)
        {
            GameManager.Instance.AddPoints(-price);

            WeaponManager weaponManager = FindFirstObjectByType<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.BuyOrGiveWeapon(equippableGunPrefab);
            }

            Debug.Log("Purchased " + weaponName + "!");
        }
        else
        {
            Debug.Log("Not enough points for " + weaponName + "!");
        }
    }

    public string GetPrompt()
    {
        return "Press E to Buy " + weaponName + " - " + price + " Points";
    }
}