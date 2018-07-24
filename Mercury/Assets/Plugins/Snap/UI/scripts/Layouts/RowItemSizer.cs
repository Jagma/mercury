using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowItemSizer : MonoBehaviour {
	
	public List<RectTransform> children = new List<RectTransform>();
	private List<float> weights = new List<float>();
	private float totalWeightWidth=0;
	private float lastWidth = 0;
	
	void Start () {
		foreach (RectTransform child in children) {
			weights.Add(child.sizeDelta.x);
			totalWeightWidth+=child.sizeDelta.x;
		}
	}
	
	void Update() {
		float totalWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
		if (lastWidth!=totalWidth){
			lastWidth=totalWidth;
			for (int i = 0; i < children.Count; i++) {
				Vector2 size = children[i].sizeDelta;
				size.x = totalWidth*weights[i]/totalWeightWidth;
				children[i].sizeDelta = size;
			}
		}
	}
}
