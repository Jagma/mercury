using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionSource {
	string GetSourceName();
	void ExecuteAction(string action, List<TokenizerSource> additionalSources=null);
}
