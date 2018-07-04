using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerStats : MonoBehaviour {

    public Text money;
    public Image rating;
    Manager man;

	// Use this for initialization
	void Start () {
        man = Manager.instance;

    }
	
	// Update is called once per frame
	void Update () {
        money.text = man.stat.money.ToString();
        rating.fillAmount = man.stat.reputation * .5f +.5f;

    }
}
