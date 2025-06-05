using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

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
        Player.Velocity = new Vector2(0, Data.Velocity.Y);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdateBehavior()
    {
        if (!Data.IsDefendPressed)
        {
            MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
            MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
        }
    }
}