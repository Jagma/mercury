using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class ActionBehaviour : MonoBehaviour, ActionSource {
	
	public virtual string GetSourceName(){
		return GetType().Name;
	}
	
	public virtual void Start(){
		ActionManager.instance.RegisterActions(this);
	}
	
	public virtual void OnDestroy(){
		ActionManager.instance.DeRegisterActions(this);
	}
	
	public void ExecuteAction(string action, List<TokenizerSource> additionalSources=null){
		Debug.Log("Executing action:"+action);
		int startBracket=action.IndexOf('[');
		int endBracket=action.LastIndexOf(']');
		string param=null;
		if (startBracket<endBracket){
			param=action.Substring(startBracket+1,endBracket-startBracket-1);
			action=action.Substring(0,startBracket);
		}
		
		if (param==null){
			gameObject.BroadcastMessage(action,SendMessageOptions.RequireReceiver);
		}else{
			MethodBase method=this.GetType().GetMethod(action);
			
			if (method==null){
				Debug.LogError("Unable to find method '"+action+"' on "+this.name+"("+this.GetType().Name+")");
			}else{
				ParameterInfo[] parameters=method.GetParameters();
				if (parameters.Length!=1){
					Debug.LogError("Unable to call method '"+action+"' on "+this.name+"("+this.GetType().Name+") it has the wrong number of paramaters.");
				}else{
					try {
						if (parameters[0].ParameterType == typeof(string)){
							gameObject.BroadcastMessage(action,Tokenizer.ResolveTokens(param, additionalSources),SendMessageOptions.RequireReceiver);
						}else if (parameters[0].ParameterType == typeof(float)){
							gameObject.BroadcastMessage(action,float.Parse(Tokenizer.ResolveTokens(param, additionalSources)),SendMessageOptions.RequireReceiver);
						}else if (parameters[0].ParameterType == typeof(int)){
							gameObject.BroadcastMessage(action,int.Parse(Tokenizer.ResolveTokens(param, additionalSources)),SendMessageOptions.RequireReceiver);
						}else if (parameters[0].ParameterType == typeof(long)){
							gameObject.BroadcastMessage(action,long.Parse(Tokenizer.ResolveTokens(param, additionalSources)),SendMessageOptions.RequireReceiver);
						}else if (parameters[0].ParameterType == typeof(bool)){
							gameObject.BroadcastMessage(action,bool.Parse(Tokenizer.ResolveTokens(param, additionalSources)),SendMessageOptions.RequireReceiver);
						}else{
							object data=Tokenizer.ReadDataToken<object>(param, additionalSources);
							if (data!=null){
								if (data.GetType() == parameters[0].ParameterType){
									gameObject.BroadcastMessage(action,data,SendMessageOptions.RequireReceiver);
								}else{
									Debug.LogError("Unable to call method '"+action+"' on "+this.name+"("+this.GetType().Name+") param type "+parameters[0].ParameterType+" incompatable with "+data.GetType()+".");
								}
							}else{
								Debug.LogError("Unable to call method '"+action+"' on "+this.name+"("+this.GetType().Name+") param type "+parameters[0].ParameterType+" data '"+param+"' was null.");
							}
						}
					} catch (FormatException e){
						Debug.LogError("Unable to convert param '"+param+"' to a "+parameters[0].ParameterType+" on call to method '"+action+"' on "+this.name+"("+this.GetType().Name+").");
					}
				}
			}
		}
	}
	
}
