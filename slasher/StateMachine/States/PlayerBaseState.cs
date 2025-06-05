using Microsoft.Xna.Framework.Input;
using slasher.HandleStateChain;

namespace slasher;

public abstract class PlayerBaseState : IState
{
    protected readonly PlayerStateData Data;
    protected readonly Player Player;
    protected readonly StateMachineInitialization MachineInitialization;

    public PlayerBaseState(PlayerStateData data, Player player, StateMachineInitialization machineInitialization)
    {
        Data = data;
        Player = player;
        MachineInitialization = machineInitialization;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdateBehavior(KeyboardState ks)
    {
        
    }
}