using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TokenizerSource {
	string GetKey();
	string ProcessToken(string token,List<TokenizerSource> additionalSources=null);
	string GetTokenChangeKeyFromToken(string token);
}
