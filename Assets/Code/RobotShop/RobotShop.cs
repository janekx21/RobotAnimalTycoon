using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotShop : MonoBehaviour {

    public const int width = 20;
    public const int height = 12;

    public enum State {
        N,//Not A Block Set
        A,//Type A (front)
        B,//Type B (back)
        C,//Type C (overhang)
        X//cant be used
    }

    State[] placed = new State[width*height];

    public List<RoboterPart> parts;

    public Robot robotStat;

    public RoboterPart current = null;
    int currentIndex = 0;

    public RoboterPart[] premade;

    public Prototype ownBuilding;

    public Button submit;
    public Text partName;

    // Use this for initialization
    void Start () { 
        if(parts == null)
            parts = new List<RoboterPart>();
        placed = new State[width * height];
        for (int i = 0; i < placed.Length; i++) {
            placed[i] = State.N;
        }
        newPartToMouse(currentIndex);

        submit.onClick.AddListener(FinishEdit);
    }

    void newPartToMouse(int index) {
        if (current) Destroy(current.gameObject);
        current = Instantiate<RoboterPart>(premade[index], (Vector2)getCurrentMouseOver(), Quaternion.identity, transform);
        current.myShop = this;
        parts.Add(current);
    }

    Vector2 lastMouseOver;

    Vector2 getCurrentMouseOver() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("RayCastTarget"))) {
            lastMouseOver = hit.point;
        }
        return lastMouseOver;
    }
	
	// Update is called once per frame
	void Update () {

        updatePlaced();

        robotStat = culcStat();
        robotStat.nameOfPart = partName.text;

        //12 max name length
        submit.interactable = parts.Count > 2 && partName.text!="" && partName.text.Length<=12;

       


        if (current) {
            var item = current;
            var woultBeX = new bool[width * height];
            //System.Array.Clear(woultBeX, 0, woultBeX.Length);
            if (!item.placed) {
                var b = getAllPointsOfPart(item);
                item.ok = false;
                item.wrong = false;

                

                for (int i = 0; i < b.Length; i++) {
                    if (!overLayIsOk(placed[i], b[i])) {
                        item.wrong = true;
                        break;
                    }
                    else
                        if (overLayIsFine(placed[i], b[i])) {
                            item.ok = true;
                            woultBeX[i] = true;
                        }
                }

                foreach (var child in item.GetComponentsInChildren<Renderer>()) {
                    child.material.color = item.wrong ? Color.red : (item.ok ? Color.green : Color.white);
                }

            }

            current.transform.position = (Vector2)getCurrentMouseOver();
            int old = currentIndex;
            if (Input.GetAxis("Mouse ScrollWheel") > 0) currentIndex++;
            if (Input.GetAxis("Mouse ScrollWheel") < 0) currentIndex--;


            if (currentIndex > premade.Length - 1) currentIndex = 0;
            if (currentIndex < 0) currentIndex = premade.Length - 1;
            if (old != currentIndex) newPartToMouse(currentIndex);

            if (current.ok && !current.wrong)
                if (Input.GetMouseButtonDown(0)) {

                    for (int i = 0; i < woultBeX.Length ; i++) {
                        if (woultBeX[i]) placed[i] = State.X;
                    }
                    current.transform.position = (Vector2) Vector2Int.RoundToInt(getCurrentMouseOver());

                    foreach (var child in current.GetComponentsInChildren<Renderer>())
                         child.material.color = Color.white;

                    current.placed = true;

                    current = null;
                    newPartToMouse(currentIndex);
                }
        }
    }


    //is the over lay ok as it is?
    bool overLayIsOk(State a,State b) {

        if (a == State.A && b == State.A) return false;
        if (a == State.B && b == State.B) return false;
        if (a == State.C && b == State.C) return false;

        if (a == State.X && b == State.A) return false;
        if (a == State.X && b == State.B) return false;
        if (a == State.X && b == State.C) return false;

        if (a == State.X && b == State.X) return false;

        if (a == State.A && b == State.X) return false;
        if (a == State.B && b == State.X) return false;
        if (a == State.C && b == State.X) return false;

        return true;
    }

    //can the part be placed?
    bool overLayIsFine(State a, State b) {
        if (a == State.A && b == State.B) return true;
        if (a == State.B && b == State.A) return true;
        return false;
    }

    private void OnDrawGizmos() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                switch(placed[i + j * width]) {
                    case State.A: Gizmos.color = Color.red;
                    break;
                    case State.B:
                    Gizmos.color = Color.blue;
                    break;
                    case State.C:
                    Gizmos.color = Color.green;
                    break;
                    case State.X:
                    Gizmos.color = Color.black;
                    break;
                    default: Gizmos.color = Color.white;
                    break;
                }
                Gizmos.DrawSphere(new Vector3(i, j, 0), .5f);
            }
        }
    }
    

    void updatePlaced() {
        System.Array.Clear(placed, 0, placed.Length);

        foreach (var item in parts) {
            if (item.placed) {
                var b = getAllPointsOfPart(item);
                for (int i = 0; i < b.Length; i++) {
                    if (overLayIsFine(placed[i], b[i])) {
                        placed[i] = State.X;
                        continue;
                    }
                    if (b[i] != State.N) {
                        placed[i] = b[i];
                        continue;
                    }
                }
            }
        }
    }

    Robot culcStat() {
        Robot stat = new Robot("unnamed Robot");
        foreach (var item in parts) {
            stat += item.stat;
        }
        return stat;
    }

    State[] getAllPointsOfPart(RoboterPart part) {
        var b = new State[width * height];
        System.Array.Clear(b, 0, b.Length);
        foreach (var point in part.connectionPoints) {
            var pos = point.pos + part.position;
            int i = Mathf.FloorToInt(pos.x);
            int j = Mathf.FloorToInt(pos.y);
            if (i >= 0 && i < width && j >= 0 && j < height) {

                b[i + j * width] = point.state;
            }
        }
        return b;
    }

    public void FinishEdit() {
        if (current) {
            parts.Remove(current);
            current = null;
        }
        ownBuilding.setNewRobotStats(robotStat);
        Manager.instance.finishRobotMaking();
    }
    public void Reset() {

    }
}
