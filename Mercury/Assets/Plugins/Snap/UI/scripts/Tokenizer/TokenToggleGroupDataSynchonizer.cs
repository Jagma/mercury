using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenToggleGroupDataSynchonizer : MonoBehaviour, DataTokenChangeListener {

	public string token;
	
	private Toggle[] toggles = null;
	
	void Start(){
		toggles = GetComponentsInChildren<Toggle>();
		
		foreach (Toggle toggle in toggles) {
			toggle.onValueChanged.AddListener(ToggleValue);
		}
		
		Refresh();
		
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	private void Refresh(){
		int value=Tokenizer.ReadDataToken<int>(token,Tokenizer.FindAdditionalSources(transform));
		for (int i = 0; i < toggles.Length; i++) {
			if (value is int && (int)value == i) {
				toggles[i].isOn = true;
			} else {
				toggles[i].isOn = false;
			}
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		Refresh();
	}
	
	void ToggleValue(bool newvalue){
		for (int i = 0; i < toggles.Length; i++) {
			if (toggles[i].isOn) {
				Tokenizer.StoreDataToken(token,i);
			}
		}
	}	
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
