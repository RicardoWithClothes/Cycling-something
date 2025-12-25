using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    // Public properties for other scripts to read
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool CrouchTriggered { get; private set; }

    private void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        JumpTriggered = Input.GetKeyDown(jumpKey);
        SprintHeld = Input.GetKey(sprintKey);
        CrouchHeld = Input.GetKey(crouchKey);
        CrouchTriggered = Input.GetKeyDown(crouchKey);
    }
}
