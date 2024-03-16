using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class EndOfWavePanel : PanelContainer
{
    private PackedScene PostBattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/player.tscn");

    public override void _Ready() { }

    public override void _Process(double delta) { }

    public void _on_button_pressed() => GetTree().ChangeSceneToPacked(PostBattleScene);
}