using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class XpDisplay : PanelContainer
{
    private Player player;
    private bool   playerAssigned;
    private bool   playerFound;
    private Label  xpDisplayValue;

    public override void _Process(double delta)
    {
        if (!playerFound)
        {
            player      = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
            playerFound = player != null;
        }
        else if (playerFound && !playerAssigned)
        {
            xpDisplayValue = GetNode<Label>("%XpValue");
            SetDisplayedValue(player);

            player.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Player.BananasHeld) && sender is Player alsoPlayer)
                    SetDisplayedValue(alsoPlayer);
            };

            playerAssigned = true;
        }
    }

    private void SetDisplayedValue(Player player)
    {
        var result      = string.Empty;
        var newXpValues = player.BananasHeld.ToString().ToCharArray();

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

        xpDisplayValue.Text = result;
    }
}