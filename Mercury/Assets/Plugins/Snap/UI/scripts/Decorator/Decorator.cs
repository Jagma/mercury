using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : MonoBehaviour {
	public string style="default";
	public bool root=true;
	
	private DecoratorStyle decoratorStyle=null;
	
	public void Start(){
		if (root){
			ApplyStyle(new List<DecoratorStyle>());
		}
	}
	
	public void ApplyStyle(List<DecoratorStyle> parentStyles){
		decoratorStyle=DecoratorStyles.instance.GetStyle(style);
		if (decoratorStyle!=null){
			decoratorStyle.Decorate(this,gameObject,parentStyles);
		}
	}
}
