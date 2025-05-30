using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class JumpStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;
    private readonly Player _player;

    public JumpStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData, Player player)
    {
        StateMachine = stateMachine;
        _stateData = stateData;
        _player = player;
    }

    public bool CanHandle(GameTime gameTime)
    {
        return _stateData.IsJumpPressed && _stateData.IsGrounded && StateMachine.Player.State != PlayerState.Dash &&
               StateMachine.Player.State is not (PlayerState.Attack1 or PlayerState.Attack2 or PlayerState.Attack3 or
                   PlayerState.AirAttack or PlayerState.SpecialAttack) && !_player.IsBlockedAbove();
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<JumpState>();
    }
}