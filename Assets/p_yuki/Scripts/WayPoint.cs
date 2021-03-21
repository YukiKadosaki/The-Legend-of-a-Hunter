using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : WP
{
    private WayPoint[] m_NextPoints;
    private bool m_Freezed = false;//一度通った点をしばらくの間通らないようにさせるため、
    private const float FREEZE_TIME = 5f;

    public bool Freezed
    {
        set
        {
            m_Freezed = value;
        }
        get => m_Freezed;
    }


    //WPクラスをWayPointクラスに合わせるための変換
    public override void Start()
    {
        base.Start();
        m_NextPoints = new WayPoint[neighborWP.Length];
        for (int i = 0;i < neighborWP.Length;i++)
        {
            m_NextPoints[i] = neighborWP[i].GetComponent<WayPoint>();
        }
    }


    //隣り合うウェイポイントの個数を返す
    public int ReturnNextPointNum()
    {
        return m_NextPoints.Length;
    }

    //添え字をもらって隣接するウェイポイントを返す
    public WayPoint ReturnWayPoint(int i)
    {
        int startIndex = i;


        //凍っていないウェイポイントを探す
        while (i < 20) {
            if (!m_NextPoints[i % m_NextPoints.Length].Freezed)
            {
                return m_NextPoints[i % m_NextPoints.Length];
            }
            i++;
            //Debug.Log("Skip");
        }

        return m_NextPoints[0];
    }

    //一定時間凍らせる
    public void FreezeAndDefrost()
    {
        Freeze();
        Invoke("Defrost", FREEZE_TIME);
    }

    //ウェイポイントを凍らせる
    private void Freeze()
    {
        Freezed = true;
        //後で消す
        this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0);
    }
    //ウェイポイントを解凍する
    private void Defrost()
    {
        Freezed = false;
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
