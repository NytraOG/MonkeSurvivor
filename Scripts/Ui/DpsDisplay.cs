using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class DpsDisplay : PanelContainer
{
    private double captureTimer;
    private float totalDamage;
    private double totalTime;
    [Export] public int DpsCaptureFrameTimeMilliseconds { get; set; } = 1000;
    public float DamageDealtInTimeFrame { get; set; }
    public int TimeRangeToCaptureInSeconds { get; set; }

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        if ((captureTimer += delta) >= (float)DpsCaptureFrameTimeMilliseconds / 1000)
        {
            if (captureTimer == 0)
                return;

            totalDamage += DamageDealtInTimeFrame;
            totalTime += captureTimer;

            if (totalTime >= TimeRangeToCaptureInSeconds)
            {
            }

            var dps = DamageDealtInTimeFrame / captureTimer;
            GetNode<Label>("%DpsValue").Text = dps.ToString("F1");

            captureTimer = 0;
            DamageDealtInTimeFrame = 0;
        }
    }
}