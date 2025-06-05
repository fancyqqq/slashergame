using System;
using Microsoft.Xna.Framework;
using slasher.HandleStateChain;

namespace slasher;

public class WallJumpState : PlayerBaseState
{
    private bool _isSliding = false;

    public WallJumpState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        _isSliding = false;
        Player.SetStateAndAnimation(PlayerState.WallJump, "wall_contact", false);
        Data.CurrentJumpPhase = JumpPhase.None;
        Player.Velocity = Vector2.Zero;
    }

    public override void OnExit()
    {
        Data.CurrentJumpPhase = JumpPhase.None;
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    public override void OnUpdateBehavior()
    {
        Console.WriteLine(!_isSliding && Player.CurrentAnimation == Player.GetAnimation("wall_contact") &&
                          Player.CurrentAnimation.IsFinished);
        if (!_isSliding && Player.CurrentAnimation == Player.GetAnimation("wall_contact") &&
            Player.CurrentAnimation.IsFinished)
        {
            Player.SetAnimation("wall_slide", true);
            _isSliding = true;
        }
        //Console.WriteLine(_isSliding);
        // Скольжение по стене
        if (_isSliding)
        {
            if (Data.IsLeftPressed || Data.IsRightPressed)
                Player.Velocity = new Vector2(0, Data.Velocity.Y);
        }

        // Если отпустил стену или не зацепился за нее, переходим в обычный прыжок
        if (!_isSliding && !Data.IsTouchingWall(Player))
        {
            Data.CurrentJumpPhase = JumpPhase.Transition;
            Player.SetAnimation("jump_trans", false);
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
        }

        // Приземление
        if (Data.IsGrounded)
        {
            MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
            MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
        }

        // Перевод в JumpState при нажатии на пробел
        if (Data.IsJumpPressed && Data.IsTouchingWall(Player))
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
        }
    }
}