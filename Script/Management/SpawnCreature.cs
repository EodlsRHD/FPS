using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCreature : MonoBehaviour
{
    public Transform Player_SpawnPos;
    public List<GameObject> Monster1_SpawnPos;

    private const ushort Monster_MaxCount = 30;
    public static ushort MonsterCount;

    private void Awake()
    {
        PlayerSpawn();
        StartCoroutine(MonsterSpawn());
    }

    private void PlayerSpawn()
    {
        var obj = Poolmanager.GetPlayer();
        obj.transform.position = Player_SpawnPos.position + new Vector3(0, 10, 0);
    }

    IEnumerator MonsterSpawn()
    {
        while(true)
        {
            if(MonsterCount < Monster_MaxCount)
            {
                var obj = Poolmanager.GetMonster();
                obj.gameObject.transform.position += Monster1_SpawnPos[Random.Range(0, Monster1_SpawnPos.Count)].transform.position;
                obj.gameObject.SetActive(true);
                obj.gameObject.GetComponent<Monster>().SpawnMonster();
                MonsterCount++;
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
