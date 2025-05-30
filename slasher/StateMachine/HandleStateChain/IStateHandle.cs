using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public interface IStateHandle
{
    StateMachineInitialization StateMachine { get; }
    bool CanHandle(GameTime gameTime);
    void Handle();
}