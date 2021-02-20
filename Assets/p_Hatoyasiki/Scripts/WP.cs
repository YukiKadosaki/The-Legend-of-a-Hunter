using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour {
    public GameObject[] neighborWP;
    public GameObject ParentPoint;
    public float consumptionCost;   // 実コスト
    public float forecastCost;      // 推定コスト
    public int WPValue;
    private static int maxWPValue = 10;
    private float[] distList = new float[maxWPValue];

    void Start() {
        WPValue = neighborWP.Length;
        for (int i=0; i<maxWPValue; i++) {
            if(neighborWP.Length < i){
                distList[i] = ObjDist(this.gameObject, neighborWP[i]);
            }else{
                break;
            }
        }
    }

    float ObjDist(GameObject objA, GameObject objB) {
        Vector3 aLocate = objA.transform.position;
        Vector3 bLocate = objB.transform.position;
        return Vector3.Distance(aLocate, bLocate);
    }
}
