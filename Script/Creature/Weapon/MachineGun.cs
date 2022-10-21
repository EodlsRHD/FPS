using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Gun_Weapon_Setting, IWeaPon
{
    private bool shot = false;

    void Awake()
    {
        info = new Weapon_Info(this.gameObject.name, 20, 30, 30, 2);
        InitialSetting();

        shot = false;
        time = 0;
        Attack_Range = 200f;
        RecoilPower = 2f;
    }

    public override void Attack(bool _bool)
    {
        shot = _bool;
    }

    void Update()
    {
        if(shot && info.bulletNum > 0)
        {
            time += Time.deltaTime;
            if(time >= 0.15f)
            {
                info.bulletNum--;
                time = 0;
                Shoot();
                SetUI(info);
            }
        }
    }

    public override void Reload()
    {
        shot = false;
        StartCoroutine(ReloadCo());
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawSphere(this.gameObject.transform.position + Shoot_Pos.transform.forward * Attack_Range, Attack_radi);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Shoot_Pos.transform.position, CamPos.transform.position + CamPos.transform.forward * Attack_Range);
    }
}
