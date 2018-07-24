using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenInputFieldDataSynchonizer : MonoBehaviour, DataTokenChangeListener {
	public string token;
	
	private InputField inputField=null;
	
	void Start(){
		inputField = GetComponent<InputField>();
		inputField.onEndEdit.AddListener(TaskEndEdit);
		
		string value=Tokenizer.ReadDataToken<string>(token);
		if (value!=null){
			inputField.text=value;
		}
		// TODO: Verify token is correct format
		
		// Add as a listener
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	public void DataTokenChanged(string token,object newvalue){
		if (inputField.text!=newvalue as string){
			inputField.text=newvalue as string;
		}
	}
	
	void TaskEndEdit(string newvalue){
		Tokenizer.StoreDataToken(token,newvalue);
	}	
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
