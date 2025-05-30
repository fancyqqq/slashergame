using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace slasher;

public class PlayerStateData
{
    public bool IsJumpPressed;
    public bool IsLeftPressed;
    public bool IsRightPressed;
    public bool IsAttackPressed;
    public bool IsDefendPressed;
    public bool IsSpecialPressed;
    public bool IsGrounded;
    public Vector2 Velocity;
    public bool IsFacingRight = true;
    public float GroundY = 300f;
    public JumpPhase CurrentJumpPhase;
    public TiledMapTileLayer CollisionLayer;
    public int TileSize;
    public float Gravity = 1200f;
    public float FallGravityMultiplier = 2f;
    // dash
    public bool CanDash = true;
    public float DashTime;
    public float DashDuration = 0.25f;
    public float DashSpeed = 1400f;
    public float DashLerp = 0.96f;
    public float DashCooldown = 1f;
    public float DashCooldownTimer;
    public bool WindPlaying;
    public Vector2 WindPosition;
    public bool WindFacingRight = true;
    public Animation WindEffect;
    public float LastAPressTime = -1f;
    public float LastDPressTime = -1f;
    public bool APressedLastFrame;
    public bool DPressedLastFrame;
    public const float DoubleTapInterval = 0.5f;
    // attack
    public bool AttackQueued;
    public bool AttackPressedLastFrame;
    public bool SpecialStarted;
}

public enum JumpPhase
{
    None,
    Start,
    Transition,
    Fall
}