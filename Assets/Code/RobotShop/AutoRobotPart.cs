using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRobotPart : RoboterPart {

	private void Awake() {
		Transform ports = null;
		foreach (Transform item in transform) {
			if (item.name.StartsWith("Ports")) ports = item;
		}
		if (ports == null) Debug.LogError("No Ports Child Found");
		List<Connector> tmp = new List<Connector>();
		foreach (Transform item in ports) {
			var state = RobotShop.State.X;
			if (item.name[0] == 'A') {
				state = RobotShop.State.A;
			}
			if (item.name[0] == 'B') {
				state = RobotShop.State.B;
			}
			if (item.name[0] == 'C') {
				state = RobotShop.State.C;
			}
			if (item.name[0] == 'X') {
				state = RobotShop.State.X;
			}
			tmp.Add(new Connector(Vector2Int.RoundToInt(item.position - ports.position), state));
		}
		connectionPoints = tmp.ToArray();
	}
}
