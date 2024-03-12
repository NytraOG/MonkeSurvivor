using Godot;

namespace MonkeSurvivor.Scripts;

public partial class UnitSpawner : Control
{
    [Export]
    public PackedScene UnitToSpawn { get; set; }

    [Export]
    public int AmountPerWave { get; set; }

    [Export]
    public float WaveCooldown { get; set; }

    [Export]
    public float WaveCooldownModifier { get; set; }

    public override void _Ready() { }

    public override void _Process(double delta) { }
}