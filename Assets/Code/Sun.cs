using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public static float time = 0;
    public Transform rotator;

    public Light sun;
    public Light moon;

    public Gradient sunColor;
    public Gradient moonColor;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        var v = new Vector3(time * 360 / 60, 0, 0);
        rotator.localRotation = Quaternion.Euler(v);
        sun.color = sunColor.Evaluate(time / 60 % 1);
        moon.color = moonColor.Evaluate(time / 60 % 1);

    }
}
