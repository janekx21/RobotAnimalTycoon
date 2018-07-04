using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RobotBatch  {
    public Robot robot;
    public int size;

    public RobotBatch(Robot robot, int size) {
        this.robot = robot;
        this.size = size;
    }
}
