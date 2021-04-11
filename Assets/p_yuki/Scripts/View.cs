//デュラハンの子オブジェクト
//Dhurahan1と関連

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private Rigidbody m_player;
    private Dhurahan1 m_Dullahan;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        m_Dullahan = transform.root.GetComponent<Dhurahan1>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_Dullahan.BossState == Dhurahan1.DhurahanState.Search)
        {
            if (other.CompareTag("Player"))
            {
                Ray ray = new Ray(this.transform.position, m_player.transform.localPosition - this.transform.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
                {
                    //デュラハンがプレイヤーを追いかけ続ける
                    m_Dullahan.FindPlayer(m_player);
                }
            }
        }
    }
}
