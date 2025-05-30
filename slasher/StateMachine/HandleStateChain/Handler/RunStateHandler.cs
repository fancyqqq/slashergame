using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class RunStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public RunStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle(GameTime gameTime)
    {
        bool canHandle = (_stateData.IsLeftPressed || _stateData.IsRightPressed) && _stateData.IsGrounded &&
                         StateMachine.Player.State is not (PlayerState.Jump or PlayerState.Dash or PlayerState.Defend or
                             PlayerState.HurtBlock or PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or
                             PlayerState.AirAttack or PlayerState.SpecialAttack);
        System.Diagnostics.Debug.WriteLine($"RunStateHandler.CanHandle: {canHandle}, IsGrounded={_stateData.IsGrounded}, State={StateMachine.Player.State}");
        return canHandle;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<RunState>();
    }
}