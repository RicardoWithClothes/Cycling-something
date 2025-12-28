using UnityEngine;

public class DevCamera : MonoBehaviour
{
    [Header("References")]
    public Transform playerCameraHolder; // The empty object inside Player where camera normally sits
    public PlayerController playerMovement; // Your movement script
    public CameraController mouseLook; // Your mouse look script (so we can turn it off)

    [Header("Settings")]
    public float flySpeed = 20f;
    public float sprintMultiplier = 3f;
    public KeyCode toggleKey = KeyCode.Tab;

    private Camera mainCam;
    private bool isDevMode = false;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        mainCam = Camera.main;

        // Initialize angles to current camera rotation so it doesn't snap weirdly
        yaw = mainCam.transform.eulerAngles.y;
        pitch = mainCam.transform.eulerAngles.x;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleDevMode();
        }

        if (isDevMode)
        {
            MoveCamera();
            RotateCamera();
        }
    }

    void ToggleDevMode()
    {
        isDevMode = !isDevMode;

        // 1. Disable/Enable Player Controls
        if (playerMovement != null) playerMovement.enabled = !isDevMode;
        if (mouseLook != null) mouseLook.enabled = !isDevMode;

        if (!isDevMode)
        {
            // --- RETURN TO PLAYER ---
            // Re-parent camera to the player
            mainCam.transform.SetParent(playerCameraHolder);

            // Reset position to exact zero (local to the holder)
            mainCam.transform.localPosition = Vector3.zero;
            mainCam.transform.localRotation = Quaternion.identity;

            // Lock cursor again
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            // --- ENTER GOD MODE ---
            // Detach camera so it can fly freely
            mainCam.transform.SetParent(null);

            // Unlock cursor (optional, but usually better to keep locked for flying)
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void MoveCamera()
    {
        float speed = flySpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);
        Vector3 move = Vector3.zero;

        // Standard WASD + Q/E for Up/Down
        if (Input.GetKey(KeyCode.W)) move += mainCam.transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= mainCam.transform.forward;
        if (Input.GetKey(KeyCode.D)) move += mainCam.transform.right;
        if (Input.GetKey(KeyCode.A)) move -= mainCam.transform.right;
        if (Input.GetKey(KeyCode.E)) move += Vector3.up;
        if (Input.GetKey(KeyCode.Q)) move -= Vector3.down;

        mainCam.transform.position += move * speed * Time.deltaTime;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * 2f;
        float mouseY = Input.GetAxis("Mouse Y") * 2f;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        mainCam.transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}