using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopPanel : PanelContainer
{
    public delegate void ItemBoughtEventHandler(BaseItem boughtItem);

    private ShopCard[] cards;

    public event ItemBoughtEventHandler ItemBought;

    public override void _Ready()
    {
        var boxContainer = GetNode<MarginContainer>(nameof(MarginContainer))
            .GetNode<HBoxContainer>(nameof(HBoxContainer));

        cards = boxContainer.GetAllChildren<ShopCard>().ToArray();

        foreach (var shopCard in cards)
        {
            shopCard.ItemBought -= ShopCardOnItemBought;
            shopCard.ItemBought += ShopCardOnItemBought;
        }
    }

    private void ShopCardOnItemBought(BaseItem boughtItem)
    {
        ItemBought?.Invoke(boughtItem);
    }

    public ShopCard[] GetShopCards()
    {
        return cards ?? GetNode<MarginContainer>(nameof(MarginContainer))
            .GetNode<HBoxContainer>(nameof(HBoxContainer)).GetAllChildren<ShopCard>().ToArray();
    }
}