using Godot;
using MonkeSurvivor.Scripts.Utils;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WaveCountDisplay : PanelContainer
{
    private Label waveCountDisplayLabel;

    public override void _Ready()
    {
        waveCountDisplayLabel = GetNode<Label>("%CountValue");
        SetWaveCount(StaticMemory.WaveNumber);
    }

    public void SetWaveCount(int value) => waveCountDisplayLabel.Text = value.ToString();
}