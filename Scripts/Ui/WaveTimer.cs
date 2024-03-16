using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WaveTimer : PanelContainer
{
    public delegate void WaveEndedEventHandler();

    private Label  label;
    private double waveTimer;

    [Export]
    public int WaveTimeSeconds { get; set; }

    public event WaveEndedEventHandler OnWaveEnded;

    public override void _Ready()
        => label = GetNode<MarginContainer>(nameof(MarginContainer)).GetNode<Label>(nameof(Label));

    public override void _Process(double delta)
    {
        label.Text = $"{WaveTimeSeconds - (int)waveTimer}";

        if ((waveTimer += delta) >= WaveTimeSeconds + 1)
        {
            OnWaveEnded?.Invoke();

            Visible     = false;
            OnWaveEnded = null;
        }
    }
}