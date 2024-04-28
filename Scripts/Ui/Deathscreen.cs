using Godot;

namespace MonkeSurvivor.Scripts.Ui;

public partial class Deathscreen : PanelContainer
{
	private PackedScene CharacterSeletionScene => ResourceLoader.Load<PackedScene>("res://Scenes/Ui/monkey_selection.tscn");
	public override void _Process(double delta)
	{
		if (Input.IsAnythingPressed())
			ResetPlaythrough();
	}

	private void ResetPlaythrough()
	{
		var tree = GetTree();
		tree.ChangeSceneToPacked(CharacterSeletionScene);
		tree.Paused = false;
	}
}