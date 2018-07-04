using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Prototype : Building {

    public Robot robotStat;

    public float cooldown = 0;
    public float maxCooldown = 60;

    public bool canPrototype = true;

    Image timerImage;
    Button button;

    public AnimationCurve researcherBonus;

    public void makeARobotHere() {
        if (canPrototype) {
            Manager.instance.MakeARobot(this);
            canPrototype = false;
            
        }

    }

    public void setNewRobotStats(Robot stat) {
        robotStat = stat;
        cooldown = maxCooldown;
        Manager.instance.stat.protoTypeStorage.Add(stat);
    }

    public override void setUp(ref List<UIRef> setting) {
        base.setUp(ref setting);
        var a = UIRef.Get<Button>(ref setting, "robotMakeButton");
        if (a != null) {
            a.onClick.AddListener(makeARobotHere);
            button = a;
        }
        var b = UIRef.Get<Image>(ref setting, "robotMakingTimer");
        if (b)
            timerImage = b;
    }

    public override void Tick() {
        base.Tick();
        //Manager.instance.stat.money -= robotStat.price;
        //float score = robotStat.sight * 2f + robotStat.movement * 1f + robotStat.intiligenz * 3f;
        //Manager.instance.stat.reputation += score * .01f;
    }

    public override void Update() {
        base.Update();
        canPrototype = cooldown<=0;
        if (cooldown > 0) cooldown -= Time.deltaTime * researcherBonus.Evaluate(Manager.instance.stat.researcher);
        if (cooldown < 0) {
            cooldown = 0;
            canPrototype = true;
        }
        if (Manager.instance.stat.researcher == 0) canPrototype = false;
        if (timerImage)
            timerImage.fillAmount = 1-cooldown / maxCooldown;
        if(button)
            button.interactable = canPrototype;
    }
}
