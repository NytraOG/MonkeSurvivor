using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Battle : Node
{
    private double captureTimer;
    private DpsDisplay dpsDisplay;
    [Export] public int DpsCaptureFrameTimeMilliseconds { get; set; } = 1000;
    public float DamageDealtInTimeFrame { get; set; }
    private double totalTime;
    private float totalDamage;
    public int TimeRangeToCaptureInSeconds { get; set; }

    [Export] public PauseMenu PauseMenu { get; set; }

    public override void _Ready()
    {
        base._Ready();

        var orangutan = new Orangutan();
        GetNode<Player>(nameof(Player)).SetMonkeyClass(orangutan);

        dpsDisplay = GetNode<DpsDisplay>("UI/DpsDisplay");
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

            var dps = totalDamage / totalTime;
            dpsDisplay.GetNode<Label>("%DpsValue").Text = dps.ToString("F1");
            
            captureTimer = 0;
            DamageDealtInTimeFrame = 0;
        }

        if (Input.IsKeyPressed(Key.Escape))
        {
            PauseMenu.Visible = true;
            GetTree().Paused = true;
        }
    }
}