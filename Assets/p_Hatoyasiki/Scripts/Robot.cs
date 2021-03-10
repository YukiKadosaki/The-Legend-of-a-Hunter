using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    void Start() {
        base.Start();
        MoveSpeed = 10f;
    }

    void Update() {
        StartCoroutine("MoveToDestination");
    }
}
