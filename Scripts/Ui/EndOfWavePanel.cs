using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class EndOfWavePanel : PanelContainer
{
    private Player      player;
    private PackedScene PostBattleScene      => ResourceLoader.Load<PackedScene>("res://Scripts/shop.tscn");
    public  PackedScene SceneTransitionScene => ResourceLoader.Load<PackedScene>("res://Scenes/Ui/scene_transition.tscn");

    public override void _Ready() => player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));

    public override void _Process(double delta) { }

    public void _on_button_pressed()
    {
        StaticMemory.Player       = player;
        StaticMemory.HeldMoney    = player.XpCurrent;
        StaticMemory.Vigor        = player.Vigor;
        StaticMemory.Strength     = player.Strength;
        StaticMemory.Dexterity    = player.Dexterity;
        StaticMemory.Intelligence = player.Intelligence;
        StaticMemory.WaveNumber++;

        GetTree().ChangeSceneToFile("res://Scripts/shop.tscn");
    }
}