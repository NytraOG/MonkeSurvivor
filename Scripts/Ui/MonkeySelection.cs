using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MonkeSurvivor.Scripts.Items;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class MonkeySelection : Node
{
	private readonly List<string>   monkeyScenepaths = new();
	private HBoxContainer cardContainer;

	public override void _Ready()
	{
		cardContainer = GetNode<HBoxContainer>(nameof(HBoxContainer));
		
		AssembleItemScenes();
		GenerateMonkeySelectionCards();
	}

	private void GenerateMonkeySelectionCards()
	{
		var selectionCardScene = ResourceLoader.Load<PackedScene>("res://Scenes/Ui/monkey_selection_card.tscn");
		
		foreach (var monkeyScenePath in monkeyScenepaths)
		{
			var scene     = ResourceLoader.Load<PackedScene>(monkeyScenePath);
			var selectionCard = selectionCardScene.Instantiate<MonkeySelectionCard>();
			var monkeyInstance = scene.Instantiate<BaseMonkey>();

			selectionCard.OnMonkeySelectionClicked += SelectionCardOnMonkeySelectionClicked;
			selectionCard.SetMonkey(monkeyInstance);
			
			cardContainer.AddChild(selectionCard);
		}
	}

	private void SelectionCardOnMonkeySelectionClicked(MonkeySelectionCard sender)
	{
		sender.Material.
	}

	private void AssembleItemScenes()
	{
		var monkeyDirectory = "res://Scenes/Classes/";

		using var directory = DirAccess.Open(monkeyDirectory);

		if (directory is not null)
		{
			directory.ListDirBegin();
			var fileName = directory.GetNext();

			while (!string.IsNullOrWhiteSpace(fileName))
			{
				if (directory.CurrentIsDir())
					return;

				if (monkeyScenepaths.All(s => !s.Contains(fileName)))
					monkeyScenepaths.Add(monkeyDirectory + fileName);

				fileName = directory.GetNext();
			}
		}
		else
			GD.Print($"An error occurred when trying to access the path. '{monkeyDirectory}'");
	}
}