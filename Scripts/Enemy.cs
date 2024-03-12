using Godot;

namespace MonkeSurvivor.Scripts;

public abstract partial class Enemy : Unit
{
    private Player player;
    [Export]
    public  double      Velocity { get; set; }

    public override void _Draw()
    {
        base._Draw();

        player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);


    }

    public override void _Process(double delta)
    {
        base._Process(delta);


    }
}