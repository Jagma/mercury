using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenToggleDataSynchonizer : MonoBehaviour, DataTokenChangeListener{
	public string token;
	
	private Toggle toggle=null;
	
	void Start(){
		toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(ToggleValue);
		
		bool value=Tokenizer.ReadDataToken<bool>(token);
		if (value!=null){
			toggle.isOn=value;
		}
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	public void DataTokenChanged(string token,object newvalue){
		if (newvalue is bool && toggle.isOn!=(bool)newvalue){
			toggle.isOn=(bool)newvalue;
			toggle.onValueChanged.Invoke(toggle.isOn);
		}
	}
	
	void ToggleValue(bool newvalue){
		Tokenizer.StoreDataToken(token,newvalue);
	}	
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
