using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class WallJumpStateHandler : IStateHandle
{
    public StateMachineInitialization StateMachine { get; }
    private readonly PlayerStateData _stateData;

    public WallJumpStateHandler(StateMachineInitialization stateMachine, PlayerStateData stateData)
    {
        _stateData = stateData;
        StateMachine = stateMachine;
    }

    public bool CanHandle()
    {
        bool canWallJump = _stateData.IsTouchingWall(StateMachine.Player) && !_stateData.IsGrounded;
        bool canHandle = canWallJump && StateMachine.Player.State is not (PlayerState.Dash or PlayerState.AirAttack);
        System.Diagnostics.Debug.WriteLine("canWallJump: " + canHandle);
        return canHandle;
    }

    public void Handle()
    {
        StateMachine.PlayerStateMachine.SwitchStates<WallJumpState>();
    }
}