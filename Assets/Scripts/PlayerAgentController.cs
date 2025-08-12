using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgentController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 0.1f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 20f;
    public int maxAmmo = 30;

    [Header("Look Options")]
    public bool enablePitch = true;
    public bool enableYaw = true;

    [Header("Zoom")]
    public float minFov = 30f;
    public float maxFov = 60f;
    public float zoomSpeed = 10f;

    // Public for UI
    public int CurrentAmmo => ammo;
    public int CurrentHealth => health;

    // Internal state
    private int ammo;
    private int health = 100;
    private Camera cam;
    private PlayerInput playerInput;
    private float pitch = 0f; // Add this field to track pitch
    private Vector2 currentMouseDelta;
    private Vector2 smoothMouseDelta;
    public float lookDamping = 0.1f; // You can adjust this in the Inspector

    public int level = 1;
    private int enemiesDefeated = 0;
    // Reference to UI (set in Inspector)
    public LevelUpUI levelUpUI;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        playerInput = GetComponent<PlayerInput>();
        ammo = maxAmmo;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Initialize pitch from camera's local rotation
        pitch = cam.transform.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f; // Convert to -180 to 180 range
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleZoom();
        if (playerInput.actions["Fire"].WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void HandleMovement()
    {
        Vector2 dir = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = (transform.right * dir.x + transform.forward * dir.y).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void HandleLook()
    {
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();
        // Yaw (rotate player horizontally)
        if (enableYaw)
        {
            float yaw = mouseDelta.x * lookSensitivity;
            transform.Rotate(0f, yaw, 0f, Space.World);
        }
        // Pitch (rotate camera vertically)
        if (enablePitch)
        {
            pitch -= mouseDelta.y * lookSensitivity;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            cam.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    private void Shoot()
    {
        if (ammo <= 0) return;
        ammo--;
        GameObject b = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = b.GetComponent<Rigidbody>();
        rb.linearVelocity = bulletSpawn.forward * bulletSpeed;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) GameManager.instance.PlayerDied();
    }

    public void Reload()
    {
        ammo = maxAmmo;
    }

    public void AddAmmo(int amount)
    {
        ammo = Mathf.Min(ammo + amount, maxAmmo);
    }

    // Called by UIManager
    public void SetHealth(int newHealth) => health = newHealth;

    private void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.fieldOfView -= scroll * zoomSpeed * Time.deltaTime;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
        }
    }

    public void OnEnemyDefeated()
    {
        enemiesDefeated++;
        Debug.Log($"Enemies defeated: {enemiesDefeated}");
        // Remove level up UI trigger from here
    }
}
