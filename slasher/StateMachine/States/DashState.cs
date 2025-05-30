using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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

        bool dashRight = Data.IsRightPressed || Data.IsFacingRight;
        Data.WindPosition = Player.Position + new Vector2(dashRight ? 25 : -25, 0);
        Data.WindFacingRight = dashRight;
        Data.WindEffect = Player.GetAnimation("wind");
        Data.WindEffect.IsLooping = false;
        Data.WindEffect.Reset();
        Data.WindPlaying = true;

        Data.Velocity = new Vector2(dashRight ? Data.DashSpeed : -Data.DashSpeed, Data.Velocity.Y);
    }

    public override void OnExit()
    {
        Data.DashTime = 0f;
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        Data.DashTime -= 1f / 60f;
        Data.Velocity = new Vector2(Data.Velocity.X * Data.DashLerp, Data.Velocity.Y);

        if (Data.WindPlaying)
        {
            Data.WindEffect.Update(Player.GameTime);
            if (Data.WindEffect.IsFinished)
                Data.WindPlaying = false;
        }

        if (Data.DashTime <= 0f && Player.CurrentAnimation.IsFinished)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}