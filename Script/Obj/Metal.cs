using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : MonoBehaviour
{
    void Hit(Hit_param _param)
    {
        GameObject effect = Poolmanager.GetEffect("Metal");
        effect.transform.position = _param.hit.point;
        effect.transform.rotation = Quaternion.LookRotation(_param.hit.normal);
        effect.transform.SetParent(this.gameObject.transform);
        effect.SetActive(true);
        effect.GetComponent<HitEffect>().Set("Metal");
    }
}
