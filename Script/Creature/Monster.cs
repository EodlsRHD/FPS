using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : Creature_ab, IMonster
{
    public enum MonsterHavior{ idle, trace, GunAttack , SwordAttack, die }
    protected MonsterHavior havior;
    private Animator ani;
    private Collider coll;

    public const float m_Range = 100f;
    public const float SwordAt_Range = 15f;
    public const float GunAt_Range = 100f;
    public const float Co_Range = 150f;
    protected float SwordAttackRate;
    protected float SwordNextAttack;
    [SerializeField] private MonsterShotGun gun;
    [SerializeField] private MonsterSword sword;
    
    private NavMeshAgent m_Agent;
    private Transform monster_Tr;
    private Transform target_Tr;
    

    private float FirenextHit;
    private float FirehitRate;

    private Vector3 destination;

    private bool isDie = false;

    public void SpawnMonster()
    {
        Info = new Cheature_Info(20, 70, 10, 15, 450);
        ani = this.gameObject.GetComponent<Animator>();
        coll = this.gameObject.GetComponent<Collider>();
        SwordAttackRate = 1f;
        FirehitRate = 1f;
        havior = MonsterHavior.idle;

        m_Agent = this.gameObject.GetComponent<NavMeshAgent>();
        m_Agent.enabled = true;
        coll.enabled = true;
        m_Agent.speed = Info.movespeed;

        monster_Tr = this.gameObject.GetComponent<Transform>();
        target_Tr = GameObject.FindWithTag("Player").GetComponent<Transform>();

        destination = monster_Tr.position;
        _para.Reset();

        StartCoroutine(ChackMonsterHavior());
        StartCoroutine(MonsterAction());
    }

    public IEnumerator ChackMonsterHavior()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            float target_dis = Vector3.Distance(target_Tr.position, monster_Tr.position);

            if (target_dis <= SwordAt_Range)
            {
                havior = MonsterHavior.SwordAttack;
            }
            //if (target_dis <= GunAt_Range)
            //{
            //    havior = MonsterHavior.GunAttack;
            //}
            else if (target_dis <= Co_Range)
            {
                havior = MonsterHavior.trace;
            }
            else
            {
                havior = MonsterHavior.idle;
            }
        }
    }

    public IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch(havior)
            {
                case MonsterHavior.idle:
                    ani.SetTrigger("Walk");
                    float Dis = Vector3.Distance(destination, monster_Tr.position);
                    if (Dis <= 30f)
                    {
                        if (RandomPoint(m_Range, out destination)) { m_Agent.destination = destination; }
                    }
                    break;
                case MonsterHavior.trace:
                    ani.SetTrigger("Walk");
                    m_Agent.destination = target_Tr.position;
                    break;
                case MonsterHavior.SwordAttack:
                    m_Agent.destination = this.gameObject.transform.position;
                    this.transform.LookAt(target_Tr);
                    ani.SetTrigger("SwordAttack");
                    break;
                case MonsterHavior.GunAttack:
                    m_Agent.destination = this.gameObject.transform.position;
                    this.transform.LookAt(target_Tr);
                    ani.SetTrigger("GunAttack");
                    break;
                case MonsterHavior.die:
                    ani.SetTrigger("Death");
                    StopAllCoroutines();
                    break;
            }
            yield return null;
        }
    }

    public bool RandomPoint(float range, out Vector3 result)
    {
        Vector3 randomPoint = monster_Tr.position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }

    protected override void OnDamage(Hit_param _param)
    {
        GameObject Bloodeffect = Poolmanager.GetMonsterEffect("MonsterBlood");
        Bloodeffect.transform.position = _param.hit.point;
        Bloodeffect.transform.rotation = Quaternion.LookRotation(_param.hit.normal);
        Bloodeffect.transform.SetParent(_param.hit.collider.transform);
        Bloodeffect.SetActive(true);
        Bloodeffect.GetComponent<HitEffect>().Set("MonsterBlood");

        GameObject hiteffect = Poolmanager.GetMonsterEffect("Hit");
        hiteffect.transform.position = _param.hit.point;
        hiteffect.transform.rotation = Quaternion.LookRotation(_param.hit.normal);
        hiteffect.transform.SetParent(_param.hit.collider.transform);
        hiteffect.SetActive(true);
        hiteffect.GetComponent<HitEffect>().Set("Hit");

        GetDamage(_param.damage);
    }

    void OnTriggerStay(Collider _coll)
    {
        if (_coll.CompareTag("Fire"))
        {
            if (Time.time > FirenextHit)
            {
                FirenextHit = Time.time + FirehitRate;
                GetDamage(3);
            }
        }
    }

    protected override void GetDamage(ushort _damage)
    {
        Info.hp -= (short)_damage;
        if (Info.hp <= 0)
        {
            havior = MonsterHavior.die;
            SpawnCreature.MonsterCount--;
            m_Agent.enabled = false;
            coll.enabled = false;
            Invoke("Die", 3f);
        }
    }

    protected override void Die()
    {
        Poolmanager.RetuenObject(2, this.gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + this.gameObject.transform.forward * 15f);
    }
}
