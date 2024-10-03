using Godot;

namespace MonkeSurvivor.Scripts.Weapons.Melee;

public partial class GorillaFist : BaseMeleeWeapon
{
    private const string PoundAnimation = "GroundPound";
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        base._Ready();
        
        animationPlayer = GetNode<AnimationPlayer>("%" + nameof(AnimationPlayer));
        animationPlayer.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished(StringName animname)
    {
        
    }

    protected override void ExecuteBehaviour(double delta)
    {
        
    }
}