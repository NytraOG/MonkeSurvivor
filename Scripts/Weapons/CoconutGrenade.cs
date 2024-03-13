using System.Linq;
using MonkeSurvivor.Scripts.Enemies;

namespace MonkeSurvivor.Scripts.Weapons;

public partial class CoconutGrenade : BaseWeapon
{
    public override void _Process(double delta) { }

    public override BaseEnemy FindTargetOrDefault() => (BaseEnemy)Enemies?.FirstOrDefault();
}