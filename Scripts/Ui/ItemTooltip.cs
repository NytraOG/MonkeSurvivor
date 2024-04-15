using Godot;
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

    public void SetDisplayedDataByItem(BaseItem item)
    {
        itemNameLabel.Text = itemNameLabel.Text.Replace("ItemName", $"[u]{item.Displayname}[/u]");

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
            ShopCard { Item: not null } shopCard => itemNameLabel.Text.Replace($"[u]{shopCard.Item.Displayname}[/u]", "ItemName"),
            InventorySlot { ContainedItem: not null } inventorySlot => itemNameLabel.Text.Replace($"[u]{inventorySlot.ContainedItem.Displayname}[/u]", "ItemName"),
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