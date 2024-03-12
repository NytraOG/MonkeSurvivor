using Godot;

namespace MonkeSurvivor.Scripts;

public abstract partial class Enemy : Unit
{
    private Player chasedPlayer;
    private bool   isAggressive;

    [Export]
    public float Speed { get; set; } = 300;

    public float DealtDamage { get; set; } = 10;

    public override void _PhysicsProcess(double delta) => ChasePlayer();

    private void ChasePlayer()
    {
        if (!isAggressive)
            return;

        var direction = (chasedPlayer.Position - Position).Normalized();
        Velocity = Speed * direction;

        LookAt(chasedPlayer.Position);
        MoveAndSlide();
    }

    public void StartChasingPlayer(Player player)
    {
        chasedPlayer = player;
        isAggressive = true;
    }
}