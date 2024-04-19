using Godot;
using MonkeSurvivor.Scripts.Ui;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class BambooSpear : BaseMeleeWeapon
{
    private AnimationPlayer animationPlayer;
    private double          timeSinceLastSwing;

    public override void _Ready()
    {
        animationPlayer                   =  GetNode<AnimationPlayer>("%" + nameof(AnimationPlayer));
        animationPlayer.AnimationFinished += AnimationPlayerOnAnimationFinished;

        OnDamageDealt += damage =>
        {
            if (GetTree().CurrentScene is Battle battle)
            {
                battle.GetNode<CanvasLayer>("UI")
                      .GetNode<DpsDisplay>("DpsDisplay")
                      .DamageDealtInTimeFrame += damage;
            }
        };
    }

    protected override void ExecuteBehaviour(double delta)
    {
        var target            = FindClosestTargetOrDefault();

        var overlappingBodies = GetOverlappingBodies();

        foreach (var body in overlappingBodies)
            ExecuteAttack(body);

        //Meleewaffenticks impln noch( soll nicht jeden frame damage machen)
        if (animationPlayer.IsPlaying() || (timeSinceLastSwing += delta) < SwingCooldown)
            return;

        var implactCollisionShape = GetNode<CollisionShape2D>("%ImpactCollision");
        implactCollisionShape.Disabled = false;

        animationPlayer.Play("WeaponSwing");

        timeSinceLastSwing = 0;
    }

    private void AnimationPlayerOnAnimationFinished(StringName animname)
    {
        var implactCollisionShape = GetNode<CollisionShape2D>("%ImpactCollision");
        implactCollisionShape.Disabled = true;
    }
}