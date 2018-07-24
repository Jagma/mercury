using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Tokenizer {
	
	static private Dictionary<string,TokenizerSource> sources=new Dictionary<string, TokenizerSource>();

    static private Dictionary<string,TokenizerDataStore> dataStores=new Dictionary<string, TokenizerDataStore>();
	static private Dictionary<string,Dictionary<string,List<DataTokenChangeListener>>> dataTokenListeners=new Dictionary<string,Dictionary<string,List<DataTokenChangeListener>>>();
	
	static public void AddTokenizerSource(TokenizerSource source){
		// Add default functions
		if (sources.Count==0){
			TokenizerFunctions tokenizerFunctions=new TokenizerFunctions();
			sources.Add(tokenizerFunctions.GetKey(),tokenizerFunctions);
		}
		
		if (sources.ContainsKey(source.GetKey())){
			Debug.LogError("Unable to add a second Tokenizer for the same Key '"+source.GetKey()+"'");
		}else{
			sources.Add(source.GetKey(),source);
			if (source is TokenizerDataStore){
				dataStores.Add(source.GetKey(),(TokenizerDataStore)source);
			}
		}
	}
	
	static public void StoreDataToken(string text, object value){
		string key=text.Substring(0,1);
		string term=text.Substring(2,text.Length-3);
		string resolved = ResolveTokens(term);
		
		if (dataStores.ContainsKey(key)){
			dataStores[key].SetData(resolved,value);
		}else{
			Debug.LogError("Unable to find datastore for '"+key+"{");
		}
	}
	
	static public oftype ReadDataToken<oftype>(string text, List<TokenizerSource> additionalSources=null){
		string key=text.Substring(0,1);
		string term=text.Substring(2,text.Length-3);
		
		if (dataStores.ContainsKey(key)){
			term=ResolveTokens(term,additionalSources);
			return dataStores[key].GetData<oftype>(term,additionalSources);
		}else if (sources.ContainsKey(key)){
			return (oftype)(object)ResolveTokens(text,additionalSources);
		}else{
			Debug.LogError("Unable to find datastore for '"+key+"{");
		}
		return default(oftype);
	}
	
	static List<string> FindAllTokens(string text,List<TokenizerSource> additonalTokenSources=null){
		if (text.Contains("_tier_0")){
			Debug.LogError("Fina all token for '"+text+"'");
		}
		List<string> allTokens=new List<string>();
		//if (additonalTokenSources!=null && additonalTokenSources.Count==1){
		//	return allTokens;			
		//}
		
		string origionalText=text;
		int count=0;
		for (int nextCloseToken=text.IndexOf("}");nextCloseToken>=0 && nextCloseToken<text.Length;nextCloseToken=text.IndexOf("}")){
			int matchingOpenToken=text.LastIndexOf("{",nextCloseToken);
			if (matchingOpenToken>0){
				string key=text.Substring(matchingOpenToken-1,1);
				TokenizerSource source=null;
				if (sources.ContainsKey(key)){
					source=sources[key];
				}
				
				if (source==null && additonalTokenSources!=null){
					foreach (TokenizerSource additonalTokenSource in additonalTokenSources){
						if (additonalTokenSource.GetKey()==key){
							source=additonalTokenSource;
							break;
						}
					}
				}
				
				string token=text.Substring(matchingOpenToken+1,nextCloseToken-matchingOpenToken-1);
				string keyToken=key+"{"+token+"}";
				if (!allTokens.Contains(keyToken)){
					if (text.Contains("_tier_0")){
						Debug.LogError("Adding Token Listener '"+keyToken+"'");
					}
					allTokens.Add(keyToken);
				}
				
				if (source!=null){
					text=text.Substring(0, matchingOpenToken-1)+source.ProcessToken(token,additonalTokenSources)+text.Substring(nextCloseToken+1);
				}else{
					text=text.Substring(0, matchingOpenToken-1)+token+text.Substring(nextCloseToken+1);
					Debug.LogWarning("Unable to process token no source found for key='"+key+"' for '"+origionalText+"'");
				}
				
				nextCloseToken=matchingOpenToken;
			}
			if (count++>100){
				Debug.LogError("Unable to process token more then 100 found in '"+origionalText+"'");
				break;
			}
		}
		return allTokens;
	}
	
	static public string ResolveTokens(string text, List<TokenizerSource> additionalSources=null){
		if (additionalSources!=null && additionalSources.Count==1 && additionalSources[0] is TokenIgnoreAllChildren){
			return text;			
		}
		string originalText=text;
		try {
			int count=0;
			for (int nextCloseToken=text.IndexOf("}");nextCloseToken!=-1;nextCloseToken=text.IndexOf("}")){
				int matchingOpenToken=text.LastIndexOf("{",nextCloseToken);
				if (matchingOpenToken>0){
					string key=text.Substring(matchingOpenToken-1,1);
					TokenizerSource source=null;
					if (sources.ContainsKey(key)){
						source=sources[key];
					}
					if (source==null && additionalSources!=null){
						foreach (TokenizerSource tokenizerSource in additionalSources){
							if (tokenizerSource.GetKey()==key){
								source=tokenizerSource;
								break;
							}
						}
					}
					if (source!=null){
						string token=text.Substring(matchingOpenToken+1,nextCloseToken-matchingOpenToken-1);
						text=text.Substring(0, matchingOpenToken-1)+source.ProcessToken(token)+text.Substring(nextCloseToken+1);
						nextCloseToken=matchingOpenToken;
					}else{
						Debug.LogWarning("Unable to process token for '"+key+"{' original was '"+originalText+"'");
						string token=text.Substring(matchingOpenToken+1,nextCloseToken-matchingOpenToken-1);
						text=text.Substring(0, matchingOpenToken-1)+text.Substring(nextCloseToken+1);
						//nextCloseToken++;
					}
				}
				if (count++>100){
					Debug.LogError("Unable to process token more then 100 found in '"+originalText+"'");
					break;
				}
			}
			return text;
		} catch (ArgumentOutOfRangeException e){
			Debug.LogError(e.Message+" for '"+originalText+"' was as far as '"+text+"'");
		}
		return "";
	}
	
	private static Dictionary<string,List<DataTokenChangeListener>> FindDataTokenListenerSet(string key){
		if (!dataTokenListeners.ContainsKey(key)){
			dataTokenListeners.Add(key,new Dictionary<string,List<DataTokenChangeListener>>());
		}
		return dataTokenListeners[key];
	}
	
	private static List<DataTokenChangeListener> FindDataTokenListenerList(string key,string term){
		Dictionary<string,List<DataTokenChangeListener>> set=FindDataTokenListenerSet(key);
		if (!set.ContainsKey(term)){
			set.Add(term,new List<DataTokenChangeListener>());
		}
		return set[term];
	}
	
	public static List<string> AddDataTokenChangeListener(DataTokenChangeListener listener, string fullToken, List<TokenizerSource> additonalTokenSources=null){
        //if (additonalTokenSources!=null && additonalTokenSources.Count==1 && additonalTokenSources[0] is TokenIgnoreAllChildren){
        //	return new List<string>();
        //}

        //Debug.Log("Added Change for data " + fullToken);

        List<string> usedTokens=new List<string>();
		List<string> allTokens=FindAllTokens(fullToken,additonalTokenSources);
		foreach (string token in allTokens){
			string key=token.Substring(0,1);
			string term=token.Substring(2,token.Length-3);
			
			term=ResolveTokens(term,additonalTokenSources);
			
			if (sources.ContainsKey(key)){
				term=sources[key].GetTokenChangeKeyFromToken(term);
			
				if (term!=null){
					List<DataTokenChangeListener> list=FindDataTokenListenerList(key,term);
					if (!list.Contains(listener)){
						Debug.Log("Listening for token. "+key+"{"+term+"}");
						list.Add(listener);
						usedTokens.Add(key+"{"+term+"}");
					}
				}
			}
		}
		
		return usedTokens;
	}
	
	public static void AddAllDataTokenChangeListener(DataTokenChangeListener listener, string token, List<TokenizerSource> additonalTokenSources=null){
		List<string> allTokens=FindAllTokens(token);
		foreach (string t in allTokens){
			AddDataTokenChangeListener(listener,t,additonalTokenSources);
		}
	}
	
	public static void RemoveDataTokenChangeListener(DataTokenChangeListener listener){
		foreach (Dictionary<string,List<DataTokenChangeListener>> dataTokenListenerSet in dataTokenListeners.Values){
			foreach (List<DataTokenChangeListener> dataTokenListenerList in dataTokenListenerSet.Values){
				dataTokenListenerList.Remove(listener);
			}	
		}
	}
	
	public static void RemoveDataTokenChangeListener(DataTokenChangeListener listener, string token){
		string key=token.Substring(0,1);
		string term=token.Substring(2,token.Length-3);
		string[] keyParts=System.Text.RegularExpressions.Regex.Split(term,"\\.(?=[^\\]]*(?:\\[|$))");
		
		List<DataTokenChangeListener> list=FindDataTokenListenerList(key,keyParts[0]);
		if (list.Contains(listener)){
			list.Remove(listener);
		}
	}
	
	public static void DataChanged(string key,string term,object newvalue){
		if (dataTokenListeners.ContainsKey(key)){
			Dictionary<string,List<DataTokenChangeListener>> set=dataTokenListeners[key];
			if (set.ContainsKey(term)){
				string token=key+"{"+term+"}";
				if (set[term].Count>0){
					Debug.Log("Change for data "+key+"{"+term+"}");
				}
				
				foreach (DataTokenChangeListener listener in set[term]){
					listener.DataTokenChanged(token,newvalue);
				}
			}
		}
	}
	
	public static List<TokenizerSource> FindAdditionalSources(Transform source){
		List<TokenizerSource> additionalSources=null;
		while (source!=null){
			TokenizerSource tokenizerSource=source.GetComponent<TokenizerSource>();
			if (tokenizerSource!=null){
				if (additionalSources==null){
					additionalSources=new List<TokenizerSource>();
				}
				//if (tokenizerSource is TokenIgnoreAllChildren){
				//	//additionalSources.Clear();
				//	//additionalSources.Add(tokenizerSource);
				//	break;
				//}
				additionalSources.Add(tokenizerSource);
			}
			source=source.parent;
		}
		return additionalSources;
	}
	
	
}
