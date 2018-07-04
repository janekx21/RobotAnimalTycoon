using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Building show;


    public bool isOver;

    public void OnPointerEnter(PointerEventData eventData) {

        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData) {

        isOver = false;
    }
}
