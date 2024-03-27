using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scenes;

public partial class Shop : Node
{
    private          CharacterSheet characterSheet;
    private          Inventory      inventory;
    private readonly List<string>   itemScenes = new();
    private          Label          moneyDisply;
    private          ShopPanel      shopPanel;
    private          PackedScene    BattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/battle.tscn");

    public override void _Ready()
    {
        GetTree().Paused = false;

        GenerateItems();

        shopPanel      = GetNode<ShopPanel>("%" + nameof(ShopPanel));
        inventory      = GetNode<Inventory>("%" + nameof(Inventory));
        characterSheet = GetNode<CharacterSheet>("%" + nameof(CharacterSheet));
        moneyDisply    = shopPanel.GetNode<Label>("%PlayerMoney");

        moneyDisply.Text = StaticMemory.HeldMoney.ToString();

        characterSheet.OnAttributeRaised += CharacterSheetOnOnAttributeRaised;
    }

    public void _on_button_pressed() => GetTree().ChangeSceneToPacked(BattleScene);

    private void GenerateItems()
    {
        var itemDirectory = "res://Scenes/Items/";

        using var directory = DirAccess.Open(itemDirectory);

        if (directory is not null)
        {
            directory.ListDirBegin();
            var fileName = directory.GetNext();

            while (fileName != "")
            {
                if (directory.CurrentIsDir())
                    return;

                if (itemScenes.All(s => !s.Contains(fileName)))
                    itemScenes.Add(itemDirectory + fileName);

                fileName = directory.GetNext();
            }
        }
        else
            GD.Print($"An error occurred when trying to access the path. '{itemDirectory}'");
    }

    private void CharacterSheetOnOnAttributeRaised(string attributename) { }
}