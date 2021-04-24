//Speakerと関連
//Viewと関連
//Updateで状態遷移を制御

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dhurahan1 : Boss
{
    //ボスの状態を表す
    public enum DhurahanState
    {
        Search,
        Listen,
        Find,
        Attack
    }

    [Header("視界")]
    [SerializeField] private Collider m_View;
    private Transform m_Transform;
    private DhurahanState m_BossState = DhurahanState.Search; //ボスの状態
    private WayPoint m_NextWayPoint;//現在の移動先
    private WayPoint[] m_WayPoints;
    private bool m_IsStateChanging = false;//状態の遷移中はtrue
    private Vector3 m_Destination;//目的地
    private IEnumerator m_MoveLinear;
    protected new const float delta = 2;
    private Vector3 m_beforePosition;
    private Rigidbody m_SeekingObject = null;
    private const float defaultSeekTime = 15;
    private double m_SeekTime;

    public DhurahanState BossState {
        get => m_BossState;
        set { m_BossState = value; }
    }

    public WayPoint NextWayPoint
    {
        get => m_NextWayPoint;
        set { m_NextWayPoint = value; }
    }

    public bool IsStateChanging
    {
        get => m_IsStateChanging;
        set { m_IsStateChanging = value; }
    }

    public Vector3 Destination
    {
        get => m_Destination;
        set { m_Destination = value; }
    }
    public Rigidbody SeekingObject
    {
        get => m_SeekingObject;
        set { m_SeekingObject = value; }
    }
    public double SeekTime
    {
        get => m_SeekTime;
        set { m_SeekTime = value; }
    }

    //public Collider View



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_Transform = transform;
        m_beforePosition = m_Transform.localPosition;
        SeekTime = defaultSeekTime;
        _animator.SetFloat("Speed", MoveSpeed);
        //WayPointのフリーズ時間計算などのためにデュラハンがWayPointの情報を得る必要がある
        GetWaypoints();
        SpeedChange(m_defaultMoveSpeed);



        //プレイヤーを捜索
        StartCoroutine(SearchMove());
    }

    // Update is called once per frame
    void Update()
    {

        //状態遷移中
        if (IsStateChanging)
        {
            //探索（Search）状態の時
            if (BossState == DhurahanState.Search)
            {
                //プレイヤーを捜索
                StartCoroutine(SearchMove());
            }
            else if (BossState == DhurahanState.Listen)
            {
                //目的地へ移動
                StartCoroutine("MoveToDestination", Destination);
            }
            else if (BossState == DhurahanState.Find)
            {
                StartCoroutine(SeekObjectAndCountTime(SeekingObject));
                SpeedChange(6f);
            }
            else if(BossState == DhurahanState.Attack)
            {
                Attack(SeekingObject);
            }

            IsStateChanging = false;
        }



    }

    /// <summary>
    /// 初期化、いつでも使う
    /// </summary>
    private void GetWaypoints()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("WP");
        m_WayPoints = new WayPoint[obj.Length];
        int i = 0;
        foreach (GameObject o in obj)
        {
            m_WayPoints[i] = o.GetComponent<WayPoint>();
            i++;
        }
    }

    private void SpeedChange(float speed)
    {
        MoveSpeed = speed;
        foreach (WayPoint p in m_WayPoints)
        {
            //50は定数
            p.ChangeFreezeTime(50 / speed);
        }
    }

    /// <summary>
    /// ステート遷移系
    /// </summary>
    /// <param name="other"></param>

    //////////////////////////////////////////Listen
    private void OnTriggerEnter(Collider other)
    {
        //音が聞こえたらFindステートへ
        if (other.CompareTag("Speaker"))
        {
            StopCoroutine(m_MoveLinear);
            Destination = other.transform.position;
            IsStateChanging = true;
            BossState = DhurahanState.Listen;
        }
    }

    ///////////////////////////////////////////Search
    private void GoToSearchState()
    {
        IsStateChanging = true;
        BossState = DhurahanState.Search;
    }


    //Viewがプレイヤーを検知すると起動する
    //移動速度が変わる
    //////////////////////////////////////////Find
    public void FindPlayer(Rigidbody player)
    {
        IsStateChanging = true;
        BossState = DhurahanState.Find;
        SeekingObject = player;
    }

    //AttackRangeがプレイヤーを検知すると起動する
    /////////////////////////////////////////Attack
    public void GoToAttackState()
    {
        IsStateChanging = true;
        m_BossState = DhurahanState.Attack;
    }


    /// <summary>
    /// 各ステートに対応するコルーチンや関数
    /// </summary>
    /// <returns></returns>

    ///////////////////////////////////////////Search
    private IEnumerator SearchMove()
    {
        GameObject point = base.serchPointTag(m_Transform.localPosition, "WP");
        NextWayPoint = point.GetComponent<WayPoint>();
        while (true)
        {
            m_MoveLinear = this.MoveLiner(NextWayPoint.transform.localPosition);
            Debug.Log("SartMoveLinear");
            yield return StartCoroutine(m_MoveLinear);
            Debug.Log("EndMoveLinear");
            
            //目的地へ到達したら、次の目的地へ
            if (Vector3.Distance(NextWayPoint.transform.localPosition, this.m_Transform.localPosition) <= delta * 30)
            {
                //見つけたウェイポイントを一時的に凍らせる
                NextWayPoint.FreezeAndDefrost();
                ChooseNextPoint(NextWayPoint);
            }
            if(m_BossState != DhurahanState.Search)
            {
                yield break;
            }
        }
    }

    ///////////////////////////////////////////Search
    private void ChooseNextPoint(WayPoint point)
    {
        int random = Random.Range(0, point.ReturnNextPointNum());
        NextWayPoint = point.ReturnWayPoint(random);
    }

    ///////////////////////////////////////////Search
    public override IEnumerator MoveLiner(Vector3 destination)
    {

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.m_Transform.position);
        int count = 0;
        float time = 0;

        while (true)
        {
            //3回に一回目的地を見直す
            if (count%3 == 0)
            {
                direction = (destination - this.m_Transform.position);
                count = 0;
            }
            else
            {
                count++;
            }
            direction.y = 0;

            this.m_Transform.position += Time.deltaTime * MoveSpeed * direction.normalized;
            m_Transform.LookAt(m_Transform.localPosition + direction);
            time += Time.deltaTime;

            yield return null;
            

            if (Vector3.Distance(this.m_Transform.position, destination) <= delta || time >= 10)
            {

                Debug.Log("WalkEnd");
                yield break;
            }
        }
    }


    //特定のオブジェクトを追いかけ続ける
    ////////////////////////////////////////////Find
    private IEnumerator SeekObjectAndCountTime(Rigidbody obj)
    {
        Coroutine moveToDestination = null;
        float time = 0;

        while (time <= SeekTime)
        {
            if (!isRunning)
            {
                Debug.Log("RunStart");
                moveToDestination = StartCoroutine(MoveToDestination(obj.position));
            }
            time += Time.deltaTime;
            yield return null;
        }
        

        //タイムアップ後処理
        //RouteList.Clear();
        
        if (null != moveToDestination)
        {
            StopCoroutine(moveToDestination);
        }
        isRunning = false;
        SeekTime = defaultSeekTime;
        SpeedChange(m_defaultMoveSpeed);
        //プレイヤーを捜索
        GoToSearchState();
        
    }

    //追いかける時間を延長する
    ////////////////////////////////////////////Find
    public void AddSeekTime()
    {
        SeekTime += Time.deltaTime;
    }

    //攻撃アニメーション
    ////////////////////////////////////////////Attack
    private void Attack(Rigidbody obj)
    {
        Vector3 lookDir = obj.position - m_Transform.position;
        lookDir.y = 0;
        m_Transform.LookAt(m_Transform.position + lookDir);
        _animator.SetTrigger("Attack");
    }


}
