using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Gun
{ 
    public void Shoot(); // Make Bullet Obj
}

public interface IWeaPon
{
    public GameObject GetObj();
    public ushort GetMagazine();
    public Weapon_Info GetWeaponInfo();
    public void SetCamObj(GameObject _obj);
    public void Attack(bool _bool);
    public void Reload();
    public void GetWeapon();
    public void Drop();
    public void SetUI(Weapon_Info weapon_);
}

public class Gun_Weapon_Setting : MonoBehaviour, Gun
{
    protected Weapon_Info info;

    [SerializeField] protected GameObject Shoot_Pos;
    protected GameObject CamPos;

    [SerializeField] protected GameObject GunFire_Eff;
    protected ParticleSystem GunFire_Effect;
    [SerializeField] protected GameObject Relode_Eff;
    protected ParticleSystem Relode_Effect;

    protected float RecoilPower; //Vector3.y
    protected float fireRate;
    protected float nextFire;
    protected float time;
    protected float Attack_Range;

    protected Rigidbody rigid;
    protected Collider Coll;
    protected bool DropWeapon;

    protected Hit_param _para;
    
    protected void InitialSetting()
    {
        GunFire_Effect = GunFire_Eff.GetComponent<ParticleSystem>();
        Relode_Effect = Relode_Eff.GetComponent<ParticleSystem>();

        _para.Reset();

        rigid = GetComponent<Rigidbody>();
        Coll = GetComponent<Collider>();
    }

    public GameObject GetObj()
    {
        return gameObject;
    }

    public ushort GetMagazine()
    {
        return info.bulletNum;
    }

    public Weapon_Info GetWeaponInfo()
    {
        return info;
    }

    public void SetCamObj(GameObject _obj)
    {
        CamPos = _obj;
    }

    public virtual void Attack(bool _bool)
    {
        if (_bool == true && info.bulletNum > 0 && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            info.bulletNum--;
            Shoot();
            SetUI(info);
        }
    }

    public virtual void Shoot()
    {
        GunFire_Effect.Play();
        Relode_Effect.Play();
        StartCoroutine(Gunrecoil());
        RaycastHit hit;
        if (Physics.Raycast(CamPos.transform.position, Shoot_Pos.transform.forward, out hit, Attack_Range))
        {
            if(hit.collider.CompareTag("Monster"))
            {
                _para.hit = hit;
                _para.damage = info.damage;
                hit.collider.gameObject.SendMessage("OnDamage", _para, SendMessageOptions.DontRequireReceiver);
                _para.Reset();
            }
            else if(hit.collider.CompareTag("Ground"))
            {
                _para.hit = hit;
                hit.collider.gameObject.SendMessage("Hit", _para, SendMessageOptions.DontRequireReceiver);
                _para.Reset();
            }
        }
        Relode_Effect.Play();
    }

    IEnumerator Gunrecoil()
    {

        yield break;
    }

    public virtual void Reload()
    {
        StartCoroutine(ReloadCo());
    }

    protected IEnumerator ReloadCo()
    {
        yield return new WaitForSeconds(info.reloadTime);
        info.bulletNum = info.original_bulletNum;
        SetUI(info);
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

    public void SetUI(Weapon_Info _info)
    {
        UIManager.Set_Bullet(_info);
    }
}

public class Weapon_Class : MonoBehaviour
{
    private IWeaPon weapon;

    public void SetWeapon(GameObject _weapon_obj, GameObject _cam)
    {
        this.weapon = _weapon_obj.GetComponent<IWeaPon>();
        this.weapon.SetCamObj(_cam);
    }    
    public ushort GetMagazine()
    {
        return weapon.GetMagazine();
    }

    public Weapon_Info GetWeaponInfo()
    {
        return weapon.GetWeaponInfo();
    }

    public void Attack(bool _bool)
    {
        weapon.Attack(_bool);
    }

    public void Reload()
    {
        weapon.Reload();
    }

    public void GetWeapon()
    {
        weapon.GetWeapon();
    }

    public void Drop()
    {
        weapon.Drop();
    }
}
