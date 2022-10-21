using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Creature_ab, IPlayer
{
    [SerializeField] private Transform PlayerBody;
    [SerializeField] private Transform camArm;
    [SerializeField] private GameObject cam;
    [SerializeField] private Image HitEffect;
    private CharacterController controller;
    private Vector3 moveDir;

    private float HitEffect_ObjOffTime;
    private float HitEffectObjOffTime;
    private float FirenextHit;
    private float FirehitRate;
    private float FireDuration;
    private float FireEndDuration;
    private bool isFire = false;

    private Weapon_Class weapon_;
    private GameObject weapon_obj;
    [SerializeField] private Transform Weapon_Pos;

    void Start()
    {
        Info = new Cheature_Info(0, 100,
                    GameManager.instance.GetMoveSpeed(),
                    GameManager.instance.GetTurnSpeed(),
                    GameManager.instance.GetJumpPower());
        FirehitRate = 1f;
        FireDuration = 3f;
        HitEffect_ObjOffTime = 0.2f;
        controller = this.gameObject.GetComponent<CharacterController>();
        weapon_ = new Weapon_Class();
        UIManager.Set_HP(Info.hp);
    }

    void Update()
    {
        ControllWeapon();
        ChangeWeapon();
        Move();
    }

    void LateUpdate()
    {
        View();
    }

    void ControllWeapon()
    {
        if (weapon_obj != null)
        {
            weapon_obj.transform.position = Weapon_Pos.position;
            if (Input.GetMouseButtonDown(0))
            {
                if (weapon_.GetMagazine() != 0) { weapon_.Attack(true); }
                else if (weapon_.GetMagazine() == 0) { weapon_.Reload(); }
            }

            if(Input.GetMouseButtonUp(0))
            {
                if (weapon_.GetMagazine() != 0) { weapon_.Attack(false); }
            }

            if (Input.GetKeyDown(KeyCode.R) && !Input.GetMouseButton(0))
            {
                weapon_.Reload();
            }
        }
    }

    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon("pistol");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon("assault");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon("shotgun");
        }
    }

    public void ChangeWeapon(string _name)
    {
        if (weapon_obj != null)
        {
            Poolmanager.RetuenObject(3, weapon_obj);
        }
        weapon_obj = Poolmanager.GetWeapon(_name);
        weapon_obj.transform.SetParent(Weapon_Pos);
        weapon_.SetWeapon(weapon_obj, cam.gameObject);
        UIManager.SwapWeapon(weapon_.GetWeaponInfo());
    }

    public void View()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X") * Info.turnspeed, Input.GetAxis("Mouse Y") * Info.turnspeed);
        Vector3 CamAngle = camArm.rotation.eulerAngles;
        float x = CamAngle.x - mouseDelta.y;
        float y = CamAngle.y + mouseDelta.x;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 45f);
        }
        if (x > 180f)
        {
            x = Mathf.Clamp(x, 320f, 361f);
        }

        camArm.rotation = Quaternion.Euler(x, y, CamAngle.z);
        PlayerBody.rotation = new Quaternion(0, camArm.rotation.y, 0, camArm.rotation.w);
        if (weapon_obj != null)
        {
            weapon_obj.transform.forward = camArm.forward;
        }
    }

    public void Move()
    {
        if (controller.isGrounded)
        {
            Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 lookForward = new Vector3(camArm.forward.x, 0f, camArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(camArm.right.x, 0f, camArm.right.z).normalized;
            var run = Input.GetKeyDown(KeyCode.LeftShift);

            moveDir = (lookForward * moveInput.y + lookRight * moveInput.x).normalized;
            if (Input.GetKeyDown(KeyCode.Space)) { moveDir.y = Info.movespeed * 0.1f; }

            PlayerBody.forward = lookForward;
        }
        moveDir.y -= GameManager.instance.Gravity * Time.deltaTime;
        controller.Move(moveDir * Time.deltaTime * Info.movespeed);
    }


    protected override void OnDamage(Hit_param _param)
    {
        GetDamage(_param.damage);
    }

    protected override void GetDamage(ushort _damage)
    {
        StartCoroutine(HitObj());
        Info.hp -= (short)_damage;
        UIManager.Set_HP(Info.hp);
        if (Info.hp <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {

    }

    void OnTriggerStay(Collider _coll)
    {
        if(_coll.CompareTag("Fire"))
        {
            if(Time.time > FirenextHit)
            {
                FirenextHit = Time.time + FirehitRate;
                GetDamage(3);
            }
        }
    }

    void OnTriggerExit(Collider _coll)
    {
        if (_coll.CompareTag("Fire"))
        {
            StartCoroutine(FireDurationSEC());
        }
    }

    IEnumerator HitObj()
    {
        HitEffect.gameObject.SetActive(true);
        HitEffectObjOffTime = Time.time + HitEffect_ObjOffTime;
        while (HitEffect.gameObject.activeSelf)
        {
            yield return new WaitForSecondsRealtime(HitEffect_ObjOffTime);
            if (Time.time > HitEffectObjOffTime)
            {
                HitEffect.gameObject.SetActive(false);
                yield break;
            }
        }
    }

    IEnumerator FireDurationSEC()
    {
        FireEndDuration = Time.time + FireDuration;
        isFire = true;
        while (isFire)
        {
            yield return new WaitForSecondsRealtime(FirehitRate);
            GetDamage(3);
            if (Time.time > FireEndDuration)
            {
                isFire = false;
                yield break;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(camArm.transform.position, camArm.transform.position + camArm.transform.forward * 10f);
    }
}
