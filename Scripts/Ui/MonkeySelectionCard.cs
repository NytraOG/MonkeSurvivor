using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class MonkeySelectionCard : PanelContainer
{
    public BaseMonkey ContainedMonkey { get; private set; }

    public void SetMonkey(BaseMonkey monkey)
    {
        ContainedMonkey = monkey;

        var label = GetNode<Label>("%MonkeyName");
        var textureNode = GetNode<TextureRect>("%MonkeyImage");

        label.Text = monkey.Name;
        textureNode.Texture = monkey.ClassSprite;
    }
}