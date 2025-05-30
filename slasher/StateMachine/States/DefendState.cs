using Microsoft.Xna.Framework;

namespace slasher;

public class DefendState : PlayerBaseState
{
    public DefendState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Defend, "defend");
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }
}