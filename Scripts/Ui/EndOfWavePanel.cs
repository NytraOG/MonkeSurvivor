using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class EndOfWavePanel : PanelContainer
{
    private Player      player;
    private PackedScene PostBattleScene => ResourceLoader.Load<PackedScene>("res://Scripts/shop.tscn");

    public override void _Ready() => player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));

    public override void _Process(double delta) { }

    public void _on_button_pressed()
    {
        StaticMemory.WaveNumber++;
        StaticMemory.HeldXp = player.XpCurrent;

        GetTree().ChangeSceneToPacked(PostBattleScene);
    }
}