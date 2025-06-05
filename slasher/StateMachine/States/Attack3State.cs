using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class Attack3State : PlayerBaseState
{
    public Attack3State(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Attack3, "attack3", false);
        Data.Velocity = new Vector2(0, Data.Velocity.Y);
    }

    public override void OnUpdateBehavior(KeyboardState ks)
    {
        if (Player.CurrentAnimation.IsFinished)
        {
            MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
        }
    }
}