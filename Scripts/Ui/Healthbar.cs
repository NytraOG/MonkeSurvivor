using System.ComponentModel;
using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Healthbar : PanelContainer
{
    private Label              label;
    private TextureProgressBar progressBar;

    [Export]
    public Player Player { get; set; }

    public override void _Ready()
    {
        progressBar = GetNode<TextureProgressBar>(nameof(TextureProgressBar));
        label       = GetNode<Label>(nameof(Label));

        Player.PropertyChanged += PlayerOnPropertyChanged;

        SetHealthbarValue();
    }

    private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(BaseUnit.HealthCurrent))
            return;

        SetHealthbarValue();
    }

    private void SetHealthbarValue()
    {
        var healthpercentage = Player.HealthCurrent / Player.HealthMaximum * 100;
        progressBar.Value = healthpercentage;
        label.Text        = $"{(int)Player.HealthCurrent}/{(int)Player.HealthMaximum}";
    }
}