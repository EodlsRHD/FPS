using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pool_Order
{
    public ushort type;
    public string name;

    public Pool_Order(ushort _type, string _name)
    {
        type = _type;
        name = _name;
    }
}

public struct Weapon_Info
{
    public string name;
    public ushort damage;
    public ushort original_bulletNum;
    public ushort bulletNum;
    public float reloadTime;

    public Weapon_Info(string _name, ushort _damage, ushort _original_bulletNum, ushort _bulletNum, float _reloadTime)
    {
        name = _name;
        damage = _damage;
        original_bulletNum = _original_bulletNum;
        bulletNum = _bulletNum;
        reloadTime = _reloadTime;
    }
}

public struct Cheature_Info
{
    public ushort damage;
    public short hp;
    public float movespeed;
    public float turnspeed;
    public float jumppower;

    public Cheature_Info(ushort _damage, short _hp, float _movespeed, float _turnspeed, float _jumppower)
    {
        damage = _damage;
        hp = _hp;
        movespeed = _movespeed;
        turnspeed = _turnspeed;
        jumppower = _jumppower;
    }
}

public struct Hit_param
{
    public RaycastHit hit;
    public ushort damage;

    public Hit_param(RaycastHit _hit, ushort _damage)
    {
        hit = _hit;
        damage = _damage;
    }

    public void Reset()
    {
        hit = new RaycastHit();
        damage = 0;
    }
}
