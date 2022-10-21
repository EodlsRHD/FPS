using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterShotGun : Gun_Weapon_Setting, IWeaPon
{
    static MonsterShotGun mgun;
    private float Attack_radi;
    private Vector3[] Impactpoint;

    void Awake()
    {
        mgun = this;
        info = new Weapon_Info("MonsterShotGun", 30, 2, 2, 6f);
        InitialSetting();

        Attack_Range = 100f;
        Attack_radi = 20f;
        Impactpoint = new Vector3[8];
        fireRate = 3f;
        RecoilPower = 0f;
    }

    public static void ani_Attack()
    {
        mgun.Attack(true);
    }

    public override void Attack(bool _bool)
    {
        if (_bool == true && info.bulletNum > 0 && Time.time > nextFire)
        {
            Equation(Shoot_Pos.transform, Attack_radi, Attack_Range);
            nextFire = Time.time + fireRate;
            info.bulletNum--;
            Shoot();
        }
        else if(info.bulletNum == 0)
        {
            Reload();
        }
    }

    public override void Shoot()
    {
        GunFire_Effect.Play();
        Relode_Effect.Play();
        for (int i = 0; i < Impactpoint.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(Shoot_Pos.transform.position, Impactpoint[i], out hit, Attack_Range))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _para.hit = hit;
                    _para.damage = (ushort)((info.damage / 8));
                    hit.collider.gameObject.SendMessage("OnDamage", _para, SendMessageOptions.DontRequireReceiver);
                    _para.Reset();
                }
                else if (hit.collider.CompareTag("Ground"))
                {
                    _para.hit = hit;
                    hit.collider.gameObject.SendMessage("Hit", _para, SendMessageOptions.DontRequireReceiver);
                    _para.Reset();
                }
            }
        }
    }

    private void Equation(Transform _shooterPos, float _radius, float _attackRange)
    {
        for (int i = 0; i < 8; i++)
        {
            Vector3 random = Random.insideUnitSphere * _radius;
            Vector3 tmp3 = _shooterPos.transform.forward * _attackRange;

            Impactpoint[i] = tmp3 + random;
        }
    }
}
