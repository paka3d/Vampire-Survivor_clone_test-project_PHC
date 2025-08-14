using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAgentController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 0.1f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int maxAmmo = 30;

    [Header("Look Options")]
    [SerializeField] private bool enablePitch = true;
    [SerializeField] private bool enableYaw = true;

    [Header("Zoom")]
    [SerializeField] private float minFov = 30f;
    [SerializeField] private float maxFov = 60f;
    [SerializeField] private float zoomSpeed = 10f;

    public int CurrentAmmo => ammo;
    public int CurrentHealth => health;

    private int ammo;
    private int health = 100;
    private Camera cam;
    private PlayerInput playerInput;
    private float pitch = 0f;

    public int Level { get; private set; } = 1;
    private int enemiesDefeated = 0;
    public LevelUpUI levelUpUI;

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        if (cam == null)
            Debug.LogError("Camera not found on PlayerAgentController!");

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            Debug.LogError("PlayerInput not found on PlayerAgentController!");

        ammo = maxAmmo;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pitch = cam != null ? cam.transform.localEulerAngles.x : 0f;
        if (pitch > 180f) pitch -= 360f;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleZoom();
        if (playerInput != null && playerInput.actions["Fire"].WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void HandleMovement()
    {
        if (playerInput == null) return;
        Vector2 dir = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = (transform.right * dir.x + transform.forward * dir.y).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    private void HandleLook()
    {
        if (playerInput == null) return;
        Vector2 mouseDelta = playerInput.actions["Look"].ReadValue<Vector2>();
        if (enableYaw)
        {
            float deltaYaw = mouseDelta.x * lookSensitivity;
            transform.Rotate(0f, deltaYaw, 0f, Space.World);
        }
        if (enablePitch && cam != null)
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
        if (b.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = bulletSpawn.forward * bulletSpeed;
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            if (GameManager.instance != null)
                GameManager.instance.PlayerDied();
            else
                Debug.LogError("GameManager.instance is null!");
        }
    }

    public void Reload() => ammo = maxAmmo;

    public void AddAmmo(int amount) => ammo = Mathf.Min(ammo + amount, maxAmmo);

    public void SetHealth(int newHealth) => health = newHealth;

    private void HandleZoom()
    {
        if (cam == null) return;
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
    }

    public void IncreaseSpeed(float amount = 1f)
    {
        moveSpeed += amount;
    }
}
