using Godot;

namespace MonkeSurvivor.Scripts.Weapons.Melee;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export]
    public double TimeBetweenDamageTicks { get; set; }

    protected double TimeSinceLastTick;
} 