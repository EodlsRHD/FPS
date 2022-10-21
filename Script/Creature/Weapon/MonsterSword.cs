using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSword : MonoBehaviour
{
    Hit_param _para = new Hit_param();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            _para.damage = (ushort)30f;
            other.gameObject.SendMessage("OnDamage", _para, SendMessageOptions.DontRequireReceiver);
            _para.Reset();
        }
    }
}
