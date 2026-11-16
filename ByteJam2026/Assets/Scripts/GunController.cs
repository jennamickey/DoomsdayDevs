using UnityEngine;
using UnityEngine.InputSystem;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public int damage = 25;
    public int maxAmmo = 30;
    public bool isAutomatic = false;
    public float fireRate = 0.15f;

    private int currentAmmo;
    private float nextFireTime = 0f;
    private Camera fpsCam;

    void Start()
    {
        currentAmmo = maxAmmo;

        fpsCam = Camera.main; // should find the main camera
        if (fpsCam == null)
        {
            fpsCam = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if (Mouse.current == null) return;

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
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit object: " + hit.collider.name);

            ZombieHealth zombie = hit.collider.GetComponent<ZombieHealth>();
            if (zombie != null)
            {
                zombie.TakeDamage(damage);
                Debug.Log("Successfully damaged the zombie!");
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing (shot into the air).");
        }
    }
}