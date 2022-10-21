using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    void ChangeWeapon(string _name);
    void Move();
    void View();
}

public interface IMonster
{
    void SpawnMonster();
    IEnumerator ChackMonsterHavior();
    IEnumerator MonsterAction();
    bool RandomPoint(float _range, out Vector3 _param);
}

public abstract class Creature_ab : MonoBehaviour
{
    protected Cheature_Info Info;
    protected Hit_param _para;

    protected abstract void GetDamage(ushort _damage);

    protected abstract void OnDamage(Hit_param _param);

    protected abstract void Die();
}
