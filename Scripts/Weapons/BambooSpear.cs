using Godot;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class BambooSpear : BaseMeleeWeapon
{
    private AnimationPlayer animationPlayer;
    private double          timeSinceLastSwing;

    public override void _Ready() => animationPlayer = GetNode<AnimationPlayer>(nameof(AnimationPlayer));

    protected override void ExecuteBehaviour(double delta)
    {
        var overlappingBodies = GetOverlappingBodies();

        foreach (var body in overlappingBodies)
            ExecuteAttack(body);

        if (animationPlayer.IsPlaying() || (timeSinceLastSwing += delta) < SwingCooldown)
            return;

        animationPlayer.Play("WeaponSwing");

        timeSinceLastSwing = 0;
    }
}