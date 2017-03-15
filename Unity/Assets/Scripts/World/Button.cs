using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public GameObject target;

    public Sprite down;
    public Sprite up;

    private ColliderCheck colliderCheck;
    private SpriteRenderer spriteRenderer;

	void Awake () {
		colliderCheck = GetComponent<ColliderCheck> ();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	void Step() {
		target.BroadcastMessage("SetToggle", colliderCheck.IsColliding);
        spriteRenderer.sprite = (colliderCheck.IsColliding) ? down : up;
	}

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    private void Restart()
    {
        target.BroadcastMessage("SetToggle", false);
        spriteRenderer.sprite = up;
    }
}
