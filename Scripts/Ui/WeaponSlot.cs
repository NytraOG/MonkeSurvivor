using Godot;
using MonkeSurvivor.Scripts.Weapons;

namespace MonkeSurvivor.Scripts.Ui;

public partial class WeaponSlot : PanelContainer
{
    private TextureRect image;
    public  BaseWeapon  Weapon { get; set; }

    public void SetWeapon(BaseWeapon weapon)
    {
        Weapon =   weapon;
        image  ??= GetNode<TextureRect>("%WeaponImage");
        var weaponImage = weapon.GetNode<TextureRect>(nameof(TextureRect));
        image.Texture = weaponImage.Texture;
    }
}