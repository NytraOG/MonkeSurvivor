using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts.Enemies;

public abstract partial class BaseEnemy : BaseUnit
{
    private Player      chasedPlayer;
    private bool        isAggressive;
    private PackedScene xpTokenScene;

    [Export]
    public float Speed { get; set; } = 300;

    [Export]
    public float DealtDamage { get; set; } = 10;

    [Export]
    public int XpOnKill { get; set; } = 1;

    public PackedScene XpTokenScene
    {
        get
        {
            xpTokenScene ??= ResourceLoader.Load<PackedScene>("res://Scenes/xp_token.tscn");

            return xpTokenScene;
        }
    }

    public override void _PhysicsProcess(double delta) => ChasePlayer();

    protected override void DieProperly()
    {
        if (GetTree().CurrentScene is Battle battle)
        {
            var xpToken = XpTokenScene.Instantiate<XpToken>();
            xpToken.Position = Position;
            battle.AddChild(xpToken);

            chasedPlayer.Enemies.Remove(this);
        }

        base.DieProperly();
    }

    private void ChasePlayer()
    {
        if (!isAggressive || chasedPlayer.IsDead)
            return;

        var direction = (chasedPlayer.Position - Position).Normalized();
        Velocity = Speed * direction;

        MoveAndSlide();

        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);

            var collidedObject = (Node)collision.GetCollider();

            if (collidedObject.Name == nameof(Player))
            {
                if (!chasedPlayer.IsInvicible)
                    DealDamageToPlayer();
            }
        }
    }

    private void DealDamageToPlayer()
    {
        chasedPlayer.HealthCurrent -= DealtDamage;
        chasedPlayer.InstatiateFloatingCombatText((int)DealtDamage, chasedPlayer.Position, false);
    }

    public void StartChasingPlayer(Player player)
    {
        chasedPlayer = player;
        isAggressive = true;
    }
}