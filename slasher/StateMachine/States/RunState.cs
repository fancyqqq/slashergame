using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public class RunState : PlayerBaseState
{
    private IMoveStrategy _moveStrategy;

    public RunState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
        : base(data, player, machineInitialization)
    {
        _moveStrategy = new GroundMoveStrategy(Data, Player);
    }

    public override void OnEnter()
    {
        Player.SetStateAndAnimation(PlayerState.Run, "run");
    }

    public override void OnExit()
    {
        Player.Velocity = Vector2.Zero;
    }
    
    public override void OnUpdateBehavior()
    {
        _moveStrategy.Move();
        
        MachineInitialization.StateHandleChain.HandleState<IdleStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<Attack1StateHandler>();
        MachineInitialization.StateHandleChain.HandleState<JumpStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DashStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<DefendStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<SpecialAttackStateHandler>();
        MachineInitialization.StateHandleChain.HandleState<WallJumpStateHandler>();
    }
}