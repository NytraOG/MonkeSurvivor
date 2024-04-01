using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scenes;

public partial class Shop : Node
{
    private readonly List<string>   itemScenes = new();
    private          CharacterSheet characterSheet;
    private          Inventory      inventory;
    private          Label          moneyDisply;
    private          ShopPanel      shopPanel;
    private          PackedScene    BattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/battle.tscn");

    public override void _Ready()
    {
        if (StaticMemory.AlreadyReadied)
            return;

        GetTree().Paused = false;

        shopPanel      = GetNode<ShopPanel>("%" + nameof(ShopPanel));
        inventory      = GetNode<Inventory>("%" + nameof(Inventory));
        characterSheet = GetNode<CharacterSheet>("%" + nameof(CharacterSheet));
        moneyDisply    = shopPanel.GetNode<Label>("%PlayerMoney");

        moneyDisply.Text = StaticMemory.HeldMoney.ToString();

        characterSheet.OnAttributeRaised += CharacterSheetOnOnAttributeRaised;
        shopPanel.ItemBought             += ShopPanelOnItemBought;

        GenerateItems();

        StaticMemory.AlreadyReadied = true;
    }

    private void ShopPanelOnItemBought(BaseItem boughtItem) => inventory.SetItem(boughtItem);

    public void _on_button_pressed()
    {
        var itemsFromInventory = inventory.GetAllSlots()
                                          .Where(s => s.ContainedItem is not null)
                                          .Select(s => s.ContainedItem)
                                          .ToList();

        StaticMemory.ItemsHeldByPlayer = itemsFromInventory;
        StaticMemory.AlreadyReadied    = false;

        GetTree().ChangeSceneToPacked(BattleScene);
    }

    private void GenerateItems()
    {
        AssembleItemScenes();

        var itemCount = itemScenes.Count;
        var shopCards = shopPanel.GetShopCards();
        var rng       = new Random();

        for (var i = 0; i < shopCards.Length; i++)
        {
            var number    = rng.Next(0, itemCount);
            var scenePath = itemScenes[number];
            var scene     = ResourceLoader.Load<PackedScene>(scenePath);
            var item      = scene.Instantiate<BaseItem>();

            shopCards[i].SetItem(item);
        }
    }

    private void AssembleItemScenes()
    {
        var itemDirectory = "res://Scenes/Items/";

        using var directory = DirAccess.Open(itemDirectory);

        if (directory is not null)
        {
            directory.ListDirBegin();
            var fileName = directory.GetNext();

            while (!string.IsNullOrWhiteSpace(fileName))
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