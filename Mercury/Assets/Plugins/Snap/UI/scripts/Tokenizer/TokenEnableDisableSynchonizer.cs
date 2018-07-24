using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenEnableDisableSynchonizer : MonoBehaviour, DataTokenChangeListener {
	public string token;
	
	public GameObject[] groupToHideOrShow;
	
	void Start(){
		if (groupToHideOrShow==null || groupToHideOrShow.Length==0){
			groupToHideOrShow=new GameObject[]{gameObject};
		}
		
		Refresh();
		
		Tokenizer.AddDataTokenChangeListener(this,token);
		
		//foreach (string tokenChangeListened in Tokenizer.AddDataTokenChangeListener(this,token)){
		//	Debug.LogError(string.Format("{0}:listeneing to {1}",name,tokenChangeListened));
		//}
	}
	
	private void Refresh(){
		bool show=false;
		object value=Tokenizer.ReadDataToken<object>(token);
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
		
		foreach (GameObject go in groupToHideOrShow){
			go.SetActive(show);
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		Refresh();
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
