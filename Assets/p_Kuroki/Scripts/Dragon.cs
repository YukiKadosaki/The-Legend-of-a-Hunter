using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Boss
{
    [SerializeField] private Vector3 dest;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        StartCoroutine(MoveLiner(dest));
        Debug.Log("End");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IEnumerator MoveToDestination (Vector3 destination)
    {
        yield return null;
    }
}
