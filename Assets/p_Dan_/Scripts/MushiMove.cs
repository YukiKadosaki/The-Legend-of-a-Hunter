using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MushiMove : MonoBehaviour
{

    private NavMeshAgent _agent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    //CollisionDetectorのonTriggersStayにセットし、衝突判定を受け取るメソッド
    public void OnDetectObject(Collider collider)
    {
        //検知したオブジェクトに「player」のタグがついていれば、そのオブジェクトを追いかける
        if (collider.CompareTag("Player"))
        {
            //自身とプレイヤーの座標差分を計算
            var positionDiff = collider.transform.position - transform.position;
            //プレイヤーとの距離を計算
            var distance = positionDiff.magnitude;
            //プレイヤーへの方向
            var direction = positionDiff.normalized;
            //_rayvastHitsに、ヒットしたcolliderや座標情報などが格納される
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance);
            Debug.Log("hitCount:" + hitCount);
            if (hitCount != 0)
            {
                _agent.isStopped = false;
                _agent.destination = collider.transform.position;
            }
            else
            {
                _agent.isStopped = true;
            }

            
        }
    }
}
