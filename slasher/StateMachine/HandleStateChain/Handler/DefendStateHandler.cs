using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class DefendStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public DefendStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        return _stateData.IsDefendPressed && _stateData.IsGrounded &&
               StateMachine.Player.State is not (PlayerState.Jump or PlayerState.Dash or PlayerState.HurtBlock or
                   PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or PlayerState.AirAttack or
                   PlayerState.SpecialAttack);
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<DefendState>();
    }
}