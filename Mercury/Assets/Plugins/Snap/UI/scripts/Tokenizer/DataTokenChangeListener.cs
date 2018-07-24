using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataTokenChangeListener  {
	void DataTokenChanged(string token,object newvalue);
}
