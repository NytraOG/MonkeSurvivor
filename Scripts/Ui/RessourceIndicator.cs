using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class RessourceIndicator : PanelContainer
{
    private Label amountLabel;

    public override void _Ready()
    {
        amountLabel = GetNode<Label>("%Amount");

        SetBananaAmount();
    }

    public void SetBananaAmount(int amount) => amountLabel.Text = ProcessDisplayedValue(amount.ToString());

    public void SetBananaAmount() => amountLabel.Text = ProcessDisplayedValue(StaticMemory.HeldBananas.ToString());

    private static string ProcessDisplayedValue(string newXpValues)
    {
        var result = string.Empty;

        result += newXpValues.Length switch
        {
            1 => "00",
            2 => "0",
            _ => string.Empty
        };

        foreach (var xpValue in newXpValues)
        {
            if (int.TryParse(xpValue.ToString(), out var parsedInteger))
                result += $"{parsedInteger}";
        }

        return result + "x";
    }
}