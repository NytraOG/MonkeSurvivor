using Godot;

namespace MonkeSurvivor.Scripts.Utils;

public partial class InventoryController : Node
{
	private Player player;

	public override void _Ready()
	{
		player = GetTree().CurrentScene.GetNode<Player>(nameof(Player));
		
		foreach (var item in StaticMemory.ItemsHeldByPlayer)
		{
			item?.ApplyEffectTo(player);
		}
	}

	public override void _ExitTree()
	{
		foreach (var item in StaticMemory.ItemsHeldByPlayer)
		{
			item?.DeductEffectFrom(player);
		}
	}
}