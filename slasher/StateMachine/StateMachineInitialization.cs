using System.Collections.Generic;
using slasher.HandleStateChain;

namespace slasher;

public class StateMachineInitialization
{
    public StateMachine PlayerStateMachine { get; private set; }
    public Player Player { get; private set; }
    public PlayerStateData PlayerStateData { get; private set; }
    public StateHandleChain StateHandleChain { get; private set; }

    public StateMachineInitialization(Player player)
    {
        Player = player;
        Initialize();
    }

    private void Initialize()
    {
        PlayerStateData = new PlayerStateData
        {
            CollisionLayer = Player.StateData.CollisionLayer,
            TileSize = Player.StateData.TileSize,
            GroundY = Player.StateData.GroundY
        };
        
        PlayerStateMachine = new StateMachine(
            new IdleState(PlayerStateData, Player, this),
            new RunState(PlayerStateData, Player, this),
            new JumpState(PlayerStateData, Player, this),
            new DashState(PlayerStateData, Player, this),
            new DefendState(PlayerStateData, Player, this),
            new HurtBlockState(PlayerStateData, Player, this),
            new Attack1State(PlayerStateData, Player, this),
            new Attack2State(PlayerStateData, Player, this),
            new Attack3State(PlayerStateData, Player, this),
            new AirAttackState(PlayerStateData, Player, this),
            new SpecialAttackState(PlayerStateData, Player, this)
        );

        StateHandleChain = new StateHandleChain(new List<IStateHandle>
        {
            new HurtBlockStateHandler(this, PlayerStateData),
            new JumpStateHandler(this, PlayerStateData, Player),
            new DashStateHandler(this, PlayerStateData),
            new DefendStateHandler(this, PlayerStateData),
            new Attack1StateHandler(this, PlayerStateData),
            new Attack2StateHandler(this, PlayerStateData),
            new Attack3StateHandler(this, PlayerStateData),
            new AirAttackStateHandler(this, PlayerStateData),
            new SpecialAttackStateHandler(this, PlayerStateData),
            new RunStateHandler(this, PlayerStateData),
            new IdleStateHandler(this, PlayerStateData)
        });

        PlayerStateMachine.SwitchStates<IdleState>();
    }
}