using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public int damage = 25;
    public int maxAmmo = 30;
    public int reserveAmmo = 120;
    public float reloadTime = 1.5f;
    public bool isAutomatic = false;
    public float fireRate = 0.15f;

    [Header("UI Reference")]
    public TMP_Text ammoText;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;
    private Camera fpsCam;

    void Awake()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        UpdateAmmoUI();
    }

    void Start()
    {
        fpsCam = Camera.main;
        if (fpsCam == null)
        {
            fpsCam = FindObjectOfType<Camera>();
        }
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Mouse.current == null || Keyboard.current == null) return;

        if (isReloading) return;

        if (Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame && reserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }
            return;
        }

        bool wantsToShoot = isAutomatic
            ? Mouse.current.leftButton.isPressed
            : Mouse.current.leftButton.wasPressedThisFrame;

        if (wantsToShoot && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            ZombieHealth zombie = hit.collider.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        if (ammoText != null)
        {
            ammoText.text = "Reloading...";
        }

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + reserveAmmo;
        }
    }
}