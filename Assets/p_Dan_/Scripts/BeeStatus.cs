using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//蜂の状態管理スクリプト
[RequireComponent(typeof(NavMeshAgent))]
public class BeeStatus : MobStatus
{
    private NavMeshAgent _agent;

    protected override void Start()
    {
        base.Start();

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //NavMeshAgentのvelocityで移動速度のベクトルが取得できる
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
    }

    protected override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
    }

    //倒された時の消滅コルーチンです
    /// <returns></returns>
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
