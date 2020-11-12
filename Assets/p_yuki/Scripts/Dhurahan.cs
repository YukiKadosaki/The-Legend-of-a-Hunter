using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dhurahan : Boss
{
    public GameObject point;
    private const int BIGVALUE = 10000;
    private const int COLUMN = 200;
    private const int ROW = 200;
    private TerrainInfo[,] m_terrainInfo = new TerrainInfo[COLUMN, ROW];
    private GameObject[,] m_point = new GameObject[COLUMN, ROW];

    // Start is called before the first frame update
    void Start()
    {
        MakeTarrainTable();
        for(int i = 0;i < COLUMN; i++)
        {
            for(int j = 0;j < ROW; j++)
            {
                if (null != m_terrainInfo[i, j])
                {
                    Debug.Log(m_terrainInfo[i, j].m_Position);
                }
                else
                {
                    Debug.Log("i:" + i + "j:" + j);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //目的地（destination）に障害物などを避けながら移動する 
    public override IEnumerator MoveToDestination(Vector3 destination)
    {
        yield return null;
    }

    public void MakeTarrainTable()
    {
        const float top = 100;//レイの発射点の高さ

        for (int j = 0; j < ROW; j++)
        {
            for (int i = 0; i < COLUMN; i++)
            {
                //座標0, 0を中心に縦横COLUMN, ROWの範囲で地形把握
                Vector3 origin = new Vector3(i - COLUMN / 2, top, j - ROW / 2);
                Ray terRay = new Ray(origin, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(terRay, out hit, BIGVALUE))
                {
                    Debug.DrawRay(origin, Vector3.down * 1000, Color.red, 100, false);

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
                    {
                        m_terrainInfo[i, j] = new TerrainInfo(hit.point, false, 0, 0);
                    }
                    else
                    {
                        m_terrainInfo[i, j] = new TerrainInfo(hit.point, true, 0, 0);
                        m_point[i, j] = Instantiate(point) as GameObject;
                        m_point[i, j].transform.localPosition = hit.point;
                    }
                }
            }
        }
    }
}




//地形情報のクラス
public class TerrainInfo
{
    public Vector3 m_Position; //ノードの座標
    public bool m_CanThrough; //そのノードが通れるか
    private float m_NowCost;  //ノードの現在のコスト
    private float m_EstimatedCost; //ノードの予測コスト

    //コンストラクタ（インスタンス化されたときに呼ばれる
    public TerrainInfo(Vector3 pos, bool thr, float ncost, float escost)
    {
        m_Position = pos;
        m_CanThrough = thr;
        m_NowCost = ncost;
        m_EstimatedCost = escost;
    }
}