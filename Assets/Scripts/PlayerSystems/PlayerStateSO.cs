using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Data/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Movement")]
    public float walkSpeed = 7f;
    public float sprintSpeed = 10f;
    public float groundDrag = 5f;
    public float airMultiplier = 0.4f;

    [Header("Jumping")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;

    [Header("Crouching")]
    public float crouchSpeed = 3.5f;
    public float crouchYScale = 0.5f;
    public float startYScale = 1f;

    [Header("Sliding")]
    public float maxSlideTime = 0.75f;
    public float slideForce = 200f;
    public float slideYScale = 0.5f;

    [Header("Ground Detection")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    public float maxSlopeAngle = 40f;
}