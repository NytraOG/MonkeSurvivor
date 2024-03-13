using System;
using System.ComponentModel;
using Godot;
using MonkeSurvivor.Scripts.Monkeys;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts;

public partial class Player : BaseUnit
{
    private bool        invincibilityRunning;
    private double      millisecondsSinceLastHit;
    private float       swingTimer;
    private TextureRect texture;
    public  RigidBody2D WieldedWeapon { get; set; }

    [Export]
    public float Speed { get; set; } = 100;

    [Export]
    public int InvicibilityTimeMilliseconds { get; set; } = 1000;

    public bool IsInvicible
    {
        get
        {
            if (InvicibilityTimeMilliseconds > millisecondsSinceLastHit)
                return true;

            invincibilityRunning     = false;
            millisecondsSinceLastHit = 0;

            return false;
        }
    }

    public float DiagonalSpeed => (float)Math.Sqrt(Math.Pow(Speed, 2) / 2);

    public override void _Ready()
    {
        texture = GetNode<TextureRect>(nameof(TextureRect));

        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(HealthCurrent))
            return;

        invincibilityRunning = true;
    }

    public void SetMonkeyClass(BaseMonkey monkey)
    {
        //Apply Modifiers
        var kek = monkey.StartingWeapon.Instantiate<RigidBody2D>();
        WieldedWeapon = kek;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        ResolveInvincibility(delta);
        ProgressSwingtimer(delta);
    }

    private void ProgressSwingtimer(double delta)
    {
        if (WieldedWeapon is null)
            return;

        swingTimer += (float)delta;

        if ( WieldedWeapon is BaseWeapon wieldedWeapon && swingTimer >= wieldedWeapon.SwingCooldown)
        {
            var duplicateWeapon = (BaseWeapon)wieldedWeapon.Duplicate();
            //Das kommt in die Waffe rein
            duplicateWeapon.Position = Position;


            var target = duplicateWeapon.FindTargetOrDefault();

            if (target is null)
                return;

            var direction = (target.Position - duplicateWeapon.Position).Normalized();
            duplicateWeapon.AngularVelocity = 600f;
            duplicateWeapon.MoveAndCollide(direction);

            swingTimer = 0;
        }
    }

    private void ResolveInvincibility(double delta)
    {
        if (millisecondsSinceLastHit > InvicibilityTimeMilliseconds)
            invincibilityRunning = false;

        if (invincibilityRunning)
            millisecondsSinceLastHit += delta * 1000;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsKeyPressed(Key.A) && Input.IsKeyPressed(Key.W))
        {
            if (!texture.FlipH)
                texture.FlipH = true;

            var direction = Vector2.Left + Vector2.Up;

            Velocity = direction * DiagonalSpeed;
        }
        else if (Input.IsKeyPressed(Key.S) && Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            var direction = Vector2.Down + Vector2.Right;

            Velocity = direction * DiagonalSpeed;
        }
        else if (Input.IsKeyPressed(Key.W) && Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            var direction = Vector2.Up + Vector2.Right;

            Velocity = direction * DiagonalSpeed;
        }
        else if (Input.IsKeyPressed(Key.A) && Input.IsKeyPressed(Key.S))
        {
            if (texture.FlipH)
                texture.FlipH = true;

            var direction = Vector2.Down + Vector2.Left;

            Velocity = direction * DiagonalSpeed;
        }
        else if (Input.IsKeyPressed(Key.A))
        {
            if (!texture.FlipH)
                texture.FlipH = true;

            var direction = Vector2.Left;

            Velocity = direction * Speed;
        }
        else if (Input.IsKeyPressed(Key.D))
        {
            if (texture.FlipH)
                texture.FlipH = false;

            var direction = Vector2.Right;

            Velocity = direction * Speed;
        }
        else if (Input.IsKeyPressed(Key.W))
        {
            var direction = Vector2.Up;

            Velocity = direction * Speed;
        }
        else if (Input.IsKeyPressed(Key.S))
        {
            var direction = Vector2.Down;

            Velocity = direction * Speed;
        }
        else
            Velocity = Vector2.Zero;

        MoveAndSlide();
    }
}