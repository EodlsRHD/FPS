using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTarget : MonoBehaviour
{
    private Cheature_Info cheature_Info;

    private void Awake()
    {
        cheature_Info = new Cheature_Info(0,1000,0,0,0);
    }

    private void TargetDie()
    {
        //GameOver
        Debug.Log("GameOver");
    }

    private void GetDamage(ushort _damage)
    {
        cheature_Info.hp -= (short)_damage;
        if (cheature_Info.hp <= 0)
        {
            TargetDie();
        }
    }

    void OnDamage(Hit_param _param)
    {
        GameObject Hiteffect = Poolmanager.GetMonsterEffect("Hit");
        GameObject Bloodeffect = Poolmanager.GetMonsterEffect("MonsterBlood");
        Hiteffect.transform.position = _param.hit.point;
        Bloodeffect.transform.position = _param.hit.point;
        Hiteffect.transform.rotation = Quaternion.LookRotation(_param.hit.normal);
        Bloodeffect.transform.rotation = Quaternion.LookRotation(_param.hit.normal);
        Hiteffect.transform.SetParent(_param.hit.collider.transform);
        Bloodeffect.transform.SetParent(_param.hit.collider.transform);
        Hiteffect.SetActive(true);
        Bloodeffect.SetActive(true);
        Hiteffect.GetComponent<HitEffect>().Set("Hit");
        Bloodeffect.GetComponent<HitEffect>().Set("MonsterBlood");
        GetDamage(_param.damage);
    }
}
