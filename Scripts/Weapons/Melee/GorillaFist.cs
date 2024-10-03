using Godot;

namespace MonkeSurvivor.Scripts.Weapons.Melee;

public partial class GorillaFist : BaseMeleeWeapon
{
    private const string PoundAnimation = "GroundPound";
    private AnimationPlayer animationPlayer;
    private CollisionShape2D collisionArea;

    public override void _Ready()
    {
        base._Ready();

        collisionArea = GetNode<CollisionShape2D>("%ImpactCollision");
        animationPlayer = GetNode<AnimationPlayer>("%" + nameof(AnimationPlayer));
        animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animname)
    {
        collisionArea.Disabled = false;

        var overlappingBodies = GetOverlappingBodies("%ImpactArea");

        foreach (var body in overlappingBodies)
            ExecuteAttack(body);
    }

    protected override void ExecuteBehaviour(double delta)
    {
        if(animationPlayer.IsPlaying())
            return;
        
        animationPlayer.AssignedAnimation = PoundAnimation;
        animationPlayer.Play();
    }
}