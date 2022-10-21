using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager uimanager;
    [SerializeField] private TextMeshProUGUI Original_Bullet;
    [SerializeField] private TextMeshProUGUI Bullet;
    [SerializeField] private TextMeshProUGUI Weapon_Name;

    [SerializeField] private Image HP_frontimage;
    [SerializeField] private TextMeshProUGUI MaxHP;
    [SerializeField] private TextMeshProUGUI HP;
    private short maxhp;

    private void Awake()
    {
        uimanager = this;
        maxhp = 0;
    }

    public static void Set_Bullet(Weapon_Info _weaponinfo)
    {
        uimanager.Bullet.text = _weaponinfo.bulletNum.ToString();
    }

    public static void SwapWeapon(Weapon_Info _weaponinfo)
    {
        uimanager.Original_Bullet.text = _weaponinfo.original_bulletNum.ToString();
        uimanager.Bullet.text = _weaponinfo.bulletNum.ToString();
        _weaponinfo.name = _weaponinfo.name.Replace("(Clone)", "");
        uimanager.Weapon_Name.text = _weaponinfo.name;
    }

    public static void Set_HP(short _hp)
    {
        if(uimanager.maxhp < _hp)
        {
            uimanager.maxhp = _hp;
            uimanager.MaxHP.text = uimanager.maxhp.ToString();
            uimanager.HP.text = _hp.ToString();
        }

        if(uimanager.maxhp > _hp)
        {
            float result = (_hp % uimanager.maxhp);
            uimanager.HP_frontimage.fillAmount = result * 0.01f;
            uimanager.HP.text = result.ToString();
        }
    }
}
