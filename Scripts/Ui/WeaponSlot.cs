using Godot;
using Godot.Interfaces;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WeaponSlot : PanelContainer,
                                  ITooltipObjectContainer
{
    public delegate void MouseEventHandler(bool entered, WeaponSlot slot);

    private TextureRect image;

    //public  BaseWeapon             Weapon        { get; set; }
    public ITooltipObject?          ContainedItem { get; set; }
    public event MouseEventHandler OnMouseEvent;

    public void SetWeapon(BaseWeapon weapon)
    {
        ContainedItem =   weapon;
        image         ??= GetNode<TextureRect>("%WeaponImage");
        var weaponImage = weapon.GetNode<TextureRect>("%" + nameof(TextureRect));
        image.Texture = weaponImage.Texture;
    }

    public void _on_mouse_entered_weapon_slot() => OnMouseEvent?.Invoke(true, this);

    public void _on_mouse_exited_weapon_slot() => OnMouseEvent?.Invoke(false, this);
}