using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Decoration {

    public Light lamp;
    public Gradient color;

    public override void Start() {
        base.Start();
        updateColor();
    }


    public override void Update() {
        base.Update();
        updateColor();
    }


    void updateColor() {
        lamp.color = color.Evaluate(Sun.time / 60 % 1);
    }
}
