using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class Attack1StateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public Attack1StateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        bool isPressed = _stateData.IsAttackPressed;
        return isPressed && _stateData.IsGrounded &&
               StateMachine.Player.State is PlayerState.Idle or PlayerState.Run;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<Attack1State>();
    }
}