using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class IdleStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public IdleStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        bool canHandle;
        canHandle = !_stateData.IsLeftPressed && !_stateData.IsRightPressed && _stateData.IsGrounded &&
               StateMachine.Player.State is not (PlayerState.Jump or
                   PlayerState.HurtBlock or PlayerState.Attack2 or PlayerState.Attack3 or
                   PlayerState.AirAttack);
        bool cnhdl = canHandle;
        if (StateMachine.Player.State is PlayerState.Attack1)
            cnhdl = StateMachine.Player.CurrentAnimation.IsFinished;
        
        return canHandle && cnhdl;
        
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<IdleState>();
    }
}