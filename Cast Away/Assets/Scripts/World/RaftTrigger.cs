using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftTrigger : MonoBehaviour {

    [SerializeField] Transform target;
    bool go = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        go = true;
        
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            Vector3.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);
        }
    }

}
