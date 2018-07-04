using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILetftPanel : HoverPanel {

    Vector2 orginalPos;
    Vector2 target;

    public float comeOut = 40;

    private void Start() {
        orginalPos = transform.localPosition;
        target = Vector2.zero;
    }

    private void Update() {
        var to = new Vector2(isOver ? comeOut : 0, 0);
        target = Vector2.Lerp(target, to, .1f);
        transform.localPosition = orginalPos + target;
    }

}
