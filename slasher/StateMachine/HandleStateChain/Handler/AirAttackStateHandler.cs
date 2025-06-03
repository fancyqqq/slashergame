using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class AirAttackStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public AirAttackStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        bool isPressed = _stateData.IsAttackPressed && !_stateData.AttackPressedLastFrame;
        _stateData.AttackPressedLastFrame = _stateData.IsAttackPressed;
        return isPressed && !_stateData.IsGrounded &&
               StateMachine.Player.State is PlayerState.Jump;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<AirAttackState>();
    }
}