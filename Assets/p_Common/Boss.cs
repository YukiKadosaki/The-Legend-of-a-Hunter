using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//abstract（抽象）クラスです。各ボスに継承して使ってください
public abstract class Boss : MobStatus
{
    
    //コルーチン　継承先でオーバーライドして使う
    //目的地(destination）に直線移動する
    public virtual IEnumerator MoveLiner(Vector3 destination)
    {

        //自分の現在地から目的地までの方向
        Vector3 direction = (destination - this.transform.localPosition);
        while (true)
        {
            direction.y = 0;

            this.transform.localPosition += Time.deltaTime * MoveSpeed * direction.normalized;

            yield return null;
            if(Vector3.Distance(this.transform.localPosition, destination) <= delta){
                yield break;
            }
        }
    }
        
    //目的地（destination）に障害物などを避けながら移動する 
    public abstract IEnumerator MoveToDestination(Vector3 destination);
}
