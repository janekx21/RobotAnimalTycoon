using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoboPartStat : MonoBehaviour {

    public RobotShop target;

    public Text[] texts;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var s = target.robotStat;
        object[] vars = { s.nameOfPart,s.price,s.energyCost,s.bulkynis,s.movement,s.sight,s.intiligenz};

        for (int i = 0; i < vars.Length; i++) {
            texts[i].text = vars[i].ToString();
        }

    }
}
