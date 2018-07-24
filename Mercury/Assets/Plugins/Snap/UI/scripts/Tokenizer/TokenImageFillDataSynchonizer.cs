using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenImageFillDataSynchonizer : MonoBehaviour, DataTokenChangeListener {
	public string token;
	
	private Image image=null;
	
	void Start(){
		image = GetComponent<Image>();
		
		float value=Tokenizer.ReadDataToken<float>(token);
		if (value!=null){
			image.fillAmount=value;
		}
		// TODO: Verify token is correct format
		
		// Add as a listener
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	public void DataTokenChanged(string token,object newvalue){
		if (image.fillAmount!=(float)newvalue){
			image.fillAmount=(float)newvalue;
		}
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
