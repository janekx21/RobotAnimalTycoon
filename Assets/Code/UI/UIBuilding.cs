using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuilding : HoverPanel {

    public Text bName;
    public Image bImage;

    Vector2 orginalPos;

	// Use this for initialization
	void Start () {
        bName.text = show.buildingName;
        orginalPos = transform.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = orginalPos + new Vector2(isOver ? 5:0, 0);

    }
}
