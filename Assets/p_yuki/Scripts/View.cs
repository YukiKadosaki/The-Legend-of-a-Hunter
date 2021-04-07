using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    private GameObject m_player;

    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player：" + other.name);
            
            Ray ray = new Ray(this.transform.position, m_player.transform.localPosition - this.transform.position);
            RaycastHit hit;
            Debug.DrawRay(this.transform.position, m_player.transform.localPosition - this.transform.position , Color.red);
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}
