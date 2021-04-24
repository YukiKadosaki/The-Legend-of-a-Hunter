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
        if (other.CompareTag("Player"))
        {
            Vector3 origin = this.transform.root.position;
            origin.y += 5;
            Vector3 direction = m_player.transform.localPosition - origin;


            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            if (m_Dullahan.BossState == Dhurahan1.DhurahanState.Search)
            {
                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(origin, direction);
                    //デュラハンがプレイヤーを追いかけ続ける
                    m_Dullahan.FindPlayer(m_player);
                }
            }
            else if(m_Dullahan.BossState == Dhurahan1.DhurahanState.Find)
            {
                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
                {
                    Debug.Log(hit.collider.name);
                    m_Dullahan.AddSeekTime();
                }
            }
        }
    }
}
