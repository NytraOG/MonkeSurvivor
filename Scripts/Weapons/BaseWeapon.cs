using Godot;

namespace MonkeSurvivor.Scripts.Weapons;

public abstract partial class BaseWeapon : RigidBody2D
{
    [Export]
    public int DamageOnHit { get; set; } = 10;

    [Export]
    public float SwingCooldown { get; set; }

}