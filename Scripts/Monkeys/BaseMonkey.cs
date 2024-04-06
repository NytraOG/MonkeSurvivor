using Godot;

namespace MonkeSurvivor.Scripts.Monkeys;

public abstract partial class BaseMonkey : Node2D
{
    [Export]
    public CompressedTexture2D ClassSprite { get; set; }

    public abstract PackedScene StartingWeapon { get; }
}