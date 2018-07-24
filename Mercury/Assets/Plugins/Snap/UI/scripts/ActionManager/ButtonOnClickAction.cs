using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClickAction : MonoBehaviour {
	
	public string action="";
	public float disableAfterClickSeconds=0;
	
	private Button button;
	
	void Start(){
		button = GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);
	}
	
	void TaskOnClick(){
		if (disableAfterClickSeconds>0){
			button.interactable=false;
		}
		StartCoroutine(PerformAction());
	}
	
	private List<TokenizerSource> FindAdditionalSources(Transform source){
		List<TokenizerSource> additionalSources=null;
		while (source!=null){
			TokenizerSource tokenizerSource=source.GetComponent<TokenizerSource>();
			if (tokenizerSource!=null){
				if (additionalSources==null){
					additionalSources=new List<TokenizerSource>();
				}
				
				additionalSources.Add(tokenizerSource);
			}
			source=source.parent;
		}
		return additionalSources;
	}
	
	private IEnumerator PerformAction(){
		// Populate action if needed
		//string newAction=Tokenizer.ReadDataToken ResolveTokens(action,FindAdditionalSources(transform));
		
		float started=Time.time;
		yield return StartCoroutine(ActionManager.instance.PerformAction(action,FindAdditionalSources(transform)));
		
		float timeUsed=Time.time-started;
		if (timeUsed<disableAfterClickSeconds && disableAfterClickSeconds>0){
			yield return new WaitForSeconds(disableAfterClickSeconds-timeUsed);
			button.interactable=true;
		}
	}
	
	void OnDisable () {
		if (button != null)
			button.interactable = true;
	}
}
