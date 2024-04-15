using Godot;
using MonkeSurvivor.Scripts.Monkeys;

namespace MonkeSurvivor.Scripts.Ui;

public partial class MonkeySelectionCard : PanelContainer
{
    public delegate void MonkeySelectionClickedEventHandler(MonkeySelectionCard sender);

    [Export] public Shader SelectionHighlightShader { get; set; }

    public BaseMonkey Monkey { get; private set; }

    public event MonkeySelectionClickedEventHandler OnMonkeySelectionClicked;

    public void SetMonkey(BaseMonkey monkey)
    {
        Monkey = monkey;

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

    public void SetWhiteShaderHighlightColor()
    {
        SetShaderHighlightColor(new Vector4(1, 1, 1, 0.1f));
    }

    public void RemoveShader()
    {
        if (Material is not ShaderMaterial shaderMaterial)
            return;

        shaderMaterial.Shader = null;
    }

    public void SetShaderHighlightColor(Vector4 color)
    {
        if (Material is not ShaderMaterial shaderMaterial)
            return;

        shaderMaterial.Shader = SelectionHighlightShader;
        shaderMaterial.SetShaderParameter("selectionHighlightColor", color);
    }
}