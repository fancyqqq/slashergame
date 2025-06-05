using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class Attack1State : PlayerBaseState
{
    public Attack1State(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Data.IsAttackPressed = false;
        Player.SetStateAndAnimation(PlayerState.Attack1, "attack1", false);
        Player.Velocity = new Vector2(0, Player.Velocity.Y);
        Data.OnAttackJustPressed += QueueNextAttack;
    }

    public override void OnExit()
    {
        Data.OnAttackJustPressed -= QueueNextAttack;
        Data.AttackQueued = false;
    }
    
    private void QueueNextAttack()
    {
        System.Diagnostics.Debug.WriteLine("Attack Queued");
        Data.AttackQueued = true;
    }
    
    public override void OnUpdateBehavior(KeyboardState ks)
    {
        Player.Velocity = new Vector2(0, Player.Velocity.Y);

        if (Player.CurrentAnimation.IsFinished)
        {
            if (Data.AttackQueued)
            {
                MachineInitialization.PlayerStateMachine.SwitchStates<Attack2State>();
            }
            else
            {
                MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
                MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
            }
        }
    }
}
// Player.Velocity = new Vector2(0, Player.Velocity.Y);
// if (Player.CurrentAnimation.IsFinished)
// {
//     MachineInitialization.StateHandleChain.HandleState<RunStateHandler>();
//     MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
// }
// else if (Data.IsAttackPressed)
// {
//     System.Diagnostics.Debug.WriteLine("переход в attack2");
//     MachineInitialization.StateHandleChain.HandleState<Attack2StateHandler>();
// }