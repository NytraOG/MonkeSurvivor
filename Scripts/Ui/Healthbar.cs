using System.ComponentModel;
using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Healthbar : PanelContainer
{
    private Label              label;
    private Player             player;
    private bool               playerAssigned;
    private bool               playerFound;
    private TextureProgressBar progressBar;

    public override void _Draw() { }

    public override void _Process(double delta)
    {
        if (!playerFound)
        {
            player      = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
            playerFound = player != null;
        }
        else if (playerFound && !playerAssigned)
        {
            progressBar = GetNode<TextureProgressBar>(nameof(TextureProgressBar));
            label       = GetNode<Label>(nameof(Label));

            player.PropertyChanged += PlayerOnPropertyChanged;

            SetHealthbarValue();

            playerAssigned = true;
        }
    }

    private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(BaseUnit.HealthCurrent))
            return;

        SetHealthbarValue();
    }

    private void SetHealthbarValue()
    {
        var healthpercentage = player.HealthCurrent / player.HealthMaximum * 100;
        progressBar.Value = healthpercentage;
        label.Text        = $"{(int)player.HealthCurrent}/{(int)player.HealthMaximum}";
    }
}