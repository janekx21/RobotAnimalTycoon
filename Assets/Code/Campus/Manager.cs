using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public Building[] allBuildings;

    Building currentOnMouse = null;
    public List<Building> buildings;

    UIBuilding[] uis;
    public UISelectedMaker selectMaker;
    public UILetftPanel lPanel;
    public UIBuilding lPanelPrefab;

 //   public Transform leftPanel;

    public static Manager instance;

    public Camera cam;

    public AnimationCurve repToStudentAdd;



    [System.Serializable]
    public class Stat {
        public int money = 0;
        public float reputation = 0;//neg and pos
        public int livingRoom = 0;
        public int students = 0;
        public int researcher = 0;
        public int storingRoom = 0;
        public int productionCap = 0;
        public List<Robot> protoTypeStorage;
        public List<RobotBatch> inStorage;//TODO ersetzen durch neue klasse

        public Stat() {
            protoTypeStorage = new List<Robot>();
        }

        public void culcRoom(Building[] arr) {
            storingRoom = 0;
            livingRoom = 0;
            foreach (var item in arr) {
                if (item.GetType() == typeof(Storage)) {
                    storingRoom += ((Storage)item).capacity;
                }
                else
                if (item.GetType() == typeof(House)) {
                    livingRoom += ((House)item).capacity;
                }
            }
        }
    }


    public Stat stat;


    public Building selected {
        get { return _selected; }
        set {
            if (_selected)
                _selected.changeSelectionState(false);
                
            _selected = value;
            if(_selected)
                _selected.changeSelectionState(true);

        }
    }

    bool robotMaking = false;

    Building _selected;

    private void Awake() {
        if (instance) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        buildings = new List<Building>();
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        if (stat == null) stat = new Stat();
        stat.money = 5550; // k
        float y = 0;
        uis = new UIBuilding[allBuildings.Length];
        for (int i = 0; i < allBuildings.Length; i++) {
            var item = allBuildings[i];
            var b = Instantiate<UIBuilding>(lPanelPrefab,
                Vector3.zero,
                Quaternion.identity, lPanel.transform);
            b.transform.localPosition = new Vector3(0, y, 0);
            b.transform.localRotation = Quaternion.identity;
            y -= b.GetComponent<RectTransform>().sizeDelta.y;
            uis[i] = b;
            b.show = item;
            
        }
    }

    float dayTimer = 0;
    int days = 0;
    int weeks = 0;
    int month = 0;

    public void DayGoesFlip() {
        days++;
        if (days % 7 == 0) weeks++;
        if (days % 30 == 0) month++;

        addStudent(Mathf.CeilToInt(repToStudentAdd.Evaluate(stat.reputation)));
    }

    void addStudent() {
        if (stat.students < stat.livingRoom) stat.students++;
    }
    void addStudent(int num) {
        if (stat.students + num < stat.livingRoom) stat.students += num;
        else stat.students = stat.livingRoom;
    }

    public Vector3 lastMousePos;
	// Update is called once per frame
	void Update () {

        dayTimer += Time.deltaTime;

        if (dayTimer > 60f) { // 1min real time = 1 tag
            DayGoesFlip();
            dayTimer = 0;
        }


        if (!robotMaking) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground"))) {
                if (currentOnMouse)
                    currentOnMouse.transform.position = hit.point;
                lastMousePos = hit.point;
            }

            if (currentOnMouse) {
                currentOnMouse.canPlace = canPlace();

                if (Input.GetMouseButtonUp(0)) {

                    if (currentOnMouse.canPlace) {
                        currentOnMouse.placed = true;
                        currentOnMouse.OnPlace();
                        stat.money -= currentOnMouse.price;
                        buildings.Add(currentOnMouse);
                        currentOnMouse = null;
 
                        stat.culcRoom(buildings.ToArray());
                    }
                    else {
                        Destroy(currentOnMouse.gameObject);
                        currentOnMouse = null;
                    }

                }
            }
            else {
                if (Input.GetMouseButtonDown(0)) {
                    foreach (var item in uis) {
                        if (item.isOver) {
                            newBuildingOnMouse(item.show);
                            break;
                        }
                    }
                }
            }
        }
	}

    void newBuildingOnMouse(Building building) {
        currentOnMouse = Instantiate<Building>(building, getOnGroundPosition(), Quaternion.identity);
    }

    Vector3 lastPointer;

    Vector3 getOnGroundPosition() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground"))) {
            lastPointer = hit.point;
           return hit.point;
        }
        else {
            return lastPointer;
        }
    }

    bool canPlace() {
        bool can = true;
        foreach (var item in currentOnMouse.colliders) {
            if (item.overlaping.Count > 0)
                can = false;
        }
        foreach (var item in uis) {
            if (item.isOver) return false;
        }
        if (currentOnMouse.price > stat.money) return false;
        return can;
    }

    RobotShop shop = null;

    public void MakeARobot(Building building) {
        if (!robotMaking) {
            if(selected)
                StartCoroutine(LoadYourAsyncScene(building));
            else {
                print("whitch building?");
            }
        }
    }


    public void finishRobotMaking() {
        if (robotMaking) {
            shop = null;
            SceneManager.UnloadSceneAsync("RobotShop");
            cam.gameObject.SetActive(true);
            robotMaking = false;
        }
    }

    IEnumerator LoadYourAsyncScene(Building building) {
        var asyncLoad = SceneManager.LoadSceneAsync("RobotShop", LoadSceneMode.Additive);
        
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        shop = GameObject.FindObjectOfType<RobotShop>();
        shop.ownBuilding = (Prototype) building;
        cam.gameObject.SetActive(false);
        robotMaking = true;
    }


}
