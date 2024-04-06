using Godot;
using MonkeSurvivor.Scripts.Ui;

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

    protected override void DieProperly()
    {
        if (GetTree().CurrentScene is Battle battle)
        {
            var xpToken = XpTokenScene.Instantiate<XpToken>();
            xpToken.Position = Position;
            battle.AddChild(xpToken);

            var unitspawner = battle.GetNode<UnitSpawner>(nameof(UnitSpawner));
            unitspawner.SpawnedEnemies.Remove(this);

            chasedPlayer.Enemies.Remove(this);
        }

        base.DieProperly();
    }

    public void ChasePlayer()
    {
        if (!isAggressive || chasedPlayer.IsDead)
            return;

        var direction = (chasedPlayer.Position - Position).Normalized();
        Velocity = Speed * direction;

        MoveAndSlide();

        for (var i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision      = GetSlideCollision(i);
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
        var totalDamagereduction = (chasedPlayer.IncreasedDamagereduction - chasedPlayer.DecreasedDamagereduction)/100;
        var resultingDamagePercentage = 1 - totalDamagereduction;
        var finalDamage = resultingDamagePercentage * DealtDamage;
        
        chasedPlayer.HealthCurrent -= finalDamage;
        chasedPlayer.InstatiateFloatingCombatText((int)DealtDamage, chasedPlayer.Position, false, false);
    }

    public void StartChasingPlayer(Player player)
    {
        chasedPlayer = player;
        isAggressive = true;
    }
}