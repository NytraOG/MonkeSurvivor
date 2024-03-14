using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;
using MonkeSurvivor.Scripts.Monkeys;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts;

public partial class Player : BaseUnit
{
    private Node battleScene;
    private IEnumerable<BaseEnemy> enemies;
    private bool invincibilityRunning;
    private double millisecondsSinceLastHit;
    private float swingTimer;
    private TextureRect texture;
    private int xpCurrent;
    public RigidBody2D WieldedWeapon { get; set; }

    [Export] public float Speed { get; set; } = 100;

    public int XpCurrent
    {
        get => xpCurrent;
        set => SetField(ref xpCurrent, value);
    }

    public int XpSpent { get; set; }

    [Export] public int InvicibilityTimeMilliseconds { get; set; } = 1000;

    public bool IsInvicible
    {
        get
        {
            if (InvicibilityTimeMilliseconds > millisecondsSinceLastHit)
                return true;

            invincibilityRunning = false;
            millisecondsSinceLastHit = 0;

            return false;
        }
    }

    public float DiagonalSpeed => (float)Math.Sqrt(Math.Pow(Speed, 2) / 2);

    public override void _Ready()
    {
        battleScene = GetTree().CurrentScene;
        var unitSpawner = battleScene.GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.WaveSpawned += UnitSpawnerOnWaveSpawned;
        texture = GetNode<TextureRect>(nameof(TextureRect));

        PropertyChanged += OnPropertyChanged;
    }

    private void UnitSpawnerOnWaveSpawned()
    {
        var allChildren = battleScene.GetChildren();
        var allEnemies = allChildren.Where(c => c is BaseEnemy);

        enemies = allEnemies
            .Cast<BaseEnemy>()
            .ToList();
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

        if (WieldedWeapon is BaseWeapon wieldedWeapon && swingTimer >= wieldedWeapon.SwingCooldown)
        {
            swingTimer = 0;

            if (enemies is null || !enemies.Any())
                return;

            var duplicateWeapon = (BaseWeapon)wieldedWeapon.Duplicate();
            duplicateWeapon.Enemies = enemies;
            duplicateWeapon.Position = Position;
            duplicateWeapon.OnDamageDealt += damage =>
            {
                if (battleScene is Battle battle) battle.GetNode<CanvasLayer>("UI")
                    .GetNode<DpsDisplay>("DpsDisplay").DamageDealtInTimeFrame += damage;
            };

            battleScene.AddChild(duplicateWeapon);
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
        {
            Velocity = Vector2.Zero;
        }

        MoveAndSlide();
    }
}