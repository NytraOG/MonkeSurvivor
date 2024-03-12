using Godot;

namespace MonkeSurvivor.Scripts;

public abstract partial class Unit : CharacterBody2D
{
    [Export]
    public float HealthCurrent { get; set; }

    [Export]
    public float HealthMaximum { get; set; } = 100;

    public override void _Draw() => HealthCurrent = HealthMaximum;
}