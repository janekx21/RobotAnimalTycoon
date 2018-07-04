using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class School : Building {

    float timer = 0;
    public float timeForNew = 60;
    Image timerImage;

    public override void Start() {
        base.Start();

    }

    public override void Update() {
        base.Update();
        if (Manager.instance.stat.students > 0) {
            timer += Time.deltaTime;
            if(timer > timeForNew) {
            
                    newResearcher();
                    timer = 0;
            }
        }

        if(timerImage!=null)
            timerImage.fillAmount = Mathf.Clamp01(timer / timeForNew);

    }

    public override void setUp(ref List<UIRef> setting) {
        base.setUp(ref setting);
        var b = UIRef.Get<Image>(ref setting, "schoolTimer");
        if (b)
            timerImage = b;
    }

        public void newResearcher() {
        Manager.instance.stat.students--;
        Manager.instance.stat.researcher++;
    }
}
