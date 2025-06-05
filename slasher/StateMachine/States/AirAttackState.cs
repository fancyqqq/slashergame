using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class AirAttackState : PlayerBaseState
{
    private IMoveStrategy _moveStrategy;
    public AirAttackState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
        _moveStrategy = new AirMoveStrategy(Data, Player);
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.AirAttack, "air_attack", false);
    }

    public override void OnUpdateBehavior(KeyboardState ks)
    {
        _moveStrategy.Move();
        
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