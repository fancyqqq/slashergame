using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class HurtBlockStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public HurtBlockStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle()
    {
        return StateMachine.Player.State == PlayerState.HurtBlock;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<HurtBlockState>();
    }
}