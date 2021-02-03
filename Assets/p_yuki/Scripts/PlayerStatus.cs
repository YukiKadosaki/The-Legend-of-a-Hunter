using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float m_maxHp;//最大hp
    private float m_Hp;//現在のhp
    [SerializeField] private float m_defaultAtk;//初期攻撃力
    private float m_Atk;//現在の攻撃力
    [SerializeField] private float m_defaultMoveSpeed;//初期速度
    private float m_MoveSpeed;//現在の移動速度
    private bool m_IsAttackable;//攻撃できるかどうか

    //初期化
    //Awake()はStart()よりも先に実行されます。
    private void Awake()
    {
        m_Hp = m_maxHp;
        m_Atk = m_defaultAtk;
        m_MoveSpeed = m_defaultMoveSpeed;
    }

    //アクセサ
    public float Hp
    {
        get => m_Hp;
        set
        {
            m_Hp = value;

            if (m_Hp >= m_maxHp)
            {
                m_Hp = m_maxHp;
            }

            if (m_Hp < 0)
            {
                m_Hp = 0;
            }
        }
    }
    public float Atk
    {
        get => m_Atk;
        set
        {
            m_Atk = value;

            if (m_Atk < 0)
            {
                m_Atk = 0;
            }
        }
    }
    public float MoveSpeed
    {
        get => m_MoveSpeed;
        set
        {
            m_MoveSpeed = value;

            if (m_MoveSpeed < 0)
            {
                m_MoveSpeed = 0;
            }
        }
    }
    public bool IsAttackable
    {
        get => m_IsAttackable;
        set { m_IsAttackable = value; }
    }


}
