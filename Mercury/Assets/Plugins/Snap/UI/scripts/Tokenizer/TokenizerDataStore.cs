using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TokenizerDataStore : TokenizerSource {
	oftype GetData<oftype>(string key, List<TokenizerSource> additionalSources=null);
	void SetData(string key,object value);
}
