using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {

	public List<WayPoint> connected;
	public float maxDistance = 10f;

	private void Awake() {
		if (WaySystem.instance == null) WaySystem.instance = GameObject.Find("WaySystem").GetComponent<WaySystem>();
		WaySystem.instance.all.Add(this);
		connected = new List<WayPoint>();
	}

	private void OnDestroy() {
		if (WaySystem.instance.all != null)
			WaySystem.instance.all.Remove(this);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float dis = maxDistance;
		connected.Clear();
		foreach (var item in WaySystem.instance.all) {
			if (item == this) continue;
			float distance = (transform.position - item.transform.position).magnitude;
			if(distance < dis) {
				if (!connected.Contains(item))
					connected.Add(item);
			}
			
		}
	}

	public virtual Vector3 GetTo(WayPoint other) {
		float distance = (other.transform.position - transform.position).magnitude / 3;
		if (connected.Count == 2) {
			
			var a = other;
			int index = (connected.IndexOf(a) + 1) % 2;
			var b = connected[index];
			var c = (a.transform.position - transform.position).normalized;
			var d = (b.transform.position - transform.position).normalized;
			var e = ((c - d) * .5f) + d;
			return (e - d).normalized * distance + transform.position;
		}
		return (other.transform.position - transform.position).normalized * distance + transform.position;
	}



	public virtual void OnDrawGizmosSelected() {
		Gizmos.color = Color.white;
		if(connected != null)
			foreach (var item in connected) 
				Gizmos.DrawLine(transform.position, item.transform.position);
		Gizmos.color = new Color(1, 1, 1, .1f);
		Gizmos.DrawWireSphere(transform.position, maxDistance);

	}
}
