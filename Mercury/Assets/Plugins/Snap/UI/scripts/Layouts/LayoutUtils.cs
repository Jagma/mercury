using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutUtils {
	public static Rect calculateSize(RectTransform rectTransform){
		Vector2 pos=new Vector2(rectTransform.localPosition.x,rectTransform.localPosition.y);
		Vector2 min=rectTransform.rect.min+pos;
		Vector2 max=rectTransform.rect.max+pos;
		
		return new Rect(min.x,min.y,max.x-min.x,max.y-min.y);
	}
}
