using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenSliderDataSynchonizer : MonoBehaviour, DataTokenChangeListener{
	public string token;
	
	private Slider slider=null;
	
	void Start(){
		slider = GetComponent<Slider>();
		slider.onValueChanged.AddListener(SliderValue);
		
		Refresh();
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	private void Refresh(){
		float value=Tokenizer.ReadDataToken<float>(token);
		if (value!=null){
			slider.value=value;
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		Refresh();
	}
	
	void SliderValue(float newvalue){
		Tokenizer.StoreDataToken(token,newvalue);
	}	
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
