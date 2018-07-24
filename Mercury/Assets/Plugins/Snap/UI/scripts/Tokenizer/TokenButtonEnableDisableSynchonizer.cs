using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenButtonEnableDisableSynchonizer : MonoBehaviour, DataTokenChangeListener {
	
	public bool conditionToEnable=true;
	public string token;
	
	public Button button;
	
	private bool refreshNeeded=true;
	//public bool buttonStatus=false;
	
	void Start(){
		if (button==null){
			button=GetComponent<Button>();
		}
		
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	public void Update(){
		if (refreshNeeded){
			Refresh();
			refreshNeeded=false;
		}
	}
	

	private void Refresh(){
		bool show=false;
		object value=Tokenizer.ReadDataToken<object>(token,Tokenizer.FindAdditionalSources(transform));
		//Debug.LogError(string.Format("{0}:Refresh {1}='{2}'",name,token,value));
		if (value is bool){
			show=(bool)value;
		} else if (value is int){
			show=((int)value)!=0;
		} else if (value is long){
			show=((long)value)!=0;
		} else if (value is float){
			show=((float)value)!=0;
		} else if (value is string){
			bool.TryParse((string)value, out show);
		} else {
			show=value!=null;
		}
		
		
		//buttonStatus=show;
		if (button!=null){
			button.interactable=(show==conditionToEnable);
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		//Refresh();
		refreshNeeded=true;
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
