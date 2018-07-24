using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kajabity.Tools.Java;

public class Language : MonoBehaviour , TokenizerSource {
	
	public string language="eng";
	
	public static Language instance=null;
	
	private JavaProperties properties;
	
	public void Awake(){
		instance=this;
		GameObject.DontDestroyOnLoad(gameObject);
		Tokenizer.AddTokenizerSource(this);
	}
	
	private void load(){
		TextAsset languageText=Resources.Load("Languages/"+language) as TextAsset;
		Stream languageStream = new MemoryStream(languageText.bytes);
		
		properties=new JavaProperties();
		properties.Load(languageStream);
		languageStream.Close();
	}
	
	public void init(){
		if (properties==null){
			load();
		}
	}
	
	public string GetKey(){
		return "$";
	}
	
	public string GetTokenChangeKeyFromToken(string token){
		return token;
	}
		
	public string ProcessToken(string token,List<TokenizerSource> additionalSources=null){
		if (properties==null){
			load();
		}
		if (properties!=null && properties.ContainsKey(token)){
			return properties.GetProperty(token);
		}
		
		Debug.LogError("Unable to find Language token '"+token+"'");
		
		return "";
	}
	
/*	
	public string ProcessString(string text){
		// TODO: make this better
		int count=0;
		for (int t=text.IndexOf("${");t!=-1;t=text.IndexOf("${")){
			if (count++>100){
				Debug.LogError("More then 100 substritutions in text aborting. "+text);
				break;
			}
			int e=text.IndexOf("}",t);
			if (e!=-1){
				text=text.Substring(0,t)+GetString(text.Substring(t+2,e-t-2))+text.Substring(e+1);
			}else{
				// Remove start no match found and log
				Debug.LogWarning("Can't find close tag removing start. "+text);
				text=text.Substring(0,t)+text.Substring(t+2);
			}
		}
		
		return text;
	}*/

}
