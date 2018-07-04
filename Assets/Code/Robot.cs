using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Robot  {
    public string nameOfPart;
    public int price;
    public float energyCost;
    public float bulkynis;
    public float sight;
    public float movement;
    public float intiligenz;
    public Robot(string name) {
        nameOfPart = name;
        price = 0;
        energyCost = 0;
        bulkynis = 0;
        sight = 0;
        movement = 0;
        intiligenz = 0;
    }

    public Robot(string nameOfPart, int price, float energyCost, float bulkynis,
                float sight, float movement, float intiligenz) : this(nameOfPart) {
        this.price = price;
        this.energyCost = energyCost;
        this.bulkynis = bulkynis;
        this.sight = sight;
        this.movement = movement;
        this.intiligenz = intiligenz;
    }

    public static Robot operator +(Robot a, Robot b) {
        var tmp = new Robot("new Robot");
        tmp.price = a.price + b.price;
        tmp.energyCost = a.energyCost + b.energyCost;
        tmp.bulkynis = a.bulkynis + b.bulkynis;
        tmp.sight = a.sight + b.sight;
        tmp.movement = a.movement + b.movement;
        tmp.intiligenz = a.intiligenz + b.intiligenz;
        return tmp;
    }
}
