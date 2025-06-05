using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class Attack2State : PlayerBaseState
{
    public Attack2State(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
    }

    public override void OnEnter()
    {
        Data.IsAttackPressed = false;
        Player.SetStateAndAnimation(PlayerState.Attack2, "attack2", false);
        Player.Velocity = new Vector2(0, Data.Velocity.Y);
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
        if (Player.CurrentAnimation.IsFinished)
        {
            if (Data.AttackQueued)
                MachineInitialization.PlayerStateMachine.SwitchStates<Attack3State>();
            else
            {
                MachineInitialization.PlayerStateMachine.SwitchStates<IdleState>();
                MachineInitialization.PlayerStateMachine.SwitchStates<RunState>();
            }
        }       
    }
}