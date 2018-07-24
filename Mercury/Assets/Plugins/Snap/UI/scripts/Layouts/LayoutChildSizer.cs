using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LayoutChildSizer : MonoBehaviour {
	public RectOffset innerBorder;
	public bool resizeWidth;
	public bool resizeHeight;
	
	public void ResizeRectTransform(RectTransform dest, RectTransform source){
		if (resizeWidth || resizeHeight){
			Rect destRect=LayoutUtils.calculateSize(source);
			Debug.Log("DestSize:"+destRect);
			
			float width=destRect.width;
			float height=destRect.height;
			float minX=20000;
			float maxX=-20000;
			float minY=20000;
			float maxY=-20000;
			
			dest.anchorMin=source.anchorMin;
			dest.anchorMax=source.anchorMax;
			dest.pivot=source.pivot;
			
			for (int t=0;t<dest.transform.childCount;t++){
				GameObject child=dest.transform.GetChild(t).gameObject;
				
				Text childText=child.GetComponent<Text>();
				if (childText !=null){
					childText.CalculateLayoutInputHorizontal();
					childText.CalculateLayoutInputVertical();
				}
				
				TextMeshProUGUI childTextMesh=child.GetComponent<TextMeshProUGUI>();
				if (childTextMesh !=null){
					childTextMesh.CalculateLayoutInputHorizontal();
					childTextMesh.CalculateLayoutInputVertical();
				}
				
				ContentSizeFitter contentSizeFitter=child.GetComponent<ContentSizeFitter>();
				if (contentSizeFitter){
					contentSizeFitter.SetLayoutHorizontal();
					contentSizeFitter.SetLayoutVertical();
				}
				
				RectTransform childRectTransform=child.GetComponent<RectTransform>();
				if (childRectTransform!=null){
					//Debug.Log("LocalChildSizeConsidered:"+childRectTransform.name+":"+childRectTransform.localPosition+"("+childRectTransform.rect.min+","+childRectTransform.rect.max+")");
					//Vector2 min=childRectTransform.rect.min+new Vector2(childRectTransform.localPosition.x,childRectTransform.localPosition.y);
					//Vector2 max=childRectTransform.rect.max+new Vector2(childRectTransform.localPosition.x,childRectTransform.localPosition.y);
					
					Rect rect=LayoutUtils.calculateSize(childRectTransform);
					//if (maxX<childRectTransform.rect.min right){
					//	maxX=childRectTransform.rect.right;
					//}
					//if (maxX<childRectTransform.rect.left){
					//	maxX=childRectTransform.rect.left;
					//}
					//if (maxY<childRectTransform.rect.top){
					//	maxY=childRectTransform.rect.top;
					//}
					//if (maxY<childRectTransform.rect.bottom){
					//	maxY=childRectTransform.rect.bottom;
					//}
					//if (minX>childRectTransform.rect.right){
					//	minX=childRectTransform.rect.right;
					//}
					//if (minX>childRectTransform.rect.left){
					//	minX=childRectTransform.rect.left;
					//}
					//if (minY>childRectTransform.rect.bottom){
					//	minY=childRectTransform.rect.bottom;
					//}
					//if (minY>childRectTransform.rect.top){
					//	minY=childRectTransform.rect.top;
					//}
					if (minX>rect.left){
						minX=rect.left;
					}
					if (minY>rect.top){
						minY=rect.top;
					}
					if (maxX<rect.right){
						maxX=rect.right;
					}
					if (maxY<rect.bottom){
						maxY=rect.bottom;
					}
				}
			}
			if (resizeWidth){
				width=Mathf.Abs(maxX-minX);
			}
			if (resizeHeight){
				height=Mathf.Abs(maxY-minY);
			}
			
			dest.sizeDelta=new Vector2(width+innerBorder.horizontal,height+innerBorder.vertical);
			Debug.Log("LocalChildSize:"+dest.name+":"+width+","+height);
		}else{
			dest.pivot=source.pivot;
			dest.anchorMin=source.anchorMin;
			dest.anchorMax=source.anchorMax;
			dest.sizeDelta=new Vector2(source.sizeDelta.x+innerBorder.horizontal,source.sizeDelta.y+innerBorder.vertical);
		}
	}
}
