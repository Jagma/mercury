using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenImageDataSynchonizer : MonoBehaviour, DataTokenChangeListener {
	public string token;
	
	private Image image=null;
	
	void Start(){
		image = GetComponent<Image>();
		
		Refresh();
		// TODO: Verify token is correct format
		
		// Add as a listener
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	private void Refresh(){
		Sprite value=Tokenizer.ReadDataToken<Sprite>(token,Tokenizer.FindAdditionalSources(transform));
		image.sprite=value;
		image.enabled=value!=null;
	}
	
	public void DataTokenChanged(string token,object newvalue){
		Refresh();
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
