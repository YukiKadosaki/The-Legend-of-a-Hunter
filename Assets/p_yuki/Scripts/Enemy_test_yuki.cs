using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_test_yuki : MonoBehaviour
{
    Animator Query;
    // Start is called before the first frame update
    void Start()
    {
        Query = this.gameObject.GetComponent<Animator>();
        Query.Play("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        Query.Play("Attack");
        if (Query.GetCurrentAnimatorStateInfo(0).normalizedTime > 10)
        {
            //10回繰り返したら
            //enabledで停止
            Query.enabled = false;
        }
    }
}
