using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dullahan_AttackRange : MonoBehaviour
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
            if (m_Dullahan.BossState == Dhurahan1.DhurahanState.Find)
            {

                Vector3 origin = this.transform.root.position;
                origin.y += 5;
                Vector3 direction = m_player.transform.localPosition - origin;


                Ray ray = new Ray(origin, direction);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(origin, direction);
                    //デュラハンがプレイヤーを攻撃する
                    m_Dullahan.GoToAttackState();
                }
            }
        }
    }
}
