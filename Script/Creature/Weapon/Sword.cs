using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeaPon
{
    Weapon_Info weapon_;
    private float time;
    [SerializeField] private GameObject Shoot_Pos;

    public Collider knife;

    private Rigidbody rigid;
    private Collider Coll;
    private bool DropWeapon;

    void Awake()
    {
        weapon_ = new Weapon_Info(this.name, 50, 1, 1, 0.3f);
        rigid = GetComponent<Rigidbody>();
        Coll = GetComponent<Collider>();
        time = 0;
    }

    public GameObject GetObj()
    {
        return this.gameObject;
    }

    public ushort GetMagazine()
    {
        return weapon_.bulletNum;
    }

    public Weapon_Info GetWeaponInfo()
    {
        return weapon_;
    }

    public void Attack(bool _bool)
    {
        if (_bool == true && weapon_.bulletNum > 0)
        {
            weapon_.bulletNum--;
            //ShootPos = Equation(Shoot_Pos.transform.position, Weapon_attackRange, ShootRange);
            //Shoot();
            SetUI(weapon_);
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCo());
    }

    IEnumerator ReloadCo()
    {
        yield return new WaitForSeconds(weapon_.reloadTime);
        weapon_.bulletNum = weapon_.original_bulletNum;
        SetUI(weapon_);
        yield break;
    }

    public void GetWeapon()
    {
        Coll.isTrigger = true;
        rigid.useGravity = false;
        DropWeapon = false;
    }

    public void Drop()
    {
        Coll.isTrigger = false;
        rigid.useGravity = true;
        DropWeapon = true;
    }

    public void SetUI(Weapon_Info _weapon)
    {
        UIManager.Set_Bullet(_weapon);
    }

    public void SetCamObj(GameObject _obj)
    {
        throw new System.NotImplementedException();
    }
}
