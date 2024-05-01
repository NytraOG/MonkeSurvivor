using Godot;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WeaponSlot : PanelContainer
{
    public delegate void MouseEventHandler(bool entered, WeaponSlot slot);

    public event MouseEventHandler OnMouseEvent;
    private TextureRect image;
    public  BaseWeapon  Weapon { get; set; }

    public void SetWeapon(BaseWeapon weapon)
    {
        Weapon =   weapon;
        image  ??= GetNode<TextureRect>("%WeaponImage");
        var weaponImage = weapon.GetNode<TextureRect>("%" + nameof(TextureRect));
        image.Texture = weaponImage.Texture;
    }

    public void _on_mouse_entered_weapon_slot() => OnMouseEvent?.Invoke(true, this);
    public void _on_mouse_exited_weapon_slot() => OnMouseEvent?.Invoke(false, this);
}