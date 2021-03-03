using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    private GameObject player;
    private static float WPMaxTime = 2f;
    private bool isRunning = false;
    private float WPReloadTime = 0f;
    private List<GameObject> RouteList;

    void Start() {
        MoveSpeed = 10f;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        AStar(serchTag(gameObject, "WP"), player.GetComponent<Player>().getPlayerWP());
    }

    void Update() {
        WPReloadTime += Time.deltaTime;
        if(WPReloadTime >= WPMaxTime || RouteList.Count == 0){
            AStar(serchTag(gameObject, "WP"), player.GetComponent<Player>().getPlayerWP());
            WPReloadTime = 0f;
        }
        // Debug.Log(isRunning);
        StartCoroutine("MoveToDestination", GetNextPoint());
    }

    //目的地（destination）に障害物などを避けながら移動する 
    public override IEnumerator MoveToDestination(Vector3 destination){
        if(isRunning){
            yield break;
        }
        isRunning = true;

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.transform.localPosition);
        direction.y = 0;
        this.transform.localPosition += Time.deltaTime * MoveSpeed * direction.normalized;

        yield return null;

        isRunning = false;
    }
    Vector3 GetNextPoint(){
        if(RouteList.Count == 0)
            return this.transform.position;
        if(RouteList[0] == player){
            // 次目的地を返す
            return RouteList[0].transform.position;
        }
        // 次目的地までの距離が近ければ目的地を次に移す
        Vector3 moveDist = this.transform.position - RouteList[0].transform.position;
        moveDist.y = 0;
        if(moveDist.magnitude < 0.5f){
            RouteList.Remove(RouteList[0]);
        }
        if(RouteList.Count == 0)
            return this.transform.position;
        // 次目的地を返す
        return RouteList[0].transform.position;
    }

    GameObject serchTag(GameObject nowObj,string tagName){
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)){
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis){
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        return targetObj;
    }

    // A*アルゴリズム関係
    void AStar(GameObject startWP, GameObject goalWP){
        if(startWP == goalWP){
            RouteList = new List<GameObject>(){player};
            return;
        }
        // リストの初期化
        List<GameObject> OpenNodeList = new List<GameObject>();
        List<GameObject> ClosedNodeList = new List<GameObject>();
        // スタート(ボス)ウェイポイントをOPリストに追加
        OpenNodeList.Add(startWP);
        // 全ウェイポイントの初期化
        InitWayPoint(goalWP);
        // 先に宣言(使用メモリ減らす用．意味ある？)
        GameObject checkNode;
        WP checkNodeClass;
        // OPリストに何か入っていたらループ
        while(OpenNodeList.Any()){
            // OPリスト内で一番スコアの低いウェイポイントの取得
            checkNode = ChoiseCheckPoint(OpenNodeList);
            // GameObject内のスクリプト(自作)の取得
            checkNodeClass = checkNode.GetComponent<WP>();
            // チェックノードに隣り合うウェイポイントの調査
            for(int i=0; i<checkNodeClass.WPValue; i++){
                // 隣のウェイポイント(CLリストにいれば調べない)
                GameObject neighbor = checkNodeClass.neighborWP[i];
                if(ClosedNodeList.IndexOf(neighbor) >= 0)
                    continue;
                // OPリストに追加
                OpenNodeList.Add(neighbor);
                // スクリプトの取得
                WP neighborClass = neighbor.GetComponent<WP>();
                // 実コストの測定
                neighborClass.consumptionCost = checkNodeClass.consumptionCost + ObjDist(checkNode, neighbor);
                // 経路のツリー作成
                neighborClass.ParentPoint = checkNode;
                // ゴールしたら経路を追ってリスト作成
                if(neighbor == goalWP){
                    RouteList = GetRoute(startWP, goalWP);
                    // foreach (GameObject RouteNode in RouteList) {
                    //     Debug.Log(RouteNode);
                    // }
                    return;
                }
            }
            // OPリストから削除してCLリストに追加
            OpenNodeList.Remove(checkNode);
            ClosedNodeList.Add(checkNode);
        }
    }
    void InitWayPoint(GameObject goalPoint){
        // 全ウェイポイント取得
        GameObject[] WPList = GameObject.FindGameObjectsWithTag("WP");
        // 先に宣言
        WP WPClass;
        foreach(GameObject WP in WPList){
            WPClass = WP.GetComponent<WP>();
            // 実コスト．親ノードの削除
            WPClass.consumptionCost = 0;
            WPClass.ParentPoint = null;
            // ゴールWPから予測コスト設定(直線距離)
            WPClass.forecastCost = ObjDist(WP, goalPoint);
        }
    }
    // 二点間距離の取得
    float ObjDist(GameObject objA, GameObject objB) {
        Vector3 aLocate = objA.transform.position;
        Vector3 bLocate = objB.transform.position;
        return Vector3.Distance(aLocate, bLocate);
    }

    GameObject ChoiseCheckPoint(List<GameObject> OpList){
        // returnするウェイポイント
        GameObject checkPoint = null;
        // 最小値格納する変数
        float minScore = Mathf.Infinity;
        // 先に宣言
        float score;
        // リスト内全点のスコア測定&最小スコアのオブジェクト捜査
        foreach(GameObject OpWP in OpList){
            WP OpWPClass = OpWP.GetComponent<WP>();
            score = OpWPClass.forecastCost + OpWPClass.consumptionCost;
            if(score < minScore){
                checkPoint = OpWP;
                minScore = score;
            }
        }
        return checkPoint;
    }
    // 設定された親ノードを追ってルートリストの取得
    List<GameObject> GetRoute(GameObject SWP, GameObject CWP){
        if(SWP == CWP){
            return new List<GameObject>();
        }else{
            List<GameObject> returnRoute = GetRoute(SWP, CWP.GetComponent<WP>().ParentPoint);
            returnRoute.Add(CWP);
            return returnRoute;
        }
    }
}
