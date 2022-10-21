using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun_Weapon_Setting, IWeaPon
{
    private float Attack_radi;
    private Vector3[] Impactpoint;

    void Awake()
    {
        info = new Weapon_Info(this.gameObject.name, 40, 8, 8, 4f);
        InitialSetting();

        Attack_Range = 200f;
        Attack_radi = 13f;
        Impactpoint = new Vector3[8];
        fireRate = 0.8f;
        RecoilPower = 5f;
    }

    public override void Attack(bool _bool)
    {
        if (_bool == true && info.bulletNum > 0 && Time.time > nextFire)
        {
            Equation(Shoot_Pos.transform, Attack_radi, Attack_Range);
            nextFire = Time.time + fireRate;
            info.bulletNum--;
            Shoot();
            SetUI(info);
        }
    }

    public override void Shoot()
    {
        GunFire_Effect.Play();
        Relode_Effect.Play();
        for (int i = 0; i < Impactpoint.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(CamPos.transform.position, Impactpoint[i], out hit, Attack_Range))
            {
                if (hit.collider.CompareTag("Monster"))
                {
                    _para.hit = hit;
                    _para.damage = (ushort)((info.damage / 8) + 2);
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
        for(int i = 0; i < 8; i++)
        {
            Vector3 random = Random.insideUnitSphere * _radius;
            Vector3 tmp3 = _shooterPos.transform.forward * _attackRange;

            Impactpoint[i] = tmp3 + random;
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawSphere(this.gameObject.transform.position + Shoot_Pos.transform.forward * Attack_Range, Attack_radi);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(CamPos.transform.position, CamPos.transform.position + CamPos.transform.forward * Attack_Range);
    }
}
