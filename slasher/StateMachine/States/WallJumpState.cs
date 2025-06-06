using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
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
        
        Player.Velocity = new Vector2(Player.Velocity.X, 0);
    }

    public override void OnExit()
    {
        Data.WallJumpBlocked = true;
        Data.CurrentJumpPhase = JumpPhase.None;
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    public override void OnUpdateBehavior()
    {
        if (!_isSliding && Player.CurrentAnimation?.Name == "wall_contact" && Player.CurrentAnimation.IsFinished)
        {   
            Player.SetAnimation("wall_slide", true);
            _isSliding = true;
        }

        if (_isSliding)
        {
            bool pressingWallSide = Data.IsFacingRight ? Data.IsRightPressed : Data.IsLeftPressed;

            if (pressingWallSide)
                Player.Velocity = new Vector2(0, Data.SlidingSpeed);
            else
            {
                Data.CurrentJumpPhase = JumpPhase.Transition;
                Player.SetAnimation("jump_trans", false);
                MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
            }
        }
        
        if (!_isSliding && !Data.IsTouchingWall(Player))
        {
            Data.CurrentJumpPhase = JumpPhase.Transition;
            Player.SetAnimation("jump_trans", false);
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
            return;
        }
        
        if (Data.IsJumpPressed && Data.IsTouchingWall(Player))
        {
            Data.WallJump = true;
            MachineInitialization.PlayerStateMachine.SwitchStates<JumpState>();
        }
        
        MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
    }
}
