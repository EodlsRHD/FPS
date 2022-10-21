using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var newobj = new GameObject().AddComponent<GameManager>();
                    _instance = newobj;
                }
            }
            return _instance;
        }
    }

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private float JumpPower;
    [SerializeField] public float Gravity;

    void Awake()
    {
        MoveSpeed = 30f;
        TurnSpeed = 5f;
        JumpPower = 500f;
        Gravity = 9.8f;

        DontDestroyOnLoad(this.gameObject);
    }

    public float GetMoveSpeed()
    {
        return MoveSpeed;
    }
    public float GetTurnSpeed()
    {
        return TurnSpeed;
    }
    public float GetJumpPower()
    {
        return JumpPower;
    }
}
