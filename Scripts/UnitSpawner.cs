using Godot;

namespace MonkeSurvivor.Scripts;

public partial class UnitSpawner : Control
{
    private TextureRect background;
    private Player      player;
    private Node        battleScene;

    [Export]
    public PackedScene UnitToSpawn { get; set; }

    [Export]
    public int AmountPerWave { get; set; }

    [Export]
    public float WaveCooldown { get; set; }

    [Export]
    public float WaveCooldownModifier { get; set; }

    public override void _Ready()
    {
        var sceneTree = GetTree();

        battleScene = sceneTree.CurrentScene;
        player      = battleScene.GetNode<Player>(nameof(Player));
        background  = battleScene.GetNode<TextureRect>("Background");
    }

    public override void _Process(double delta)
    {

    }

    public void SpawnUnit<T>()
            where T : Enemy
    {
        var enemyInstance = UnitToSpawn.Instantiate<T>();
        enemyInstance.SetPosition(new Vector2(player.Position.X, player.Position.Y + (float)1 / 3));
        battleScene.AddChild(enemyInstance);
    }
}