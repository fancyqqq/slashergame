using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace slasher;
public class JumpState : PlayerBaseState
{
    private readonly float _jumpForce = -550f;

    public JumpState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Jump, "jump_start");
        Data.CurrentJumpPhase = JumpPhase.Start;
        Data.Velocity = new Vector2(Data.Velocity.X, _jumpForce);
    }

    public override void OnExit()
    {
        Data.CurrentJumpPhase = JumpPhase.None;
    }

    public override void OnUpdateBehaviour(KeyboardState ks)
    {
        switch (Data.CurrentJumpPhase)
        {
            case JumpPhase.Start:
                if (Data.Velocity.Y > -75f)
                {
                    Data.CurrentJumpPhase = JumpPhase.Transition;
                    if (Player.State != PlayerState.AirAttack)
                        Player.SetAnimation("jump_trans", false);
                }
                break;

            case JumpPhase.Transition:
                if (Player.CurrentAnimation == Player.GetAnimation("jump_trans") &&
                    Player.CurrentAnimation.IsFinished)
                {
                    Data.CurrentJumpPhase = JumpPhase.Fall;
                    Player.SetAnimation("jump_fall");
                }
                break;
        }

        if (Data.CurrentJumpPhase == JumpPhase.None && Data.Velocity.Y > 0 && Player.Position.Y < Data.GroundY)
        {
            Data.CurrentJumpPhase = JumpPhase.Transition;
            Player.SetAnimation("jump_trans", false);
        }
    }
}