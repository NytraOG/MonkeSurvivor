using Godot;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseMeleeWeapon : BaseWeapon
{
    [Export]
    public double TimeBetweenDamageTicks { get; set; }

    protected double TimeSinceLastTick;
} //TODO disbale weapon collision method hier rein