using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_fMovementSpeed;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _rigidBody.velocity = input * m_fMovementSpeed;

        if (input.magnitude > 0)
        {
            this.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(input.x, input.y), Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(this.transform.position, Vector2.up);
            RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction, 500, 1 << 8);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.red, 100);
            if (hit2d)
            {
                BaseInteractable interactable;
                if ((interactable = hit2d.transform.gameObject.GetComponent<BaseInteractable>()))
                {
                    interactable.Interact();
                    if (interactable is ResourceInteractable)
                    {
                        ResourceInteractable resource = (ResourceInteractable)interactable;
                        Debug.Log(resource.ResourceType);
                        Debug.Log(resource.HarvestAmount);
                    }
                }
            }
        }
    }

}
