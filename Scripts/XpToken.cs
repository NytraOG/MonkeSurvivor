using System.Linq;
using Godot;

namespace MonkeSurvivor.Scripts;

public partial class XpToken : StaticBody2D
{
    private Area2D attractArea;
    private Area2D collectionArea;

    [Export]
    public int GrantedXp { get; set; } = 1;

    [Export]
    public float Speed { get; set; } = 10;

    public override void _Ready() { }

    public override void _PhysicsProcess(double delta)
    {
        attractArea    ??= GetNode<Area2D>("AttractArea");
        collectionArea ??= GetNode<Area2D>("CollectionArea");

        var bodiesInAttractionRange = attractArea.GetOverlappingBodies()
                                                 .Where(b => b.Name == nameof(Player))
                                                 .ToList();

        if (bodiesInAttractionRange.Any() && bodiesInAttractionRange[0] is Player player)
        {
            var direction = (player.Position - Position).Normalized();

            MoveAndCollide(direction * Speed);
        }

        var bodiesInCollectionarea = collectionArea.GetOverlappingBodies()
                                                   .Where(b => b.Name == nameof(Player))
                                                   .ToList();

        if (bodiesInCollectionarea.Any() && bodiesInCollectionarea[0] is Player alsoPlayer)
        {
            alsoPlayer.XpCurrent += GrantedXp;

            QueueFree();
        }
    }
}