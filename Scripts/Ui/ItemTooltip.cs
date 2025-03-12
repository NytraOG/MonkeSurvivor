using Godot;
using Godot.Interfaces;
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
        SubscribeToWeaponSlots(shopScene);

        itemNameLabel        = GetNode<RichTextLabel>("%ItemName");
        itemDescriptionLabel = GetNode<RichTextLabel>("%ItemDescription");
    }

    private void SubscribeToInventorySlots(Node shopScene)
    {
        var inventory      = shopScene.GetNode<Inventory>("%" + nameof(Inventory));
        var inventorySlots = inventory.GetAllSlots();

        foreach (var inventorySlot in inventorySlots) inventorySlot.MouseEntering += InventorySlotOnMouseEntering;
    }

    private void InventorySlotOnMouseEntering(bool entered, ITooltipObjectContainer inventorySlot)
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
        var weaponSlotRight = shopScene.GetNode<WeaponSlot>("%WeaponSlotRightHand");
        var weaponSlotLeft  = shopScene.GetNode<WeaponSlot>("%WeaponSlotLeftHand");
        var weaponSlotHead  = shopScene.GetNode<WeaponSlot>("%WeaponSlotHead");
        var weaponSlotTail  = shopScene.GetNode<WeaponSlot>("%WeaponSlotTail");

        weaponSlotRight.OnMouseEvent += WeaponSlotRightOnMouseEvent;
        weaponSlotLeft.OnMouseEvent  += WeaponSlotLeftOnMouseEvent;
        weaponSlotHead.OnMouseEvent  += WeaponSlotHeadOnMouseEvent;
        weaponSlotTail.OnMouseEvent  += WeaponSlotTailOnMouseEvent;
    }

    private void WeaponSlotTailOnMouseEvent(bool entered, WeaponSlot slot) => HandleTooltipBehaviour(entered, slot);

    private void WeaponSlotHeadOnMouseEvent(bool entered, WeaponSlot slot) => HandleTooltipBehaviour(entered, slot);

    private void WeaponSlotLeftOnMouseEvent(bool entered, WeaponSlot slot) => HandleTooltipBehaviour(entered, slot);

    private void WeaponSlotRightOnMouseEvent(bool entered, WeaponSlot slot) => HandleTooltipBehaviour(entered, slot);

    private void HandleTooltipBehaviour(bool entered, WeaponSlot slot)
    {
        if (slot.ContainedItem is null)
            return;

        if (entered)
        {
            SetDisplayedDataByItem(slot.ContainedItem);
            Position = new Vector2(845, 426);
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

    public void SetDisplayedDataByItem(ITooltipObject item)
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
            SetDisplayedDataByItem(shopCard.ContainedItem);
            SetPositionByNode(shopCard);
        }
        else
            ResetTooltip(shopCard);
    }

    private void ResetTooltip(ITooltipObjectContainer container)
    {
        Position = new Vector2(-900, -900);

        itemNameLabel.Text = container switch
        {
            ShopCard { ContainedItem: not null } shopCard => itemNameLabel.Text.Replace($"[u]{shopCard.ContainedItem.TooltipName}[/u]", "ItemName"),
            InventorySlot { ContainedItem: not null } inventorySlot => itemNameLabel.Text.Replace($"[u]{inventorySlot.ContainedItem.TooltipName}[/u]", "ItemName"),
            WeaponSlot { ContainedItem: not null } weaponSlot => itemNameLabel.Text.Replace($"[u]{weaponSlot.ContainedItem.TooltipName}[/u]", "ItemName"),
            _ => itemNameLabel.Text
        };
    }

    public void SetPositionByNode(ITooltipObjectContainer container)
    {
        var xPosition = container.Position.X - Size.X + 10;

        if (xPosition <= 0) xPosition = container.Position.X + container.Size.X + 20;

        Position = new Vector2(xPosition, 27);
    }
}