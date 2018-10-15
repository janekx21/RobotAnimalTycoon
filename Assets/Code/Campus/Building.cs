using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour {

    public string buildingName = "";
    public int price;
    public float reputationInc = 0;

    public BuildingCollider[] colliders;
    public bool placed = true;

    public bool canPlace = false;

    public GameObject repr;

    public UISettings settingsPrefab;

    public bool bonus;

    public void Awake() {
        foreach (var item in colliders) {
            item.origen = this;
        }
		if (!placed)
			repr.layer = LayerMask.NameToLayer("BuildingFront");
		else
			OnPlace();

	}

    public virtual void OnPlace() {
        repr.layer = LayerMask.NameToLayer("Buildings");
        repr.GetComponent<Renderer>().material.color = Color.white;
    }

    

    UISettings settings;

    public virtual void Start () {
        if (settingsPrefab) {
            settings = Instantiate<UISettings>(settingsPrefab, Manager.instance.selectMaker.transform);
            settings.gameObject.SetActive(false);
            settings.origen = this;
            var t = settings.GetComponent<RectTransform>();
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }

        Manager.instance.stat.reputation += reputationInc;
    }

    bool updateBonus() {
        return true;
    }

    bool movingWithMouse = false;
    Vector3 moveOffset;
    Vector3 oldPosition;
    public virtual void Update () {
        bonus = updateBonus();
        if (!placed) {
            repr.GetComponent<Renderer>().material.color = canPlace ? Color.white : Color.red;
        }
        else {
            if(movingWithMouse) {
                transform.position = Manager.instance.lastMousePos - moveOffset;
                bool can = true;
                foreach (var item in colliders) {
                    if (item.overlaping.Count > 0)
                        can = false;
                }

                if (Input.GetMouseButtonUp(0)) {
                    if (!can) {
                        transform.position = oldPosition;
                        can = true;
                    }
                    movingWithMouse = false;
                    repr.layer = LayerMask.NameToLayer("Buildings");
					OnMoveEnd();

				}
                repr.GetComponent<Renderer>().material.color = can ? Color.cyan : Color.red;
            }
        }
        
    }

	public virtual void OnMoveBegin() { }
	public virtual void OnMoveEnd() { }

    public virtual void Tick() { }

    public virtual void setUp(ref List<UIRef> setting) { }

    public void OnMouseDown() {
        
        if (selected) {
            oldPosition = transform.position;
            movingWithMouse = true;
            moveOffset = Manager.instance.lastMousePos - transform.position;
            repr.layer = LayerMask.NameToLayer("BuildingFront");
			OnMoveBegin();
		}
        Manager.instance.selected = this;
    }

    public bool selected = false;

    public void changeSelectionState(bool selected) {
        this.selected = selected;
        if(settings)
            settings.gameObject.SetActive(selected);

        foreach (var item in GetComponentsInChildren<Renderer>())
            item.material.color = selected?Color.cyan: Color.white;

    }

    public void OnDestroy() {
        if(settings)
            Destroy(settings.gameObject);
    }



}
