using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboterPart : MonoBehaviour {

    [System.Serializable]
    public struct Connector {
        public Vector2Int pos;
        public RobotShop.State state;
    }

    public Connector[] connectionPoints;
    public Vector2Int position;

    public bool placed = false;

    public bool ok;
    public bool wrong;

    public RobotShop myShop;


    public Robot stat;

    // Use this for initialization
    void Start () {
		
	}

    private void OnDestroy() {
        myShop.parts.Remove(this);
    }

    // Update is called once per frame
    void Update () {
        position = Vector2Int.RoundToInt(transform.position);
    }

    private void OnDrawGizmosSelected() {
        position = Vector2Int.RoundToInt(transform.position);
        foreach (var item in connectionPoints) {
            switch (item.state) {
                case RobotShop.State.A:
                Gizmos.color = Color.red;
                break;
                case RobotShop.State.B:
                Gizmos.color = Color.blue;
                break;
                case RobotShop.State.C:
                Gizmos.color = Color.green;
                break;
                default:
                Gizmos.color = Color.white;
                break;
            }
            Gizmos.DrawCube((Vector3)(Vector2)item.pos + transform.position, new Vector3(.1f,.1f,1f));
        }
    }
}
