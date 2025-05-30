using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class SpecialAttackStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public SpecialAttackStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle(GameTime gameTime)
    {
        bool isPressed = _stateData.IsSpecialPressed && !_stateData.SpecialStarted;
        return isPressed && _stateData.IsGrounded &&
               StateMachine.Player.State is PlayerState.Idle or PlayerState.Run;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<SpecialAttackState>();
    }
}