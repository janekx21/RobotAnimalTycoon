using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWay : Building {

	public override void OnPlace() {
		base.OnPlace();
		WaySystem.instance.UpdateModels();
	}

	public override void OnMoveEnd() {
		base.OnMoveEnd();
		WaySystem.instance.UpdateModels();
	}
}
