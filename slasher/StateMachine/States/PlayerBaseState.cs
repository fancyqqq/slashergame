using Microsoft.Xna.Framework.Input;

namespace slasher;

public abstract class PlayerBaseState : IState
{
    protected readonly PlayerStateData Data;
    protected readonly TestPlayer _testPlayer;
    protected readonly StateMachineInitialization _machineInitialization;

    public PlayerBaseState(PlayerStateData data, TestPlayer testPlayer, StateMachineInitialization machineInitialization)
    {
        Data = data;
        _testPlayer = testPlayer;
        _machineInitialization = machineInitialization;
    }

    public virtual void OnEnter()
    {
        
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdateBehaviour(KeyboardState ks)
    {
        
    }
}