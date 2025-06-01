using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace slasher;

public class Game1 : Game
{
    private TiledMap _map;
    private TiledMapRenderer _mapRenderer;
    private Player _player;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _debugPixel;
    private StateMachineInitialization _stateMachineInitialization;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        DisplayMode display = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        _graphics.PreferredBackBufferWidth = display.Width;
        _graphics.PreferredBackBufferHeight = display.Height;
        _graphics.IsFullScreen = false;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _debugPixel = new Texture2D(GraphicsDevice, 1, 1);
        _debugPixel.SetData(new[] { Color.White });
        
        _map = Content.Load<TiledMap>("slasher_map");
        _mapRenderer = new TiledMapRenderer(GraphicsDevice, _map);
        var collisionLayer = _map.GetLayer<TiledMapTileLayer>("Collisions");
        
        int groundY = FindGroundLevel(collisionLayer, _map.TileHeight);
        
        _player = new Player(Content, GraphicsDevice, collisionLayer, _map.TileWidth, groundY);
        _stateMachineInitialization = new StateMachineInitialization(_player);
    }
    
    private int FindGroundLevel(TiledMapTileLayer layer, int tileSize)
    {
        for (int y = 0; y < layer.Height; y++)
        {
            for (int x = 0; x < layer.Width; x++)
            {
                if (layer.GetTile((ushort)x, (ushort)y).GlobalIdentifier != 0)
                    return y * tileSize;
            }
        }
        return 0;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var ks = Keyboard.GetState();
        var ms = Mouse.GetState();
    
        var inputReader = new InputReader(_stateMachineInitialization.PlayerStateData);
        inputReader.Read(ks, ms);

        _stateMachineInitialization.StateHandleChain.HandleState(gameTime);
        _stateMachineInitialization.PlayerStateMachine.Update(ks);
    
        _player.Update(gameTime, ks, ms);
        _mapRenderer.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);

    _mapRenderer.Draw();

    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

    _player.Draw(_spriteBatch);

    DrawRectHollow(_spriteBatch, _player.BoundingBox, 2, Color.Red);

    var predictiveBoxes = new List<Rectangle>
    {
        new Rectangle(_player.BoundingBox.Left - _player.StateData.TileSize, _player.BoundingBox.Y, _player.StateData.TileSize, _player.BoundingBox.Height),
        new Rectangle(_player.BoundingBox.Right, _player.BoundingBox.Y, _player.StateData.TileSize, _player.BoundingBox.Height),
        new Rectangle(_player.BoundingBox.X, _player.BoundingBox.Top - _player.StateData.TileSize, _player.BoundingBox.Width, _player.StateData.TileSize),
        new Rectangle(_player.BoundingBox.X, _player.BoundingBox.Bottom, _player.BoundingBox.Width, _player.StateData.TileSize),
        new Rectangle(_player.BoundingBox.Left - _player.StateData.TileSize, _player.BoundingBox.Top - _player.StateData.TileSize, _player.StateData.TileSize, _player.StateData.TileSize),
        new Rectangle(_player.BoundingBox.Right, _player.BoundingBox.Top - _player.StateData.TileSize, _player.StateData.TileSize, _player.StateData.TileSize)
    };
    foreach (var box in predictiveBoxes)
    {
        DrawRectHollow(_spriteBatch, box, 1, Color.Green);
    }

    _spriteBatch.End();

    base.Draw(gameTime);
}

public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness, Color color)
{
    Texture2D rectTexture = new Texture2D(GraphicsDevice, 1, 1);
    rectTexture.SetData(new[] { color });

    spriteBatch.Draw(
        rectTexture,
        new Rectangle(rect.X, rect.Y, rect.Width, thickness),
        color
    );
    spriteBatch.Draw(
        rectTexture,
        new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness),
        color
    );
    spriteBatch.Draw(
        rectTexture,
        new Rectangle(rect.X, rect.Y, thickness, rect.Height),
        color
    );
    spriteBatch.Draw(
        rectTexture,
        new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height),
        color
    );
}
}