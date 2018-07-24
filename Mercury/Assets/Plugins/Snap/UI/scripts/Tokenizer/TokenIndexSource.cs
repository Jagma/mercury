using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenIndexSource : MonoBehaviour, TokenizerSource {
	public string tokenName="index";
	public string tokenSetType="#";
	public string index="0";
	
	public string GetKey(){
		return tokenSetType;
	}
	
	public string GetTokenChangeKeyFromToken(string token){
		return token;
	}
	
	public string ProcessToken(string token,List<TokenizerSource> additionalSources=null){
		if (token==tokenName){
			return index;
		}
		return "";
	}

}
