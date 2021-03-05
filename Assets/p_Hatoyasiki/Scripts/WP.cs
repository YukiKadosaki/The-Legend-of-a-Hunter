using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP : MonoBehaviour {
    public GameObject[] neighborWP;
    private GameObject m_ParentPoint;
    private float m_consumptionCost;   // 実コスト
    private float m_forecastCost;      // 推定コスト
    private int m_WPValue;
    private static int maxWPValue = 10;
    private float[] distList = new float[maxWPValue];

    public GameObject ParentPoint{
        get => m_ParentPoint;
        set{
            m_ParentPoint = value;
        }
    }
    public float consumptionCost{
        get => m_consumptionCost;
        set{
            m_consumptionCost = value;
            if(m_consumptionCost < 0)
                m_consumptionCost = 0f;
        }
    }
    public float forecastCost{
        get => m_forecastCost;
        set{
            m_forecastCost = value;
            if(m_forecastCost < 0)
                m_forecastCost = 0f;
        }
    }
    public int WPValue{
        get => m_WPValue;
        set{
            m_WPValue = value;
            if(m_forecastCost < 0)
                m_WPValue = 0;
        }
    }




    void Start() {
        WPValue = neighborWP.Length;
        for (int i=0; i<maxWPValue; i++) {
            if(neighborWP.Length < i){
                distList[i] = Vector3.Distance(this.gameObject.transform.position, neighborWP[i].transform.position);
            }else{
                break;
            }
        }
    }
}
