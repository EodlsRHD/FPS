using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float Movespeed;
    public ushort damage;
    public void Shoot(Vector3 _direction, ushort _Damage)
    {
        Movespeed = 0.5f;
        damage = _Damage;
        this.direction = _direction;
        Invoke("DestroyBullet", 4f);
    }

    public void DestroyBullet()
    {
        Poolmanager.RetuenObject(1, this.gameObject);
    }

    void Update()
    {
        transform.Translate(direction * Movespeed);
        this.transform.localRotation = Quaternion.Euler(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log("Destroy");
            DestroyBullet();
        }
    }
}
