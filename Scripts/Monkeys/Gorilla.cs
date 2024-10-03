using Godot;

namespace MonkeSurvivor.Scripts.Monkeys;

public partial class Gorilla : BaseMonkey
{
  public override PackedScene StartingWeapon => ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/gorilla_fist.tscn");
}