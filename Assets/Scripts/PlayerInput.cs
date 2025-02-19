using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerInput : NetworkBehaviour
{
    private Player player;

    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;
    private CursorMode cursorMode = CursorMode.Auto;
    public LayerMask mask;
    public GameObject projectile;
    private Ray ray;

    private Camera cam;
    private Vector3 direction;
    [SerializeField] private Animator animator;

    private Gun currentGun;
    private GameObject pistol;


    private GameObject canvas;

    private bool activateElements = false;

    private bool playerPaused = false;
    private GameObject pauseMenu;

    public GameObject pausePrefab;

    private HealthComponent healthComponent;
    private int equippedGun = 1;

    private bool canControl = false;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) {
            return;
        }
        
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        direction = Vector3.zero;

        player = GetComponent<Player>();
        GameObject gunHolder = player.GetGunHolder();
        pistol = gunHolder.transform.GetChild(0).gameObject;

        currentGun = pistol.GetComponent<Gun>();
        pistol.SetActive(true);

        GameManager.instance.OnGameStart += Test_OnGameStart;

        canvas = transform.Find("HUD").gameObject;

        healthComponent = GetComponent<HealthComponent>();

        // Set the main camera to follow the player
        cam = Camera.main;
        CameraFollow temp = cam.GetComponent<CameraFollow>();
        temp.SetTarget(gameObject);
    }

    private void Awake()
    {
        if (!IsOwner) {
            return;
        }
    }

    private void Test_OnGameStart(object sender, EventArgs e) {
        canControl = true;
        GameManager.instance.OnGameStart -= Test_OnGameStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) {
            return;
        }

        if (canControl) {
            // Activate the camera for the current player
            if (!activateElements) {
                activateElements = true;
                canvas.SetActive(true);
            }

            // Handle animation for moving
            if (direction != Vector3.zero) {
                animator.SetBool("isMoving", true);
            }
            else {
                animator.SetBool("isMoving", false);
            }

            if (healthComponent.currentHP.Value > 0.0f && !playerPaused) {
                // Get player input
                direction = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

                // Use a raycast to point the player towards the mouse pointer
                ray = cam.ScreenPointToRay(Input.mousePosition);

                // Switch weapons
                if (Input.GetKeyDown(KeyCode.Alpha1) && equippedGun != 1) {
                    equippedGun = 1;
                    // Set the attack and reload bool to false
                    currentGun.SwapWeapons();

                    // Then switch to weapon
                    pistol.SetActive(true);
                    currentGun = pistol.GetComponent<Gun>();
                }

                // Shoot Gun
                if (Input.GetMouseButtonDown(0) && currentGun.canShoot) {
                    currentGun.isAttacking = true;
                    currentGun.Shoot();
                }

                // Reload Gun
                if ((Input.GetKeyDown(KeyCode.R) && currentGun.canReload) || (currentGun.totalInMag == 0 && currentGun.canReload)) {
                    currentGun.canReload = false;
                    currentGun.Reload();
                }
            }
            else if (healthComponent.currentHP.Value <= 0.0f || playerPaused) {
                direction = Vector3.zero;
            }

            // Pause
            if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu == null) {
                playerPaused = true;
                var pausePrefab = Resources.Load<GameObject>("Prefabs/Network/Pause Menu(Network)");
                pauseMenu = Instantiate(pausePrefab, Vector3.zero, transform.rotation, transform);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu != null) {
                playerPaused = false;
                Destroy(pauseMenu);
            }
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) {
            return;
        }
        
        if (healthComponent.currentHP.Value > 0.0f 
                && !playerPaused 
                && canControl) {
            // Move in the direction that the player presses
            player.Move(direction);

            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, mask)) {
                // Debug.DrawRay(cam.transform.position, ray.direction * hit.distance, Color.green, 0, false);

                // Rotate the player to look in the direction of the crosshair
                Vector3 directionTarget = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(directionTarget, Vector3.up);
                player.GetRigidbody().rotation = lookRotation;
            }
        }
    }

    public bool GetPlayerPaused()
    {
        return playerPaused;
    }

    public void ChangePause(bool value)
    {
        playerPaused = value;
    }

    [ServerRpc]
    void SpawnPauseMenuServerRpc()
    {
        pauseMenu = Instantiate(pausePrefab, Vector3.zero, transform.rotation, transform);
        pauseMenu.GetComponent<NetworkObject>().Spawn();
    }
}
