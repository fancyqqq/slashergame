using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class AirAttackState : PlayerBaseState
{
    public AirAttackState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.AirAttack, "air_attack", false);
    }

    public override void OnUpdateBehavior(KeyboardState ks)
    {
        if (Data.IsLeftPressed && !Data.IsRightPressed)
        {
            Player.Velocity = new Vector2(-Data.AirSpeed, Player.Velocity.Y);
            Data.IsFacingRight = false;
        }
        else if (Data.IsRightPressed && !Data.IsLeftPressed)
        {
            Player.Velocity = new Vector2(Data.AirSpeed, Player.Velocity.Y);
            Data.IsFacingRight = true;
        }
        
        if (Player.CurrentAnimation.IsFinished)
        {
            Data.CurrentJumpPhase = JumpPhase.Transition;
            Player.SetAnimation("jump_trans", false);
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
        }
        else if (Data.IsGrounded)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<RunState>();
            MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}