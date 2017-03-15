using UnityEngine;
using System.Collections;

public class ColliderCheck : MonoBehaviour
{
    public LayerMask _layerCheck;

    public bool IsColliding
    {
        get;
        private set;
    }

    public Collider2D OtherCollider
    {
        get;
        private set;
    }
	
	void FixedUpdate()
    {
        IsColliding = false;
        OtherCollider = null;
	}

    void Update()
    {
        if (OtherCollider == null)
        {
            IsColliding = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleTrigger(collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        HandleTrigger(collider);
    }

    void HandleTrigger(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & _layerCheck) != 0)
        {
            IsColliding = true;
            OtherCollider = collider;
        }
    }

    void OnDrawGizmos()
    {
        if (IsColliding && OtherCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, OtherCollider.transform.position);
        }
    }
}
