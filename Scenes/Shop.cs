using Godot;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scenes;

public partial class Shop : Node
{
    private CharacterSheet characterSheet;
    private Inventory      inventory;
    private Label          moneyDisply;
    private ShopPanel      shopPanel;
    private PackedScene    BattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/battle.tscn");

    public override void _Ready()
    {
        GetTree().Paused = false;

        shopPanel      = GetNode<ShopPanel>("%" + nameof(ShopPanel));
        inventory      = GetNode<Inventory>("%" + nameof(Inventory));
        characterSheet = GetNode<CharacterSheet>("%" + nameof(CharacterSheet));
        moneyDisply    = shopPanel.GetNode<Label>("%PlayerMoney");

        moneyDisply.Text = StaticMemory.HeldMoney.ToString();

        characterSheet.OnAttributeRaised += CharacterSheetOnOnAttributeRaised;
    }

    public override void _Process(double delta) { }

    public void _on_button_pressed() => GetTree().ChangeSceneToPacked(BattleScene);

    private void CharacterSheetOnOnAttributeRaised(string attributename)
    {
        var upgradeCost = StaticMemory.Player.GetAttributeUpgradeCost(attributename);


    }
}