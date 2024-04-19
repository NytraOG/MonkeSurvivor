using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Ui;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scenes;

public partial class Shop : Node
{
    private readonly List<string>       itemScenes = new();
    private          CharacterSheet     characterSheet;
    private          Inventory          inventory;
    private          RessourceIndicator ressourceIndicator;
    private          ShopPanel          shopPanel;
    private          WeaponSlot         weaponSlotRightHand;
    private          PackedScene        BattleScene => ResourceLoader.Load<PackedScene>("res://Scenes/battle.tscn");

    public override void _Ready()
    {
        if (StaticMemory.AlreadyReadied)
            return;

        GetTree().Paused = false;

        shopPanel           = GetNode<ShopPanel>("%" + nameof(ShopPanel));
        inventory           = GetNode<Inventory>("%" + nameof(Inventory));
        characterSheet      = GetNode<CharacterSheet>("%" + nameof(CharacterSheet));
        weaponSlotRightHand = GetNode<WeaponSlot>("%WeaponSlotRightHand");

        ressourceIndicator = shopPanel.GetNode<RessourceIndicator>("%" + nameof(RessourceIndicator));
        ressourceIndicator.SetBananaAmount(StaticMemory.Player.BananasHeld);

        StaticMemory.Player.PropertyChanged += PlayerOnPropertyChanged;
        characterSheet.OnAttributeRaised    += CharacterSheetOnOnAttributeRaised;
        shopPanel.ItemBought                += ShopPanelOnItemBought;
        TreeExiting                         += OnTreeExiting;

        GenerateItems();

        characterSheet.SetDisplayedValues(StaticMemory.Player);
        characterSheet.CharacterImage.Texture = StaticMemory.Player.GetNode<TextureRect>(nameof(TextureRect)).Texture;

        weaponSlotRightHand.SetWeapon(StaticMemory.Player.WieldedWeaponRightHand);

        StaticMemory.AlreadyReadied = true;
    }

    private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(Player.BananasHeld) || sender is not Player player)
            return;

        if (!IsInstanceValid(player) && GetTree().CurrentScene is Shop shop)
        {
            ressourceIndicator = shop.GetNode<CanvasLayer>("UI")
                                     .GetNode<ShopPanel>("%" + nameof(ShopPanel))
                                     .GetNode<RessourceIndicator>("%" + nameof(RessourceIndicator));
        }

        ressourceIndicator.SetBananaAmount(player.BananasHeld);
    }

    private void ShopPanelOnItemBought(BaseItem boughtItem)
    {
        StaticMemory.Player.BananasHeld  -= boughtItem.Price;
        StaticMemory.Player.BananasSpent += boughtItem.Price;

        ressourceIndicator.SetBananaAmount(StaticMemory.Player.BananasHeld);

        inventory.SetItem(boughtItem);

        boughtItem.ApplyEffectTo(StaticMemory.Player);
        characterSheet.SetDisplayedValues(StaticMemory.Player);
    }

    public void _on_button_pressed()
    {
        var itemsFromInventory = inventory.GetAllSlots()
                                          .Where(s => s.ContainedItem is not null)
                                          .Select(s => s.ContainedItem)
                                          .ToList();

        StaticMemory.ItemsHeldByPlayer = itemsFromInventory;
        StaticMemory.AlreadyReadied    = false;

        GetTree().ChangeSceneToPacked(BattleScene);

        QueueFree();
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

    private void OnTreeExiting()
    {
        StaticMemory.Player.PropertyChanged -= PlayerOnPropertyChanged;
        characterSheet.OnAttributeRaised    -= CharacterSheetOnOnAttributeRaised;
        shopPanel.ItemBought                -= ShopPanelOnItemBought;
        TreeExiting                         -= OnTreeExiting;
    }
}