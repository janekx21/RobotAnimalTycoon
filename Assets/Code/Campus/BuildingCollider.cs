using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingCollider : MonoBehaviour {

    public List<BuildingCollider> overlaping;
    public Building origen;

	// Use this for initialization
	void Awake () {
        overlaping = new List<BuildingCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        var bc = other.GetComponent<BuildingCollider>();
        if (bc) {
            if (bc.origen != origen)
                overlaping.Add(bc);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        var bc = other.GetComponent<BuildingCollider>();
        if (bc) {
            if(bc.origen != origen)
                overlaping.Remove(bc);
        }
    }


    void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject())
            origen.OnMouseDown();
    }

    


}
