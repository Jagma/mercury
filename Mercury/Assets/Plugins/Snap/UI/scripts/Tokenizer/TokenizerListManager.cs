using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenizerListManager : MonoBehaviour,DataTokenChangeListener {
	
	public GameObject itemBaseObject;
	public string primaryListToken;
	public string tokenSetType="#";
	public string tokenName="index";
	public int maxItemsToDisplay=500;
	
	public List<GameObject> currentList=new List<GameObject>();
	
	public void Start(){
		itemBaseObject.SetActive(false);
		
		Tokenizer.AddDataTokenChangeListener(this,primaryListToken,Tokenizer.FindAdditionalSources(transform));
		if (Tokenizer.ReadDataToken<object>(primaryListToken)!=null){
			RebuildList();
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		if (newvalue!=null){
			RebuildList();
		}
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this);
	}
	
	public void RebuildList(){
		int size=Tokenizer.ReadDataToken<int>(primaryListToken.Substring(0,primaryListToken.Length-1)+"[Count]}");
		
		if (size>=maxItemsToDisplay){
			size=maxItemsToDisplay-1;
		}
		if (size<0){
			size=0;
		}
		
		while (size<currentList.Count){
			GameObject.Destroy(currentList[currentList.Count-1]);
			currentList.RemoveAt(currentList.Count-1);
		}
		while (size>currentList.Count){
			GameObject go=GameObject.Instantiate(itemBaseObject,transform,false);
			foreach (TokenIgnoreAllChildren tokenIgnoreAllChildren in go.GetComponentsInChildren<TokenIgnoreAllChildren>()){
				DestroyImmediate(tokenIgnoreAllChildren);
			}
			currentList.Add(go);
			// Add index
			TokenIndexSource tokenIndexSource=go.AddComponent<TokenIndexSource>();
			tokenIndexSource.tokenName=tokenName;
			tokenIndexSource.tokenSetType=tokenSetType;
			tokenIndexSource.index=""+(currentList.Count-1);
			
			go.AddComponent<TokenizerFiller>();
			
			go.SetActive(true);
		}
	}
}
