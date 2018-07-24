using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
 
public class CaretFix:MonoBehaviour, ISelectHandler{
	private bool alreadyFixed;
	public float up;
	public float right;
	
	public void OnSelect(BaseEventData eventData){
		if (!alreadyFixed){
			alreadyFixed = true;
			string nm = gameObject.name+" Input Caret";
			RectTransform caretRT = (RectTransform)transform.Find(nm);
			
			Vector2 anchoredPosition = caretRT.anchoredPosition;
			
			anchoredPosition.y = anchoredPosition.y + up;
			anchoredPosition.x = anchoredPosition.x + right;
			
			caretRT.anchoredPosition = anchoredPosition;
		}
	}
}