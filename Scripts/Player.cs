using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using MonkeSurvivor.Scenes;
using MonkeSurvivor.Scripts.Enemies;
using MonkeSurvivor.Scripts.Monkeys;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Utils;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts;

public partial class Player : BaseUnit
{
    private Node               battleScene;
    private bool               invincibilityRunning;
    private double             millisecondsSinceLastHit;
    private double             regenerationTimer;
    private RessourceIndicator ressourceIndicator;
    private float              swingTimer;
    private TextureRect        texture;
    private int                bananasHeld;
    public  List<BaseEnemy>    Enemies                  { get; set; }
    public  StaticBody2D       WieldedWeaponRightHand   { get; set; }
    public  StaticBody2D       WieldedWeaponLeftHand    { get; set; }
    public  StaticBody2D       WieldedWeaponTail        { get; set; }
    public  StaticBody2D       WieldedWeaponHeadmounted { get; set; }

    [Export]
    public float Speed { get; set; } = 400;

    public int BananasHeld
    {
        get => bananasHeld;
        set => SetField(ref bananasHeld, value);
    }

    public int BananasSpent { get; set; }

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

    public override void _Ready() => Initialize();

    public void Initialize()
    {
        battleScene = GetTree().CurrentScene;
        var unitSpawner = battleScene.GetNode<UnitSpawner>(nameof(UnitSpawner));
        unitSpawner.WaveSpawned += UnitSpawnerOnWaveSpawned;

        ressourceIndicator = battleScene.GetNode<CanvasLayer>("UI")
                                        .GetNode<RessourceIndicator>(nameof(RessourceIndicator));

        texture   = GetNode<TextureRect>(nameof(TextureRect));

        invincibilityRunning = true;

        PropertyChanged += OnPropertyChanged;
    }

    private void UnitSpawnerOnWaveSpawned()
    {
        var allChildren = battleScene.GetChildren();
        var allEnemies  = allChildren.Where(c => c is BaseEnemy);

        Enemies = allEnemies
                 .Cast<BaseEnemy>()
                 .ToList();
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BananasHeld))
        {
            if(GetTree() is null)
                return;
            
            if (!IsInstanceValid(ressourceIndicator) && GetTree().CurrentScene is Shop shop)
            {
                ressourceIndicator = shop.GetNode<CanvasLayer>("UI")
                                         .GetNode<ShopPanel>("%" + nameof(ShopPanel))
                                         .GetNode<RessourceIndicator>("%" + nameof(RessourceIndicator));
            }
            else if (!IsInstanceValid(ressourceIndicator) && GetTree().CurrentScene is Battle battle)
            {
                ressourceIndicator = battle.GetNode<CanvasLayer>("UI")
                                           .GetNode<RessourceIndicator>(nameof(RessourceIndicator));
            }
            
            ressourceIndicator.SetBananaAmount(BananasHeld);
        }

        if (e.PropertyName != nameof(HealthCurrent))
            return;

        invincibilityRunning = true;
    }

    public void SetMonkeyClass(BaseMonkey monkey)
    {
        texture ??= GetNode<TextureRect>(nameof(TextureRect));
        //Apply Modifiers
        WieldedWeaponRightHand = monkey.StartingWeapon.Instantiate<StaticBody2D>();
        texture.Texture        = monkey.ClassSprite;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!IsInstanceValid(battleScene))
            battleScene = GetTree().CurrentScene;

        ResolveRegenerationTicks(delta);
        ResolveInvincibility(delta);
        ProgressSwingtimer(delta);
    }

    private void ResolveRegenerationTicks(double delta)
    {
        regenerationTimer += delta;

        var regeneratedAmountPerSecond = FinalHealthregeneration;

        if (regeneratedAmountPerSecond > 0)
        {
            var regeneratedAmountPerFrame = delta * regeneratedAmountPerSecond;

            if (regeneratedAmountPerFrame + HealthCurrent > HealthMaximum)
                HealthCurrent = HealthMaximum;
            else
                HealthCurrent += (float)regeneratedAmountPerFrame;

            if (regenerationTimer >= 1)
            {
                InstatiateFloatingCombatText((int)regeneratedAmountPerSecond, Position, false, true);

                regenerationTimer = 0;
            }
        }
    }

    private void ProgressSwingtimer(double delta)
    {
        if (WieldedWeaponRightHand is null)
            return;

        swingTimer += (float)delta;

        if (WieldedWeaponRightHand is BaseWeapon wieldedWeapon && swingTimer >= wieldedWeapon.SwingCooldown)
        {
            swingTimer = 0;

            if (Enemies is null || !Enemies.Any())
                return;

            var duplicateWeapon = (BaseWeapon)wieldedWeapon.Duplicate();
            duplicateWeapon.Enemies  = Enemies;
            duplicateWeapon.Position = Position;

            duplicateWeapon.OnDamageDealt += damage =>
            {
                if (battleScene is Battle battle)
                {
                    battle.GetNode<CanvasLayer>("UI")
                          .GetNode<DpsDisplay>("DpsDisplay")
                          .DamageDealtInTimeFrame += damage;
                }
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
            Velocity = Vector2.Zero;

        MoveAndSlide();
    }
}