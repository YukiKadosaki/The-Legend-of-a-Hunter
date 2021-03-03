using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    public GameObject player;
    private static float WPMaxTime = 1.5f;
    private float WPReloadTime = 0f;

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

    Vector3 GetNextPoint(){
        if(RouteList.Count == 0)
            return this.transform.position;
        // 次目的地までの距離が近ければ目的地を次に移す
        Vector3 moveDist = this.transform.position - RouteList[0].transform.position;
        moveDist.y = 0;
        if(moveDist.magnitude < delta){
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
}
