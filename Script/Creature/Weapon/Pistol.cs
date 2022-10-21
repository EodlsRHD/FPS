using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun_Weapon_Setting, IWeaPon
{
    void Awake()
    {
        info = new Weapon_Info(this.gameObject.name, 13, 15, 15, 1f);
        InitialSetting();

        Attack_Range = 200f;
        fireRate = 0.4f;
        RecoilPower = 1f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(this.gameObject.transform.position + Shoot_Pos.transform.forward * Attack_Range, 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(Shoot_Pos.transform.position, this.gameObject.transform.position + Shoot_Pos.transform.forward * Attack_Range);
    }
}
