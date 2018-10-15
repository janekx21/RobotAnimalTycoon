using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointer : WayPoint {
	public Vector3 direction = Vector3.forward;
	public override Vector3 GetTo(WayPoint other) {
		if (other.GetType() == typeof(WayPointer)) return Vector3.zero;
		return direction + transform.position;
	}

	public override void OnDrawGizmosSelected() {
		base.OnDrawGizmosSelected();
		Gizmos.DrawLine(transform.position, transform.position + direction);
	}
}
