using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract（抽象）クラスです。各ボスに継承して使ってください
public abstract class Boss : MonoBehaviour
{
    //定数
    private const float delta = 0.1f;

    //メンバ変数にはm_をつけてます
    //アクセサにはm_は付いていません
    //メンバ変数への入力はアクセサを介して行いましょう

    [SerializeField] private float m_maxHp;//最大hp
    private float m_Hp;//現在のhp
    [SerializeField] private float m_defaultAtk;//初期攻撃力
    private float m_Atk;//現在の攻撃力
    [SerializeField] private float m_defaultMoveSpeed;//初期速度
    private float m_MoveSpeed;//現在の移動速度

    //初期化
    //Awake()はStart()よりも先に実行されます。
    private void Awake()
    {
        m_Hp = m_maxHp;
        m_Atk = m_defaultAtk;
        m_MoveSpeed = m_defaultMoveSpeed;
    }

    //アクセサ
    //※下のgetの設定により、 float a = Hp と書いたとき、aに代入される値はm_Hpとなります
    //get => は get { }と同じです
    //※下のsetの設定により、 Hp = -100 と書いたとき、m_Hpに0が代入されます
    public float Hp
    {
        get => m_Hp;
        set
        {
            m_Hp = value;

            if(m_Hp >= m_maxHp)
            {
                m_Hp = m_maxHp;
            }

            if(m_Hp < 0)
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

            if(m_Atk < 0)
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

    //コルーチン　継承先でオーバーライドして使う
    //目的地(destination）に直線移動する
    public virtual IEnumerator MoveLiner(Vector3 destination)
    {
        while (true)
        {
            float y = this.transform.localPosition.y;
            //自分の現在地から目的地までの方向
            Vector3 direction = (destination - this.transform.localPosition);
            direction.y = y;

            this.transform.localPosition += Time.deltaTime * m_MoveSpeed * direction.normalized;

            yield return null;
            if(Vector3.Distance(this.transform.localPosition, destination) <= delta){
                yield break;
            }
        }
    }
        
    //目的地（destination）に障害物などを避けながら移動する 
    public abstract IEnumerator MoveToDestination(Vector3 destination);
}
