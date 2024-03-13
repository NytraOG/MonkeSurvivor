using Godot;

namespace MonkeSurvivor.Scripts.Monkeys;

public partial class Orangutan : BaseMonkey
{
    [Export]
    public Texture2D Sprite { get; set; }

    public override PackedScene StartingWeapon => ResourceLoader.Load<PackedScene>("res://Scenes/Weapons/coconut_grenade.tscn");
}