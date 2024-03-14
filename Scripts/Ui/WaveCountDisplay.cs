using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WaveCountDisplay : PanelContainer
{
    private Label waveCountDisplayLabel;

    public override void _Ready()
    {
        waveCountDisplayLabel = GetNode<Label>("%CountValue");
        SetWaveCount(1);
    }

    public void SetWaveCount(int value)
    {
        waveCountDisplayLabel.Text = value.ToString();
    }
}