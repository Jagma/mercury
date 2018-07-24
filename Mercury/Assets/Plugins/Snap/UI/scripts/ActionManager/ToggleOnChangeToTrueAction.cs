using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOnChangeToTrueAction : MonoBehaviour {

	public string action="";
	public float disableAfterClickSeconds=0;
	
	private Toggle toggle;
	
	void Start(){
		toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(TaskOnClick);
	}
	
	void TaskOnClick(bool active){
		if (active) {
			if (disableAfterClickSeconds>0){
				toggle.interactable=false;
			}
			StartCoroutine(PerformAction());
		}
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
			toggle.interactable=true;
		}
	}
	
	void OnDisable () {
		toggle.interactable = true;
	}
}
