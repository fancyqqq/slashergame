using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace slasher.HandleStateChain;

public class StateHandleChain
{
    private readonly List<IStateHandle> _handles;

    public StateHandleChain(List<IStateHandle> handles)
    {
        _handles = handles;
    }

    public void HandleState(GameTime gameTime)
    {
        _handles.FirstOrDefault(h => h.CanHandle(gameTime))?.Handle();
    }

    public void HandleState<T>(GameTime gameTime) where T : IStateHandle
    {
        _handles.FirstOrDefault(h => h.GetType() == typeof(T) && h.CanHandle(gameTime))?.Handle();
    }

    public bool CanHandleState<T>(GameTime gameTime) where T : IStateHandle
    {
        return _handles.Any(h => h.GetType() == typeof(T) && h.CanHandle(gameTime));
    }
}
