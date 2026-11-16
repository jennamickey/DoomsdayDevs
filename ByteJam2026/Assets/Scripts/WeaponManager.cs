using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [Header("Starting Loadout")]
    public GameObject startingPistolPrefab;

    [Header("Shared UI & Camera")]
    public TMP_Text ammoText;
    public Camera fpsCam;

    private GameObject[] weaponSlots = new GameObject[2];
    private int currentSlot = 0;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (startingPistolPrefab != null)
        {
            BuyOrGiveWeapon(startingPistolPrefab);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            int nextSlot = (currentSlot == 0) ? 1 : 0;
            if (weaponSlots[nextSlot] != null)
            {
                EquipSlot(nextSlot);
            }
        }
        else if (Keyboard.current.digit1Key.wasPressedThisFrame && weaponSlots[0] != null)
        {
            EquipSlot(0);
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame && weaponSlots[1] != null)
        {
            EquipSlot(1);
        }
    }

    public void EquipSlot(int slotIndex)
    {
        currentSlot = slotIndex;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
            {
                weaponSlots[i].SetActive(i == currentSlot);
            }
        }
    }

    public void BuyOrGiveWeapon(GameObject weaponPrefab)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null && weaponSlots[i].name.StartsWith(weaponPrefab.name))
            {
                GunController existingGun = weaponSlots[i].GetComponent<GunController>();
                if (existingGun != null)
                {
                    existingGun.RefillAmmo();
                }
                EquipSlot(i);
                return;
            }
        }

        GameObject spawnedGun = Instantiate(weaponPrefab, transform);
        spawnedGun.transform.localPosition = Vector3.zero;
        spawnedGun.transform.localRotation = Quaternion.identity;

        GunController gunController = spawnedGun.GetComponent<GunController>();
        if (gunController != null)
        {
            gunController.ammoText = ammoText;
            gunController.fpsCam = fpsCam;
            gunController.RefillAmmo();
        }

        if (weaponSlots[0] == null)
        {
            weaponSlots[0] = spawnedGun;
            EquipSlot(0);
            return;
        }

        if (weaponSlots[1] == null)
        {
            weaponSlots[1] = spawnedGun;
            EquipSlot(1);
            return;
        }

        Destroy(weaponSlots[currentSlot]);
        weaponSlots[currentSlot] = spawnedGun;
        EquipSlot(currentSlot);
    }
}