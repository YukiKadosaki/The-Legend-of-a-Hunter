//Speakerと関連
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
        Find,
    }

    [Header("開始地点のウェイポイント")]
    [SerializeField] private WayPoint m_StartWayPoint;
    [Header("視界")]
    [SerializeField] private Collider m_View;
    private DhurahanState m_BossState = DhurahanState.Search; //ボスの状態
    private WayPoint m_NextWayPoint;//現在の移動先
    private bool m_IsStateChanging = false;//状態の遷移中はtrue
    private Vector3 m_Destination;//目的地
    private IEnumerator m_MoveLinear;

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
            else if(BossState == DhurahanState.Find)
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
            BossState = DhurahanState.Find;
        }
    }

    private IEnumerator SearchMove()
    {
        while (true)
        {
            m_MoveLinear = MoveLiner(NextWayPoint.transform.localPosition);
            yield return StartCoroutine(m_MoveLinear);


            //目的地へ到達したら、次の目的地へ
            if (Vector3.Distance(NextWayPoint.transform.localPosition, this.transform.localPosition) <= delta * 10)
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

}
