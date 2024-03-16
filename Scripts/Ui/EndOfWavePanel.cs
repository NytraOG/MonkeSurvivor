using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class EndOfWavePanel : PanelContainer
{
    private PackedScene PostBattleScene => ResourceLoader.Load<PackedScene>("res://Scripts/shop.tscn");

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void _on_button_pressed()
    {
        StaticMemory.StaticString = "hihi";
        GetTree().ChangeSceneToPacked(PostBattleScene);
    }
}