using UnityEngine;

// 1. THE ABSTRACT BASE STATE
public abstract class PlayerBaseState
{
    protected PlayerController ctx; // Access to the controller
    public PlayerBaseState(PlayerController context) { ctx = context; }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}

// 2. WALKING STATE
public class PlayerState_Walk : PlayerBaseState
{
    public PlayerState_Walk(PlayerController currentContext) : base(currentContext) { }

    public override void Enter() { }

    public override void Update()
    {
        // Transitions
        if (ctx.input.SprintHeld && ctx.isGrounded)
            ctx.SwitchState(ctx.SprintState);

        if (ctx.input.CrouchHeld && ctx.isGrounded)
            ctx.SwitchState(ctx.CrouchState);

        if (ctx.input.JumpTriggered && ctx.readyToJump && ctx.isGrounded)
        {
            ctx.DoJump();
            // We don't necessarily need to switch to Air state explicitly here
            // because the next frame !isGrounded will trigger the AirState check below.
        }

        if (!ctx.isGrounded)
            ctx.SwitchState(ctx.AirState);
    }

    public override void FixedUpdate()
    {
        ctx.MovePlayer(ctx.stats.walkSpeed);
    }
    public override void Exit() { }
}

public class PlayerState_Sprint : PlayerBaseState
{
    public PlayerState_Sprint(PlayerController currentContext) : base(currentContext) { }

    public override void Enter() { }

    public override void Update()
    {
        if (!ctx.input.SprintHeld && ctx.isGrounded)
            ctx.SwitchState(ctx.WalkState);

        if (ctx.input.JumpTriggered && ctx.readyToJump && ctx.isGrounded)
            ctx.DoJump();

        if (!ctx.isGrounded)
            ctx.SwitchState(ctx.AirState);
    }

    public override void FixedUpdate()
    {
        ctx.MovePlayer(ctx.stats.sprintSpeed);
    }
    public override void Exit() { }
}

// 4. CROUCHING STATE
public class PlayerState_Crouch : PlayerBaseState
{
    public PlayerState_Crouch(PlayerController currentContext) : base(currentContext) { }

    public override void Enter()
    {
        ctx.ChangeScale(ctx.stats.crouchYScale);
        ctx.AddDownForce(5f);
    }

    public override void Update()
    {
        // Stop crouching check
        if (!ctx.input.CrouchHeld)
        {
            // Ceiling Check (prevent standing up inside tunnels)
            float ceilingDist = (ctx.stats.startYScale - ctx.stats.crouchYScale) + 0.2f;
            Vector3 origin = ctx.transform.position + Vector3.up * (ctx.stats.crouchYScale * 0.5f);

            if (!Physics.Raycast(origin, Vector3.up, ceilingDist, ctx.stats.whatIsGround))
            {
                ctx.SwitchState(ctx.WalkState); // Stand up
            }
        }

        if (!ctx.isGrounded)
            ctx.SwitchState(ctx.AirState);
    }

    public override void FixedUpdate()
    {
        ctx.MovePlayer(ctx.stats.crouchSpeed);
    }

    public override void Exit()
    {
        ctx.ChangeScale(ctx.stats.startYScale);
    }
}

// 5. AIR STATE
public class PlayerState_Air : PlayerBaseState
{
    public PlayerState_Air(PlayerController currentContext) : base(currentContext) { }

    public override void Enter() { }

    public override void Update()
    {
        if (ctx.isGrounded)
        {
            ctx.SwitchState(ctx.WalkState);
        }
    }

    public override void FixedUpdate()
    {
        // Moving in air (usually same speed as walk or sprint, handled by logic)
        ctx.MovePlayer(ctx.stats.walkSpeed);
    }
    public override void Exit() { }
}