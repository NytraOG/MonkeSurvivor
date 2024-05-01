using Godot;
using MonkeSurvivor.Scripts.Interfaces;
using MonkeSurvivor.Scripts.Items;
using Environment = System.Environment;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ItemTooltip : BaseTooltip
{
    private RichTextLabel itemDescriptionLabel;
    private RichTextLabel itemNameLabel;

    public override void _Ready()
    {
        var shopScene = GetTree().CurrentScene;

        SubscribeToShopCards(shopScene);
        SubscribeToInventorySlots(shopScene);

        itemNameLabel        = GetNode<RichTextLabel>("%ItemName");
        itemDescriptionLabel = GetNode<RichTextLabel>("%ItemDescription");
    }

    private void SubscribeToInventorySlots(Node shopScene)
    {
        var inventory      = shopScene.GetNode<Inventory>("%" + nameof(Inventory));
        var inventorySlots = inventory.GetAllSlots();

        foreach (var inventorySlot in inventorySlots) inventorySlot.MouseEntering += InventorySlotOnMouseEntering;
    }

    private void InventorySlotOnMouseEntering(bool entered, InventorySlot inventorySlot)
    {
        if (entered)
        {
            SetDisplayedDataByItem(inventorySlot.ContainedItem);
            SetPositionByNode(inventorySlot);
        }
        else
            ResetTooltip(inventorySlot);
    }

    private void SubscribeToWeaponSlots(Node shopScene)
    {
        var weaponSlotRight = shopScene.GetNode<WeaponSlot>("%WeaponSlotRight");
        var weaponSlotLeft = shopScene.GetNode<WeaponSlot>("%WeaponSlotLeft");
        var weaponSlotHead = shopScene.GetNode<WeaponSlot>("%WeaponSlotHead");
        var weaponSlotTail = shopScene.GetNode<WeaponSlot>("%WeaponSlotTail");
        
        weaponSlotRight.OnMouseEvent -= WeaponSlotRightOnMouseEvent;
        weaponSlotRight.OnMouseEvent += WeaponSlotRightOnMouseEvent;
        
        weaponSlotLeft.OnMouseEvent -= WeaponSlotLeftOnMouseEvent;
        weaponSlotLeft.OnMouseEvent += WeaponSlotLeftOnMouseEvent;
        
        weaponSlotHead.OnMouseEvent -= WeaponSlotHeadOnMouseEvent;
        weaponSlotHead.OnMouseEvent += WeaponSlotHeadOnMouseEvent;
        
        weaponSlotTail.OnMouseEvent -= WeaponSlotTailOnMouseEvent;
        weaponSlotTail.OnMouseEvent += WeaponSlotTailOnMouseEvent;
    }

    private void WeaponSlotTailOnMouseEvent(bool entered, WeaponSlot slot)
    {
        HandleTooltipBehaviour(entered, slot);
    }

    private void WeaponSlotHeadOnMouseEvent(bool entered, WeaponSlot slot)
    {
        HandleTooltipBehaviour(entered, slot);   
    }

    private void WeaponSlotLeftOnMouseEvent(bool entered, WeaponSlot slot)
    {
        HandleTooltipBehaviour(entered, slot);
    }

    private void WeaponSlotRightOnMouseEvent(bool entered, WeaponSlot slot)
    {
        HandleTooltipBehaviour(entered, slot);
    }

    private void HandleTooltipBehaviour(bool entered, WeaponSlot slot)
    {
        if (entered)
        {
            SetDisplayedDataByItem(slot.Weapon);
            SetPositionByNode(slot);
        }
        else
            ResetTooltip(slot);
    }

    private void SubscribeToShopCards(Node shopScene)
    {
        var shopPanel = shopScene.GetNode<ShopPanel>("%" + nameof(ShopPanel));
        var shopCards = shopPanel.GetShopCards();

        foreach (var shopCard in shopCards)
        {
            shopCard.OnMouseEvent -= ShopCardOnOnMouseEvent;
            shopCard.OnMouseEvent += ShopCardOnOnMouseEvent;
        }
    }

    public void SetDisplayedDataByItem(ITooltipConsumable item)
    {
        itemNameLabel.Text = itemNameLabel.Text.Replace("ItemName", $"[u]{item.TooltipName}[/u]");

        itemDescriptionLabel.Text = $"{Environment.NewLine}" +
                                    $"{item.GetTooltipDescription()}{Environment.NewLine}" +
                                    $"{Environment.NewLine}";
    }
    

    private void ShopCardOnOnMouseEvent(bool entered, ShopCard shopCard)
    {
        if (entered && !shopCard.Disabled)
        {
            SetDisplayedDataByItem(shopCard.Item);
            SetPositionByNode(shopCard);
        }
        else
            ResetTooltip(shopCard);
    }

    private void ResetTooltip(PanelContainer container)
    {
        Position = new Vector2(-900, -900);

        itemNameLabel.Text = container switch
        {
            ShopCard { Item: not null } shopCard => itemNameLabel.Text.Replace($"[u]{shopCard.Item.TooltipName}[/u]", "ItemName"),
            InventorySlot { ContainedItem: not null } inventorySlot => itemNameLabel.Text.Replace($"[u]{inventorySlot.ContainedItem.TooltipName}[/u]", "ItemName"),
            _ => itemNameLabel.Text
        };
    }

    public void SetPositionByNode(PanelContainer container)
    {
        var xPosition = container.Position.X - Size.X + 10;

        if (xPosition <= 0) xPosition = container.Position.X + container.Size.X + 20;

        Position = new Vector2(xPosition, 27);
    }
}