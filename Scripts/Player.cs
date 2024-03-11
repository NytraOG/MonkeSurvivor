using Godot;

namespace MonkeSurvivor.Scripts;

public partial class Player : PanelContainer
{
    private TextureRect texture;

    [Export]
    public float Velocity { get; set; } = 10;

    public override void _Ready() => texture = GetNode<TextureRect>(nameof(TextureRect));

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.A))
        {
            if (!texture.FlipH)
                texture.FlipH = true;

            SetPosition(new Vector2(Position.X - Velocity, Position.Y));
        }
        else if (Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            SetPosition(new Vector2(Position.X + Velocity, Position.Y));
        }
        else if (Input.IsKeyPressed(Key.W))
            SetPosition(new Vector2(Position.X, Position.Y - Velocity));
        else if (Input.IsKeyPressed(Key.S))
            SetPosition(new Vector2(Position.X, Position.Y + Velocity));
    }
}