using Godot;
using MonkeSurvivor.Scripts;

namespace MonkeSurvivor;

public partial class Camera : Marker2D
{
    [Export]
    public float Velocity { get; set; } = 500;
    private Camera2D camera;
    private Player   player;

    public override void _Ready()
    {
        camera = GetNode<Camera2D>(nameof(Camera2D));
        player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
    }



    public override void _Process(double delta)
    {

    }
}