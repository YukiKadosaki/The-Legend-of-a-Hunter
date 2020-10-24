using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dhurahan : Boss
{
    [SerializeField] private Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log("Start");
        StartCoroutine(MoveLiner(dest));
        Debug.Log("End");
        */

        

        Debug.Log("HP = " + Hp);
        Debug.Log("ATK = " + Atk);
        Debug.Log("MoveSpeed = " + MoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HP = " + Hp);
        Debug.Log("ATK = " + Atk);
        Debug.Log("MoveSpeed = " + MoveSpeed);
    }

    //目的地（destination）に障害物などを避けながら移動する 
    public override IEnumerator MoveToDestination(Vector3 destination)
    {
        yield return null;
    }
}
