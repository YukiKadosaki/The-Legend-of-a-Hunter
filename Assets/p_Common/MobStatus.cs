using UnityEngine;

/// <summary>
/// Mob（動くオブジェクト、MovingObjectの略）の状態管理スクリプト
/// </summary>
public abstract class MobStatus : MonoBehaviour
{
    /// <summary>
    /// 状態の定義
    /// </summary>
    protected enum StateEnum
    {
        Normal, // 通常
        Attack, // 攻撃中
        Die // 死亡
    }

    protected enum BossNameEnum
    {
        Bee,
        Dragon,
        Dhurahan,
        Robot,
        Other
    }

    /// <summary>
    /// 移動可能かどうか
    /// </summary>
    public bool IsMovable => StateEnum.Normal == _state;

    /// <summary>
    /// 攻撃可能かどうか
    /// </summary>
    public bool IsAttackable => StateEnum.Normal == _state;


    //定数
    protected const float delta = 1;

    //メンバ変数にはm_をつけてます
    //アクセサにはm_は付いていません
    //メンバ変数への入力はアクセサを介して行いましょう

    [SerializeField] private float m_maxHp;//最大hp
    private float m_Hp;//現在のhp
    [SerializeField] private float m_defaultAtk;//初期攻撃力
    private float m_Atk;//現在の攻撃力
    [SerializeField] private float m_defaultMoveSpeed;//初期速度
    private float m_MoveSpeed;//現在の移動速度
    protected Animator _animator;
    protected StateEnum _state = StateEnum.Normal; // Mob状態
    [SerializeField] protected BossNameEnum _name;

    /// <summary>
    /// ライフの値を返します
    /// </summary>
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

    

    protected virtual void Start()
    {
        //ステータスの初期設定
        m_Hp = m_maxHp;
        m_Atk = m_defaultAtk;
        m_MoveSpeed = m_defaultMoveSpeed;
        _animator = GetComponentInChildren<Animator>();

        // ライフゲージの表示開始
        //LifeGaugeContainer.Instance.Add(this);
    }

    /// <summary>
    /// キャラが倒れた時の処理を記述します。
    /// </summary>
    protected virtual void OnDie()
    {
        // ライフゲージの表示終了
        //LifeGaugeContainer.Instance.Remove(this);
        switch (_name)
        {
            case BossNameEnum.Bee:
                PlayerPrefs.SetInt("Bee", 1);
                break;
            case BossNameEnum.Dragon:
                PlayerPrefs.SetInt("Dragon", 1);
                break;
            case BossNameEnum.Dhurahan:
                PlayerPrefs.SetInt("Dhurahan", 1);
                break;
            case BossNameEnum.Robot:
                PlayerPrefs.SetInt("Robot", 1);
                break;
        }

    }

    /// <summary>
    /// 指定値のダメージを受けます。
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (_state == StateEnum.Die) return;

        Hp -= damage;
        if (Hp > 0) return;

        _state = StateEnum.Die;
        //_animator.SetTrigger("Die");

        OnDie();
    }

    /// <summary>
    /// 可能であれば攻撃中の状態に遷移します。
    /// </summary>
    public void GoToAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");
    }

    /// <summary>
    /// 可能であればNormalの状態に遷移します。
    /// </summary>
    public void GoToNormalStateIfPossible()
    {
        if (_state == StateEnum.Die) return;

        _state = StateEnum.Normal;
    }
}