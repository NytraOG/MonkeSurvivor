using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Deathscreen : PanelContainer
{
	private Player player;
	private PackedScene ResetScene => ResourceLoader.Load<PackedScene>("res://Scenes/Ui/monkey_selection.tscn");
	public override void _Process(double delta)
	{
		player ??= GetTree().CurrentScene.GetNode<Player>(nameof(Player));
		
		if (player.IsDead && Input.IsAnythingPressed())
			ResetPlaythrough();
	}

	private void ResetPlaythrough()
	{
		var tree = GetTree();
		tree.ChangeSceneToPacked(ResetScene);
		tree.Paused = false;
	}
}