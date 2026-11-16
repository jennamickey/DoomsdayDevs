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
    public int maxReserveAmmo = 120;
    public float reloadTime = 1.5f;
    public bool isAutomatic = false;
    public float fireRate = 0.15f;

    [Header("UI Reference")]
    public TMP_Text ammoText;

    [Header("Aiming")]
    [SerializeField] private LayerMask hitLayers = Physics.DefaultRaycastLayers;
    
    public Camera fpsCam;
    private int currentAmmo;
    private float nextFireTime = 0f;
    private bool isReloading = false;

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
        if (fpsCam == null) fpsCam = GetComponentInParent<Camera>();
        if (fpsCam == null) fpsCam = Camera.main;
        if (fpsCam == null)
        {
            fpsCam = FindObjectOfType<Camera>();
        }

        UpdateAmmoUI();
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (isReloading) return;

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame &&
            currentAmmo < maxAmmo &&
            reserveAmmo > 0)
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
        if (fpsCam == null)
        {
            Debug.LogError("Assign the player's camera to the gun's Aiming reference.", this);
            return;
        }

        currentAmmo--;
        UpdateAmmoUI();

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            Mathf.Infinity,
            hitLayers,
            QueryTriggerInteraction.Collide
        );

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Transform playerRoot = fpsCam.transform.root;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.transform.IsChildOf(playerRoot)) continue;

            ZombieHealth zombie = hit.collider.GetComponentInParent<ZombieHealth>();

            if (hit.collider.isTrigger && zombie == null) continue;

            if (zombie != null)
            {
                int damageAmount = damage;

                if (GameManager.Instance != null &&
                    GameManager.Instance.IsDoubleDamageActive())
                {
                    damageAmount *= 2;
                }

                zombie.TakeDamage(damageAmount);
                Debug.Log("Successfully damaged the zombie!");
            }

            break;
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

    public void RefillAmmo()
    {
        currentAmmo = maxAmmo;
        reserveAmmo = maxReserveAmmo;

        UpdateAmmoUI();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        isReloading = false;
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + reserveAmmo;
        }
    }
}
