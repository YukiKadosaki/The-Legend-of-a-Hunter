//Speakerと関連

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dhurahan1 : Boss
{
    //ボスの状態を表す
    private enum State
    {
        Search,
        Find
    }

    [Header("開始地点のウェイポイント")]
    [SerializeField] WayPoint m_StartWayPoint;
    private const float delta = 1;//小さい値
    private State m_State = State.Search; //ボスの状態
    private WayPoint m_WayPoint;//現在の移動先


    // Start is called before the first frame update
    void Start()
    {
        m_WayPoint = m_StartWayPoint;

        //プレイヤーを捜索
        StartCoroutine("SearchMove");
    }

    // Update is called once per frame
    void Update()
    {
        //探索（Search）状態の時
        if(m_State == State.Search)
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speaker"))
        {
            Debug.Log("Destination :" + other.transform.position);
        }
    }

    private IEnumerator SearchMove()
    {
        while (true)
        {
            yield return StartCoroutine("MoveLiner", m_WayPoint.transform.localPosition);


            //目的地へ到達したら、次の目的地へ
            if (Vector3.Distance(m_WayPoint.transform.localPosition, this.transform.localPosition) <= delta * 10)
            {
                //見つけたウェイポイントを一時的に凍らせる
                m_WayPoint.FreezeAndDefrost();
                ChooseNextPoint(m_WayPoint);
            }
            if(m_State != State.Search)
            {
                yield break;
            }
        }
    }

    private void ChooseNextPoint(WayPoint point)
    {
        int random = Random.Range(0, point.ReturnNextPointNum());
        m_WayPoint = point.ReturnWayPoint(random);
    }

    public override IEnumerator MoveToDestination(Vector3 destination)
    {
        yield return null;
    }
}
