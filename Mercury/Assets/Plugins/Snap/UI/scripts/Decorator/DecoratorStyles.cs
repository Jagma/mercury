using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoratorStyles : MonoBehaviour {
	
	public static DecoratorStyles instance=null;
	
	private Dictionary<string,DecoratorStyle> styles=new Dictionary<string, DecoratorStyle>();
	
	public void Awake () {
		instance=this;
		
		GameObject.DontDestroyOnLoad(gameObject.transform.parent);
		
		FindAll();
		
		gameObject.SetActive(false);
	}
	
	public DecoratorStyle GetStyle(string name){
		if (styles.ContainsKey(name)){
			return styles[name];
		}else{
			Debug.LogError("Unable to find style '"+name+"'.");
		}
		return null;
	}
	
	private void FindAll(){
		foreach (DecoratorStyle decoratorStyle in gameObject.GetComponentsInChildren<DecoratorStyle>(true)){
			AddStyle(decoratorStyle);
		}
	}
	
	private void AddStyle(DecoratorStyle style){
		string styleName=style.gameObject.name;
		if (!styles.ContainsKey(styleName)){
			if (style!=null){
				style.Init();
				styles.Add(styleName,style);
			}else{
				Debug.LogError("Unable to find style on child object called '"+styleName+"'.");
			}
		}else{
			Debug.LogError("Unable to add style '"+styleName+"' as it has already been defined.");
		}
	}
}
