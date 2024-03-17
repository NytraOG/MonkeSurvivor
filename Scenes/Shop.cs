using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scenes;

public partial class Shop : Node
{
    private PackedScene BattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/battle.tscn");

    public override void _Ready() => GetTree().Paused = false;

    public override void _Process(double delta) { }

    public void _on_button_pressed()
    {
        StaticMemory.HeldXp -= 20;

        GetTree().ChangeSceneToPacked(BattleScene);
    }
}