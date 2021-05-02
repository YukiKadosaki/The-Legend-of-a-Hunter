using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//abstract（抽象）クラスです。各ボスに継承して使ってください
public abstract class Boss : MobStatus
{
    private List<GameObject> m_RouteList = new List<GameObject>();
    protected bool isRunning = false;

    public List<GameObject> RouteList{
        get => m_RouteList;
    }
    
    //コルーチン　継承先でオーバーライドして使う
    //目的地(destination）に直線移動する
    public virtual IEnumerator MoveLiner(Vector3 destination)
    {

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.transform.localPosition);
        while (true)
        {
            direction.y = 0;

            this.transform.localPosition += Time.deltaTime * MoveSpeed * direction.normalized;

            yield return null;
            Debug.Log(Vector3.Distance(this.transform.localPosition, destination));
            if(Vector3.Distance(this.transform.localPosition, destination) <= delta){
                yield break;
            }
        }
    }
        
    //目的地（destination）に障害物などを避けながら移動する 
    public virtual IEnumerator MoveToDestination(Vector3 goalPoint) {
        if(isRunning)
            yield break;
        isRunning = true;

        AStar(serchPointTag(gameObject.transform.position, "WP"), serchPointTag(goalPoint, "WP"));
        while (true) {
            if (RouteList.Count == 0){
                isRunning = false;
                yield break;
            }
            Vector3 destination = RouteList[0].transform.position;
            Vector3 moveDist = this.transform.position - destination;
            moveDist.y = 0;


            while (moveDist.magnitude >= delta) {
                //自分の現在地から目的地までの方向
                Vector3 direction = (destination - this.transform.position);
                direction.y = 0;
                this.transform.position += Time.deltaTime * MoveSpeed * direction.normalized;
                Debug.Log("MoveSpeed:" + MoveSpeed);
                Debug.Log("x：" + Time.deltaTime * MoveSpeed * direction.normalized);
                this.transform.LookAt(this.transform.position + direction);
                yield return null;
                moveDist = this.transform.position - destination;
                moveDist.y = 0;
            }
            if (null != RouteList[0])
            {
                RouteList.Remove(RouteList[0]);
            }
        }
    }

    // A*アルゴリズム関係
    public void AStar(GameObject startWP, GameObject goalWP){
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
                    m_RouteList = GetRoute(startWP, goalWP);
                    // foreach (GameObject RouteNode in m_RouteList) {
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

    protected GameObject serchPointTag(Vector3 objPos,string tagName){
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagName)){
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, objPos);

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
}
