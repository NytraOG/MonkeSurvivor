using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Enemies;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : StaticBody2D
{
    public delegate void DamageDealtEventHandler(float totalDamage);

    protected Area2D                  ImpactArea;
    public    DamageDealtEventHandler OnDamageDealt;
    private   Player                  player;
    protected Random                  Rng;
    protected Area2D                  SplashArea;
    protected BaseEnemy               Target;
    public    IEnumerable<BaseEnemy>  Enemies { get; set; }
    protected List<string> AlreadyHitEnemyNames = new (); 

    [Export]
    public int MinDamage { get; set; }

    [Export]
    public int MaxDamage { get; set; }

    [Export]
    public float KnockbackForce { get; set; }
    
    public HitResult FinalDamage
    {
        get
        {
            Rng ??= new Random();

            player ??= GetTree()
                      .CurrentScene
                      .GetNode<Player>(nameof(Player));

            var roll   = Rng.Next(0, 101);
            var isCrit = player.CriticalHitChance >= roll;

            var damageDelta = MaxDamage - MinDamage;
            var addedRandomizedDamageValue = Rng.Next(0, damageDelta);
            var damageInRange = MinDamage + addedRandomizedDamageValue;

            var dealtDamage = isCrit ? damageInRange * (100 + player.CriticalHitDamage / 100) : damageInRange;

            return new HitResult(dealtDamage, isCrit);
        }
    }

    [Export]
    public float SwingCooldown { get; set; }

    [Export]
    public bool DealsSplashDamage { get; set; }

    [Export(PropertyHint.Range, "0, 1")]
    public float SplashDamage { get; set; } = 0.75f;

    [Export]
    public float Speed { get; set; } = 50;

    public float TotalDamageDealt { get; set; }
    public int   EnemyCount       { get; set; }

    public override void _PhysicsProcess(double delta) => ExecuteBehaviour(delta);

    protected virtual List<Node2D> GetOverlappingBodies()
    {
        ImpactArea ??= GetNode<Area2D>("%ImpactArea");

        var overlappingBodies = ImpactArea.GetOverlappingBodies()
                                          .Where(b => b.Name != nameof(Player))
                                          .ToList();
        
        return overlappingBodies;
    }

    protected abstract void ExecuteBehaviour(double delta);

    public void DealDamageTo(BaseEnemy enemy, float multiplier = 1)
    {
        var damage = FinalDamage.DamageDealt * multiplier;
        TotalDamageDealt    += damage;
        enemy.HealthCurrent -= damage;
        enemy.InstatiateFloatingCombatText((int)damage, enemy.Position, FinalDamage.IsCritical, false);
    }

    protected virtual void ExecuteAttack(Node node)
    {
        if (node is not BaseEnemy enemy || AlreadyHitEnemyNames.Any(e => e == enemy.Name))
            return;
        
        AlreadyHitEnemyNames.Add(enemy.Name);
        
        DealDamageTo(enemy);

        if (DealsSplashDamage)
            DealSplashDamageAround(enemy);

        OnDamageDealt?.Invoke(TotalDamageDealt);
    }

    protected virtual BaseEnemy FindRandomTargetOrDefault()
    {
        if (Target is not null)
            return Target;

        var eligebleTargets = Enemies?.Where(e => !e.IsDead)
                                      .ToList();

        if (eligebleTargets == null)
            return null;

        EnemyCount =   eligebleTargets.Count;
        Rng        ??= new Random();

        var randomNumber = Rng.Next(EnemyCount);

        if (eligebleTargets.Count == EnemyCount)
            Target = eligebleTargets[randomNumber];

        return Target;
    }

    protected virtual BaseEnemy FindClosestTargetOrDefault()
    {
        if (Enemies is null || !Enemies.Any())
            return null;

        var enemyPositionContainer = Enemies.Select(e => new
        {
            e,
            e.Position
        });

        enemyPositionContainer = enemyPositionContainer.OrderBy(e => (e.Position - Position).Length());

        return enemyPositionContainer.FirstOrDefault()?.e;
    }

    protected void DealSplashDamageAround(BaseEnemy enemy)
    {
        SplashArea ??= GetNode<Area2D>("SplashArea");

        var overlappingBodies = SplashArea.GetOverlappingBodies().Where(b => b.Name != nameof(Player));

        foreach (var hitEnemy in overlappingBodies.Where(b => b is BaseEnemy).Cast<BaseEnemy>())
        {
            if (hitEnemy != enemy)
                DealDamageTo(hitEnemy, SplashDamage);
        }
    }
}