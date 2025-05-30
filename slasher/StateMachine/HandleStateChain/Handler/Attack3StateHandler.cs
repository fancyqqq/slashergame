using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class Attack3StateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public Attack3StateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
    }

    public bool CanHandle(GameTime gameTime)
    {
        bool isPressed = _stateData.IsAttackPressed && !_stateData.AttackPressedLastFrame;
        _stateData.AttackPressedLastFrame = _stateData.IsAttackPressed;
        if (isPressed && StateMachine.Player.State == PlayerState.Attack2)
        {
            _stateData.AttackQueued = true;
            return true;
        }
        return false;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<Attack3State>();
    }
}