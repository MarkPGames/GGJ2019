using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    private Camera _camera;

    private float speed = 5f;

    private void Start()
    {
        _camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player)
        {
            Vector2 playerScreenPosition = _camera.WorldToScreenPoint(_player.transform.position);

            //if (Mathf.Abs(playerScreenPosition.magnitude) >= Mathf.Abs(new Vector2(Screen.width, Screen.height).magnitude))
            //{
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, speed);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -50);
            //}
        }
    }
}
