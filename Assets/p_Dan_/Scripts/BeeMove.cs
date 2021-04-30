using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BeeStatus))]
public class BeeMove : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private BeeStatus _status;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgentを保持しておく
        _status = GetComponent<BeeStatus>();
    }

    // CollisionDetector.csのonTriggerStayにセットし、衝突中に実行される。
    public void OnDetectObject(Collider collider)
    {
        if(!_status.IsMovable)
        {
            navMeshAgent.isStopped = true;
            return;
        }
        // 検知したオブジェクトに「Player」のタグがついていれば、そのオブジェクトを追いかける
        if (collider.CompareTag("Player"))
        {
            navMeshAgent.destination = collider.transform.position;
        }
    }
}