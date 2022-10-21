using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private ParticleSystem system;
    private string type;
    public void Set(string _type)
    {
        system = GetComponent<ParticleSystem>();
        type = _type;
        system.Play();
        if(type.Contains("MonsterBlood"))
        {
            Invoke("Destroy", 5f);
        }
        else
        {
            Invoke("Destroy", 3f);
        }
    }

    void Destroy()
    {
        if(type.Contains("Metal"))
        {
            Poolmanager.RetuenObject(0, this.gameObject);
        }
        else if (type.Contains("Send"))
        {
            Poolmanager.RetuenObject(5, this.gameObject);
        }
        else if (type.Contains("Hit"))
        {
            Poolmanager.RetuenObject(1, this.gameObject);
        }
        else if (type.Contains("Stone"))
        {
            Poolmanager.RetuenObject(6, this.gameObject);
        }
        else if(type.Contains("MonsterBlood"))
        {
            Poolmanager.RetuenObject(7, this.gameObject);
        }
    }
}
