using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaftTrigger : MonoBehaviour
{

    [SerializeField] Transform target;
    bool go = false;

    PlayerController _playerController;
    void OnTriggerEnter2D(Collider2D col)
    {
        go = true;

        if (!_playerController)
        {
            if (col.tag == "Player")
            {
                _playerController = col.gameObject.GetComponent<PlayerController>();
            }
        }
        if (_playerController)
        {
            _playerController.ChangeState(PlayerState.DISABLED, Vector3.zero);
        }

    }

    private void Update()
    {
        if (go)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.001f)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, 5f * Time.deltaTime);
                this.transform.position = newPos;
                if (_playerController)
                {
                    _playerController.transform.position = newPos;
                }
            }
            else
            {
                _playerController.ChangeState(PlayerState.NORMAL, Vector3.zero);
            }
        }
    }

}
