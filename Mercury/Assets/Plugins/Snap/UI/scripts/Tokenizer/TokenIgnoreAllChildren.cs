using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenIgnoreAllChildren : MonoBehaviour, TokenizerSource {
	public string GetKey(){
		return "-";
	}
	
	public string GetTokenChangeKeyFromToken(string token){
		return token;
	}
	
	public string ProcessToken(string token,List<TokenizerSource> additionalSources=null){
		return "";
	}

}
