using Microsoft.Xna.Framework;

namespace slasher;

public class IdleState : PlayerBaseState
{
    public IdleState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Idle, "idle");
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }
}