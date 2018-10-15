using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaySystem : MonoBehaviour {

	public static WaySystem instance;
	public List<WayPoint> all;
	public Way way;
	public GameObject round;

	private void Awake() {
		all = new List<WayPoint>();
		instance = this;
	}

	public void UpdateModels() {
		List<WayPoint> starter = new List<WayPoint>();
		List<WayPoint> ender = new List<WayPoint>();
		foreach (var item in all) {
			foreach (var connection in item.connected) {
				bool can = true;
				for (int i = 0;i<ender.Count;i++) {
					if(ender[i] == item) {
						if(starter[i] == connection) {
							can = false;
						}
					}
				}
				if (can) {
					starter.Add(item);
					ender.Add(connection);
				}
			}
		}
		foreach (Transform item in transform) {
			Destroy(item.gameObject);
		}
		for (int i = 0; i < starter.Count; i++) {
			//	print(string.Format("{0}->{1}",starter[i],ender[i]));
			Vector3 directionA = starter[i].GetTo(ender[i]);
			Vector3 directionB = ender[i].GetTo(starter[i]);
			if (directionA == Vector3.zero || directionB == Vector3.zero) {
				continue;
			}
			var w = Instantiate<Way>(way, transform);
			w.posA = starter[i].transform.position;
			w.dirA = directionA;
			w.posB = ender[i].transform.position;
			w.dirB = directionB;
			w.GenerateTransforms();
			w.UpdateModel();
		}
		foreach (var item in all) {
			if(item.connected.Count > 2 && item.GetType() != typeof(WayPointer)) {
				/*
				foreach (var firstem in item.connected) {
					foreach (var totem in item.connected) {
						if (totem == firstem) continue;
						float angle = Vector3.SignedAngle(totem.transform.position - item.transform.position,
						firstem.transform.position - item.transform.position, Vector3.up);
						if(angle > 180) {
							print("Here PLS");
						}
						print(angle);
					}
				}
				*/
				Instantiate(round, item.transform.position, Quaternion.identity, transform);
			}
		}
	}

	// Use this for initialization
	void Start () {
		UpdateModels();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F1)) {
			UpdateModels();
		}
	}
}
