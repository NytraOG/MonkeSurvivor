using Godot;

namespace MonkeSurvivor.Scripts.Monkeys;

public partial class Mandrill : BaseMonkey
{
    public override PackedScene StartingWeapon => ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/coconut_grenade.tscn");

}