using System.Collections.Generic;
using slasher.HandleStateChain;

namespace slasher;

public class StateMachineInitialization
{
    public StateMachine PlayerStateMachine { get; private set; }
    public Player Player { get; private set; }
    public PlayerStateData PlayerStateData => Player.StateData;
    public StateHandleChain StateHandleChain { get; private set; }

    public StateMachineInitialization(Player player)
    {
        Player = player;
        Initialize();
    }

    private void Initialize()
    {
        PlayerStateMachine = new StateMachine(
            new IdleState(Player.StateData, Player, this),
            new RunState(Player.StateData, Player, this),
            new JumpState(Player.StateData, Player, this),
            new DashState(Player.StateData, Player, this),
            new DefendState(Player.StateData, Player, this),
            new HurtBlockState(Player.StateData, Player, this),
            new Attack1State(Player.StateData, Player, this),
            new Attack2State(Player.StateData, Player, this),
            new Attack3State(Player.StateData, Player, this),
            new AirAttackState(Player.StateData, Player, this),
            new SpecialAttackState(Player.StateData, Player, this)
        );

        StateHandleChain = new StateHandleChain(new List<IStateHandle>
        {
            new HurtBlockStateHandler(this, Player.StateData),
            new JumpStateHandler(this, Player.StateData, Player),
            new DashStateHandler(this, Player.StateData),
            new DefendStateHandler(this, Player.StateData),
            new Attack1StateHandler(this, Player.StateData),
            new Attack2StateHandler(this, Player.StateData),
            new Attack3StateHandler(this, Player.StateData),
            new AirAttackStateHandler(this, Player.StateData),
            new SpecialAttackStateHandler(this, Player.StateData),
            new RunStateHandler(this, Player.StateData),
            new IdleStateHandler(this, Player.StateData)
        });

        PlayerStateMachine.SwitchStates<IdleState>();
    }
}