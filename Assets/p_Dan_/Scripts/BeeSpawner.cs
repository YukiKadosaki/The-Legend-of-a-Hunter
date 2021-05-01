using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BeeSpawner : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject enemyPrefab;

   private void Start()
    {
        StartCoroutine(SpawnLoop());//Coroutineを開始
    }

    /// <summary>
    /// 摘出減のCoroutine
    /// </summary>
    /// <returns></returns>

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            //距離１０のベクトル
            var distanceVector = new Vector3(10, 0);
            //プレイヤーのいる位置をベースにした敵の出現位置。Y軸に対して上記のベクトルをランダムに０°〜360°回転させている
            var spawnPositionFromPlayer = Quaternion.Euler(0, Random.Range(0, 360f), 0) * distanceVector;

            //指定座標から一番近いNavMeshの座標を探す
            NavMeshHit navMeshHit;
            if(NavMesh.SamplePosition(spawnPositionFromPlayer, out navMeshHit, 10, NavMesh.AllAreas))
            {
                //enemyPrefabを複製、NavMeshAgentは必ずNavMesh上に配置する
                Instantiate(enemyPrefab, navMeshHit.position, Quaternion.identity);
            }
            //10秒待つ
            yield return new WaitForSeconds(10);
            
            //if(player.Life <= 0)
            //{
                //プレイヤーが倒れたらループを抜ける
                //break;
            //}
        }
    }
}
