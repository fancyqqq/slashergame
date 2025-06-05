using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class DashState : PlayerBaseState
{
    public DashState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Dash, "dash", false);
        Data.DashTime = Data.DashDuration;
        Data.CanDash = false;
        Data.DashCooldownTimer = Data.DashCooldown;

        bool dashRight = Data.DashDirection > 0;

        Data.WindPosition = Player.Position + new Vector2(dashRight ? 25 : -25, 0);
        Data.WindFacingRight = dashRight;
        Data.WindEffect = Player.GetAnimation("wind");
        Data.WindEffect.IsLooping = false;
        Data.WindEffect.Reset();
        Data.WindPlaying = true;

        Player.Velocity = new Vector2(dashRight ? Data.DashSpeed : -Data.DashSpeed, Data.Velocity.Y);
    }

    public override void OnExit()
    {
        Data.DashTime = 0f;
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    public override void OnUpdateBehavior(KeyboardState ks)
    {
        Data.DashTime -= 1f / 60f;
        Player.Velocity = new Vector2(Data.Velocity.X * Data.DashLerp, Data.Velocity.Y);

        if (Data.WindPlaying)
        {
            Data.WindEffect.Update(Player.GameTime);
            // if (Data.WindEffect.IsFinished)
            //     Data.WindPlaying = false;
        }

        if (Data.DashTime <= 0f && Player.CurrentAnimation.IsFinished)
        {
            if (!Data.IsGrounded)
            {
                Data.CurrentJumpPhase = JumpPhase.Transition;
                Player.SetAnimation("jump_trans", false);
                MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
            }
            MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
            MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
        }
    }
}