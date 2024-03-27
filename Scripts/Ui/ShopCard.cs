using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class ShopCard : PanelContainer
{
    public delegate void MouseEventHandler(bool entered, ShopCard shopCard);

    private Label itemCostLabel;
    private Label itemDescriptionLabel;
    private Label itemNameLabel;

    [Export]
    public int CardId { get; set; }

    public bool                    Disabled { get; set; }
    public event MouseEventHandler OnMouseEvent;

    public override void _Ready()
    {
        itemNameLabel        = GetNode<Label>("%ItemName");
        itemDescriptionLabel = GetNode<Label>("%ItemDescription");
        itemCostLabel        = GetNode<Label>("%ItemCost");
    }

    public override void _Process(double delta) { }

    public void _on_buy_pressed()
    {
        Modulate = new Color(Modulate, 0);
        Disabled = true;

        _on_mouse_exited_shopCard();
    }

    public void _on_mouse_entered_shopCard() => OnMouseEvent?.Invoke(true, this);

    public void _on_mouse_exited_shopCard() => OnMouseEvent?.Invoke(false, this);
}