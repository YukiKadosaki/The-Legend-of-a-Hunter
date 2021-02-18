using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hati_Yuki : MobStatus
{
    private Animator _anim;
    float time = 0;
    float theta = 0;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        _anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(2 <= time && time <= 4)
        {
            _anim.SetBool("Attack", true);
        }
        else if(4 <= time)
        {
            time = 0;
        }
        else
        {
            _anim.SetBool("Attack", false);
        }

        time += Time.deltaTime;
        

    }
}
