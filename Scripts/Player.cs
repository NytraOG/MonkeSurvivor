using System;
using Godot;

namespace MonkeSurvivor.Scripts;

public partial class Player : PanelContainer
{
    private TextureRect texture;

    [Export]
    public float Velocity { get; set; } = 100;

    public float DiagonalSpeed => (float)Math.Sqrt(Math.Pow(Velocity, 2) / 2);

    public override void _Ready() => texture = GetNode<TextureRect>(nameof(TextureRect));

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.A) && Input.IsKeyPressed(Key.W))
        {
            if (!texture.FlipH)
                texture.FlipH = true;

            SetPosition(new Vector2(Position.X - DiagonalSpeed * (float)delta, Position.Y - DiagonalSpeed * (float)delta));
        }
        else if (Input.IsKeyPressed(Key.S) && Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            SetPosition(new Vector2(Position.X + DiagonalSpeed * (float)delta, Position.Y + DiagonalSpeed * (float)delta));
        }
        else if (Input.IsKeyPressed(Key.W) && Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            SetPosition(new Vector2(Position.X + DiagonalSpeed * (float)delta, Position.Y - DiagonalSpeed * (float)delta));
        }
        else if (Input.IsKeyPressed(Key.A) && Input.IsKeyPressed(Key.S))
        {
            if (texture.FlipH)
                texture.FlipH = true;

            SetPosition(new Vector2(Position.X - DiagonalSpeed * (float)delta, Position.Y + DiagonalSpeed * (float)delta));
        }
        else if (Input.IsKeyPressed(Key.A))
        {
            if (!texture.FlipH)
                texture.FlipH = true;

            SetPosition(new Vector2(Position.X - Velocity * (float)delta, Position.Y));
        }
        else if (Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            SetPosition(new Vector2(Position.X + Velocity * (float)delta, Position.Y));
        }
        else if (Input.IsKeyPressed(Key.W))
            SetPosition(new Vector2(Position.X, Position.Y - Velocity * (float)delta));
        else if (Input.IsKeyPressed(Key.S))
            SetPosition(new Vector2(Position.X, Position.Y + Velocity * (float)delta));
    }
}