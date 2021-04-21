using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BeeMove : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // NavMeshAgentを保持しておく
    }

    // CollisionDetector.csのonTriggerStayにセットし、衝突中に実行される。
    public void OnDetectObject(Collider collider)
    {
        // 検知したオブジェクトに「Player」のタグがついていれば、そのオブジェクトを追いかける
        if (collider.CompareTag("Player"))
        {
            navMeshAgent.destination = collider.transform.position;
        }
    }
}