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

    [Header("開始地点のウェイポイント")]
    [SerializeField] private WayPoint m_StartWayPoint;
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

    //public Collider View



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        NextWayPoint = m_StartWayPoint;
        m_Transform = transform;
        m_beforePosition = m_Transform.localPosition;
        _animator.SetFloat("Speed", MoveSpeed);

        GetWaypoints();
        SpeedChange(2);//仮



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
            else if(BossState == DhurahanState.Listen)
            {
                //目的地へ移動
                StartCoroutine("MoveToDestination", Destination);
            }

            IsStateChanging = false;
        }



    }

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

    private IEnumerator SearchMove()
    {
        while (true)
        {
            m_MoveLinear = MoveLiner(NextWayPoint.transform.localPosition);
            yield return StartCoroutine(m_MoveLinear);

            
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

    private void ChooseNextPoint(WayPoint point)
    {
        int random = Random.Range(0, point.ReturnNextPointNum());
        NextWayPoint = point.ReturnWayPoint(random);
    }

    public override IEnumerator MoveLiner(Vector3 destination)
    {

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.m_Transform.localPosition);
        while (true)
        {
            direction.y = 0;

            this.m_Transform.localPosition += Time.deltaTime * MoveSpeed * direction.normalized;
            m_Transform.LookAt(m_Transform.localPosition + direction);

            yield return null;

            if (Vector3.Distance(this.m_Transform.localPosition, destination) <= delta)
            {
                yield break;
            }
        }
    }

    //Findステートになっているはず。
    //特定のオブジェクトを追いかけ続ける
    private IEnumerator SeekObjectAndCountTime(Rigidbody obj, int seekTime)
    {
        float time = 0;
        while (time <= seekTime)
        {
            if (!isRunning)
            {
                StartCoroutine(MoveToDestination(obj.position));
            }
            Debug.Log("time:" + time);
            time += Time.deltaTime;
            yield return null;
        }
        BossState = DhurahanState.Search;
        SpeedChange(m_defaultMoveSpeed);

    }

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
        foreach(WayPoint p in m_WayPoints)
        {
            //50は定数
            p.ChangeFreezeTime(50 / speed);
        }
    }

    //Viewがプレイヤーを検知すると起動する
    //移動速度が変わる
    public void FindPlayer(Rigidbody player)
    {
        BossState = DhurahanState.Find;
        StartCoroutine(SeekObjectAndCountTime(player, 8));
        SpeedChange(4f);
    }

}
