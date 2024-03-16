using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class XpDisplay : PanelContainer
{
    private Label xpDisplayValue;

    public override void _Ready()
    {
        xpDisplayValue      = GetNode<Label>("%XpValue");
        xpDisplayValue.Text = "000";

        GetTree()
               .CurrentScene
               .GetNode<Player>(nameof(Player))
               .PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(Player.XpCurrent) && sender is Player player)
            {
                var result      = string.Empty;
                var newXpValues = player.XpCurrent.ToString().ToCharArray();

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
        };
    }
}