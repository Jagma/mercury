using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenGameObjectDataSynchonizer : MonoBehaviour, DataTokenChangeListener {
	public string token;
	
	public Component component=null;
	public string fieldName="";
	
	private FieldInfo fieldInfo;
	private PropertyInfo propertyInfo;
	
	void Start(){
		if (fieldName!=null){
			fieldInfo=component.GetType().GetField(fieldName);
			if (fieldInfo==null){
				propertyInfo=component.GetType().GetProperty(fieldName);
			}				
		}
		if (fieldInfo==null && propertyInfo==null){
			Debug.LogError(string.Format("Unable to find field/property '{0}' on component '{1}' on gameObject '{2}'",fieldName,component.GetType().Name,gameObject.name));
		}
		
		Refresh();
		// TODO: Verify token is correct format
		
		// Add as a listener
		Tokenizer.AddDataTokenChangeListener(this,token);
	}
	
	private List<TokenizerSource> FindAdditionalSources(){
		Transform source=transform;
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
	
	
	private void Refresh(){
		if (fieldInfo!=null){
			Transform value=Tokenizer.ReadDataToken<Transform>(token,FindAdditionalSources());
			fieldInfo.SetValue(component,value);
		}else if (propertyInfo!=null){
			Transform value=Tokenizer.ReadDataToken<Transform>(token,FindAdditionalSources());
			propertyInfo.SetValue(component,value,null);
		}
	}
	
	public void DataTokenChanged(string token,object newvalue){
		Refresh();
	}
	
	public void OnDestroy(){
		Tokenizer.RemoveDataTokenChangeListener(this,token);
	}
}
