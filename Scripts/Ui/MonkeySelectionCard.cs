using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class MonkeySelectionCard : PanelContainer
{
    public delegate void MonkeySelectionClickedEventHandler(MonkeySelectionCard sender);

    [Export]
    public ShaderMaterial ShaderMaterial { get; set; }

    public BaseMonkey ContainedMonkey { get; private set; }

    public event MonkeySelectionClickedEventHandler OnMonkeySelectionClicked;

    public void SetMonkey(BaseMonkey monkey)
    {
        ContainedMonkey = monkey;

        var label = GetNode<Label>("%MonkeyName");
        var textureNode = GetNode<TextureRect>("%MonkeyImage");

        label.Text = monkey.Name;
        textureNode.Texture = monkey.ClassSprite;
    }

    public void _on_mouse_input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton { Pressed: true })
            OnMonkeySelectionClicked?.Invoke(this);
    }

    public void SetShaderHighlightColor(Vector4 color)
    {
        ShaderMaterial.SetShaderParameter("selectionHighlightColor", color);
    }
}