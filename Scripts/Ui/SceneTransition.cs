using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class SceneTransition : CanvasLayer
{
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer                   =  GetNode<AnimationPlayer>(nameof(AnimationPlayer));
        animationPlayer.AnimationFinished += AnimationPlayerOnAnimationFinished;

        FadeInBlackness();
    }

    private void AnimationPlayerOnAnimationFinished(StringName animname)
    {
        animationPlayer.Play("fade_out_blackness");

        animationPlayer.AnimationFinished -= AnimationPlayerOnAnimationFinished;
    }

    public override void _Process(double delta) { }

    public void FadeInBlackness() => animationPlayer.Play("fade_in_blackness");
}