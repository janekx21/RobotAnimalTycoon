using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class UIRef {
    public string name;
    public GameObject element;
    public static T Get<T>(ref List<UIRef> list,string name)where T:class {// where T:class
        foreach (var item in list) {

            if (item.name == name) {
                try {
                    return item.element.GetComponent<T>();
                }
                catch {
                    return null;
                }
            }
        }
        return null;
    }
}


public class UISettings : MonoBehaviour {

    public Building origen;

    [SerializeField]
    public List<UIRef> elements;
    

	// Use this for initialization
	void Start () {

        if (elements == null) elements = new List<UIRef>();
        origen.setUp(ref elements);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
