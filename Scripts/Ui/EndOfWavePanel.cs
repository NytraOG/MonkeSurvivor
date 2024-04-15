using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class EndOfWavePanel : PanelContainer
{
    private PackedScene PostBattleScene      => ResourceLoader.Load<PackedScene>("res://Scripts/shop.tscn");
    public  PackedScene SceneTransitionScene => ResourceLoader.Load<PackedScene>("res://Scenes/Ui/scene_transition.tscn");

    public override void _Process(double delta) { }

    public void _on_button_pressed()
    {
        StaticMemory.Player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
        StaticMemory.WaveNumber++;

        var tree = GetTree();
        tree.CurrentScene.RemoveChild(StaticMemory.Player);
        tree.ChangeSceneToPacked(PostBattleScene);
    }
}