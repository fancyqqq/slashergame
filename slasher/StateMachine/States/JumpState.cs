using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;
public class JumpState : PlayerBaseState
{
    private IMoveStrategy _moveStrategy;
    private readonly float _jumpForce = -550f;

    public JumpState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
        _moveStrategy = new AirMoveStrategy(Data, Player);
    }

    public override void OnEnter()
    {
        switch (Data.CurrentJumpPhase)
        {
            case JumpPhase.None:
                Player.SetStateAndAnimation(PlayerState.Jump, "jump_start");
                Data.CurrentJumpPhase = JumpPhase.Start;
                Player.Velocity = new Vector2(Data.Velocity.X, _jumpForce);
                break;

            case JumpPhase.Transition:
                Player.SetStateAndAnimation(PlayerState.Jump, "jump_trans", false);
                break;

            case JumpPhase.Fall:
                Player.SetStateAndAnimation(PlayerState.Jump, "jump_fall");
                break;
        }
    }

    public override void OnExit()
    {
        Data.CurrentJumpPhase = JumpPhase.None;
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
    }

    public override void OnUpdateBehavior(KeyboardState ks)
    {
        System.Diagnostics.Debug.WriteLine("Jump State");
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
                else if (Data.IsGrounded)
                {
                    if (Data.IsLeftPressed || Data.IsRightPressed)
                        MachineInitialization.PlayerStateMachine.SwitchStates<RunState>();
                    else
                        MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
                }
                break;
        }
        
        _moveStrategy.Move(); 
        
        if (Data.IsGrounded && Data.CurrentJumpPhase == JumpPhase.Fall)
        {
            if (Data.IsLeftPressed || Data.IsRightPressed)
                MachineInitialization.PlayerStateMachine.SwitchStates<RunState>();
            else
                MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
        MachineInitialization.StateHandleChain.HandleState<DashStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<AirAttackStateHandler>();
    }
}