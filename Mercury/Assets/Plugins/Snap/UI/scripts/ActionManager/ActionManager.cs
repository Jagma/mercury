using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {
	
	public static ActionManager instance=null;
	
	private Dictionary<string,ActionSource> actionSources=new Dictionary<string,ActionSource>();
	
	public void Awake() {
        if (instance == null) {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else {
            GameObject.Destroy(gameObject);
        }
	}
	
	public void RegisterActions(ActionSource source){
		string key=source.GetSourceName();
		if (!actionSources.ContainsKey(key)){
			Debug.Log("Adding Action source '"+key+"'.");
			actionSources.Add(key,source);
		}else{
			Debug.LogError("Action source '"+key+"' duplicated.");
		}
	}
	
	public void DeRegisterActions(ActionSource source){
		string key=source.GetSourceName();
		if (actionSources.ContainsKey(key)){
			actionSources.Remove(key);
		}else{
			Debug.LogError("Action source '"+key+"' not found when trying to remove.");
		}
	}

    public void PerformActionAsync(string action, List<TokenizerSource> additionalSources = null) {
        StartCoroutine(PerformAction(action,additionalSources));
    }

    public IEnumerator PerformAction(string action, List<TokenizerSource> additionalSources=null){
		string[] actionParts=System.Text.RegularExpressions.Regex.Split(action,"\\.(?=[^\\]]*(?:\\[|$))");
		
		if (actionParts.Length==2){
			if (actionSources.ContainsKey(actionParts[0])){
                actionSources[actionParts[0]].ExecuteAction(actionParts[1], additionalSources);
			}else{
				Debug.LogWarning("Action source '"+actionParts[0]+"' unknown at this time '"+action+".");
			}
		}else{
			Debug.LogError("Action invalid '"+action+"'.");
		}
		yield return null;
	}
	
}
