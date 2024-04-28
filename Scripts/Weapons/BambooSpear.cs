using Godot;
using MonkeSurvivor.Scripts.Ui;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class BambooSpear : BaseMeleeWeapon
{
    private const string SwingAnimation = "WeaponSwing";
    private const string PokeAnimation = "WeaponPoke";
    private AnimationPlayer animationPlayer;
    private string lastPlayedAnimation;
    private double timeSinceLastSwing;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("%" + nameof(AnimationPlayer));
        animationPlayer.AnimationFinished += AnimationPlayerOnAnimationFinished;

        OnDamageDealt += damage =>
        {
            if (GetTree().CurrentScene is Battle battle)
                battle.GetNode<CanvasLayer>("UI")
                    .GetNode<DpsDisplay>("DpsDisplay")
                    .DamageDealtInTimeFrame += damage;
        };
    }

    protected override void ExecuteBehaviour(double delta)
    {
        var target = FindClosestTargetOrDefault();

        var overlappingBodies = GetOverlappingBodies();

        foreach (var body in overlappingBodies)
            ExecuteAttack(body);

        //Meleewaffenticks impln noch( soll nicht jeden frame damage machen)
        if (animationPlayer.IsPlaying() || (timeSinceLastSwing += delta) < SwingCooldown)
            return;

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        var impactCollisionShape = GetNode<CollisionShape2D>("%ImpactCollision");
        impactCollisionShape.Disabled = false;

        switch (lastPlayedAnimation)
        {
            case null:
                lastPlayedAnimation = SwingAnimation;
                animationPlayer.AssignedAnimation = SwingAnimation;
                animationPlayer.Play();
                break;
            case SwingAnimation:
                lastPlayedAnimation = PokeAnimation;
                animationPlayer.AssignedAnimation = PokeAnimation;
                animationPlayer.Play();
                break;
            case PokeAnimation:
                lastPlayedAnimation = SwingAnimation;
                animationPlayer.AssignedAnimation = SwingAnimation;
                animationPlayer.Play();
                break;
        }

        timeSinceLastSwing = 0;
    }

    private void AnimationPlayerOnAnimationFinished(StringName animname)
    {
        var implactCollisionShape = GetNode<CollisionShape2D>("%ImpactCollision");
        implactCollisionShape.Disabled = true;
    }
}