using System.Collections;
using UnityEngine;

//攻撃制御クラス
[RequireComponent(typeof(MobStatus))]
public class BeeAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f;//攻撃後のクールダウン(秒)
    [SerializeField] private Collider attackCollider;

    private MobStatus _status;

    private void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    //攻撃可能な状態であれば攻撃を行います
    public void AttackIfPossible()
    {
        if (!_status.IsAttackable) return;
        //ステータスと衝突したオブジェクトで攻撃可否を判断

        _status.GoToAttackStateIfPossible();
    }

    //攻撃対象が攻撃範囲に入った時に呼ばれます
    ///<param name="collider"></param>
    public void OnAttackRangeEnter(Collider collider)
    {
        AttackIfPossible();
    }

    //攻撃の開始時に呼ばれます
    public void OnAttackStart()
    {
        attackCollider.enabled = true;
    }

    //attackColliderが攻撃対象にHitした時に呼ばれます
    ///<param name="collider"></param>
    public void OnHitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        if (null == targetMob) return;

        //プレイヤーにダメージを与える
        targetMob.Damage(1);
    }

    //攻撃の終了時に呼ばれます
    public void OnAttackFinished()
    {
        attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _status.GoToNormalStateIfPossible();
    }

}