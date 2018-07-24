using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecoratorStyle : MonoBehaviour {
	private Button button=null;
	private InputField inputField=null;
	private Toggle toggle=null;
	private Image image=null;
	private ColumnLayout columnLayout=null;
	private RowLayout rowLayout=null;
	private Text text=null;
	public TextMeshProUGUI textMesh=null;
	
	public bool copyRectTransforms=true;
	
	public void Init(){
		button=GetComponentInMyChildren<Button>();
		inputField=GetComponentInMyChildren<InputField>();
		toggle=GetComponentInMyChildren<Toggle>();
		image=gameObject.GetComponent<Image>();
		columnLayout=gameObject.GetComponent<ColumnLayout>();
		rowLayout=gameObject.GetComponent<RowLayout>();
		text=GetComponentInMyChildren<Text>();
		textMesh=GetComponentInMyChildren<TextMeshProUGUI>();
	}
	
	private componentType GetComponentInMyChildren<componentType>() where componentType : Component {
		for (int t=0;t<transform.childCount;t++){
			componentType component=transform.GetChild(t).GetComponent<componentType>();
			if (component!=null){
				return component;
			}
		}
		return null;
	}
	
	public void Decorate(Decorator decorator,GameObject go,List<DecoratorStyle> parentStyles){
		//Debug.Log("Decorating "+go.name);
		
		if (go.GetComponent<DecoratorIgnore>()){
			return;
		}
		
		Decorator privateDecorator=go.GetComponent<Decorator>();
		if (privateDecorator!=null && privateDecorator!=decorator){
			parentStyles=new List<DecoratorStyle>(parentStyles);
			parentStyles.Add(this);
			privateDecorator.ApplyStyle(parentStyles);
		}else{
			bool doChildren=true;
			if (Decorate(go.transform.GetComponent<Button>(),parentStyles)){
				doChildren=false;
			} else if (Decorate(go.transform.GetComponent<InputField>(),parentStyles)){
				doChildren=false;
			} else if (Decorate(go.transform.GetComponent<Toggle>(),parentStyles)){
				doChildren=false;
			} else if (Decorate(go.transform.GetComponent<ColumnLayout>(),parentStyles)){
				doChildren=true;
			} else if (Decorate(go.transform.GetComponent<RowLayout>(),parentStyles)){
				doChildren=true;
			} else if (Decorate(go.transform.GetComponent<Image>(),parentStyles)){
				doChildren=true;
			} else if (Decorate(go.transform.GetComponent<Text>(),parentStyles)){
				doChildren=false;
			} else if (Decorate(go.transform.GetComponent<TextMeshProUGUI>(),parentStyles)){
				doChildren=false;
			}
			
			for (int t=0;doChildren && t<go.transform.childCount;t++){
				Decorate(decorator,go.transform.GetChild(t).gameObject,parentStyles);
			}
		}		
	}
	
	private bool DecorateChild(GameObject go,List<DecoratorStyle> parentStyles){
		Decorator privateDecorator=go.GetComponent<Decorator>();
		if (privateDecorator!=null){
			parentStyles=new List<DecoratorStyle>(parentStyles);
			parentStyles.Add(this);
			privateDecorator.ApplyStyle(parentStyles);
			return true;
		}
		return false;
	}
	
	public TextMeshProUGUI GetTextMeshStyle(List<DecoratorStyle> parentStyles){
		if (textMesh!=null){
			return textMesh;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].textMesh!=null){
					return parentStyles[t].textMesh;
				}
			}
		}
		return null;
	}	
	
	public Text GetTextStyle(List<DecoratorStyle> parentStyles){
		if (text!=null){
			return text;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].text!=null){
					return parentStyles[t].text;
				}
			}
		}
		return null;
	}	
	
	public RowLayout GetRowLayoutStyle(List<DecoratorStyle> parentStyles){
		if (rowLayout!=null){
			return rowLayout;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].rowLayout!=null){
					return parentStyles[t].rowLayout;
				}
			}
		}
		return null;
	}	
	
	public ColumnLayout GetColumnLayoutStyle(List<DecoratorStyle> parentStyles){
		if (columnLayout!=null){
			return columnLayout;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].columnLayout!=null){
					return parentStyles[t].columnLayout;
				}
			}
		}
		return null;
	}	
	
	public InputField GetInputFieldStyle(List<DecoratorStyle> parentStyles){
		if (inputField!=null){
			return inputField;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].inputField!=null){
					return parentStyles[t].inputField;
				}
			}
		}
		return null;
	}	
	
	public Toggle GetToggleStyle(List<DecoratorStyle> parentStyles){
		if (toggle!=null){
			return toggle;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].toggle!=null){
					return parentStyles[t].toggle;
				}
			}
		}
		return null;
	}	
	
	public Button GetButtonStyle(List<DecoratorStyle> parentStyles){
		if (button!=null){
			return button;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].button!=null){
					return parentStyles[t].button;
				}
			}
		}
		return null;
	}
	
	public Image GetImageStyle(List<DecoratorStyle> parentStyles){
		if (image!=null){
			return image;
		}
		if (parentStyles!=null){
			for (int t=parentStyles.Count-1;t>=0;t--){
				if (parentStyles[t].image!=null){
					return parentStyles[t].image;
				}
			}
		}
		return null;
	}
	
	public bool Decorate(Text field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			Text style=GetTextStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No Text Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(TextMeshProUGUI field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			TextMeshProUGUI style=GetTextMeshStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No TextMeshProUGUI Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(RowLayout field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			RowLayout style=GetRowLayoutStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No RowLayout Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(ColumnLayout field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			ColumnLayout style=GetColumnLayoutStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No ColumnLayout Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(Toggle field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			Toggle style=GetToggleStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No Button Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(InputField field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			InputField style=GetInputFieldStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No InputField Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(Image field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			Image style=GetImageStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No Image Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	public bool Decorate(Button field,List<DecoratorStyle> parentStyles){
		if (field!=null){
			Button style=GetButtonStyle(parentStyles);
			if (style!=null){
				Copy(field,style);
				CopyLayoutRule(field.gameObject,style.gameObject);
			}else{
				Debug.Log("No Button Style found on "+name);
			}
			return true;
		}
		return false;
	}
	
	private void CopyLayoutRule(GameObject dest, GameObject source){
		LayoutRule sourceRule=source.GetComponent<LayoutRule>();
		
		if (sourceRule!=null){
			LayoutRule destRule=dest.GetComponent<LayoutRule>();
			if (destRule==null){
				destRule=dest.AddComponent<LayoutRule>();
			}
			if (!destRule.overrrideStyle){
				destRule.stretchWidth=sourceRule.stretchWidth;
				destRule.stretchHeight=sourceRule.stretchHeight;
				destRule.border=sourceRule.border;
			}
		}
		
	}
	
	private void Copy(RowLayout dest, RowLayout source){
		if (dest!=null && source!=null){
			dest.border=source.border;
			dest.horizontalAlignment=source.horizontalAlignment;
			dest.verticleAlignment=source.verticleAlignment;
			dest.spacing=source.spacing;
			dest.matchWidths=source.matchWidths;
			dest.matchHeights=source.matchHeights;
			dest.resizeContainerHeight=source.resizeContainerHeight;
			dest.resizeContainerWidth=source.resizeContainerWidth;
			
			Copy(dest.gameObject.GetComponentInChildren<Image>(),source.gameObject.GetComponentInChildren<Image>());
		}
	}	
	
	private void Copy(TextMeshProUGUI dest, TextMeshProUGUI source){
		if (dest!=null && source!=null){
			dest.alignment=source.alignment;
			dest.alpha=source.alpha;
			dest.autoSizeTextContainer=source.autoSizeTextContainer;
			dest.characterSpacing=source.characterSpacing;
			dest.characterWidthAdjustment=source.characterWidthAdjustment;
			dest.color=source.color;
			dest.colorGradient=source.colorGradient;
			dest.colorGradientPreset=source.colorGradientPreset;
			dest.enableAutoSizing=source.enableAutoSizing;
			dest.enableCulling=source.enableCulling;
			dest.enableKerning=source.enableKerning;
			dest.enableVertexGradient=source.enableVertexGradient;
			dest.enableWordWrapping=source.enableWordWrapping;
			dest.extraPadding=source.extraPadding;
			dest.faceColor=source.faceColor;
			dest.firstVisibleCharacter=source.firstVisibleCharacter;
			dest.font=source.font;
			//dest.fontMaterial=source.fontMaterial;
			//dest.fontMaterials=source.fontMaterials;
			//dest.fontSharedMaterial=source.fontSharedMaterial;
			//dest.fontSharedMaterials=source.fontSharedMaterials;
			dest.fontSize=source.fontSize;
			dest.fontSizeMax=source.fontSizeMax;
			dest.fontSizeMin=source.fontSizeMin;
			dest.fontStyle=source.fontStyle;
			dest.fontWeight=source.fontWeight;
			dest.geometrySortingOrder=source.geometrySortingOrder;
			dest.horizontalMapping=source.horizontalMapping;
			dest.ignoreRectMaskCulling=source.ignoreRectMaskCulling;
			dest.ignoreVisibility=source.ignoreVisibility;
			dest.lineSpacing=source.lineSpacing;
			dest.lineSpacingAdjustment=source.lineSpacingAdjustment;
			dest.mappingUvLineOffset=source.mappingUvLineOffset;
			dest.margin=source.margin;
			dest.maskable=source.maskable;
			dest.maskOffset=source.maskOffset;
			dest.material=source.material;
			dest.maxVisibleCharacters=source.maxVisibleCharacters;
			dest.maxVisibleLines=source.maxVisibleLines;
			dest.maxVisibleWords=source.maxVisibleWords;
			dest.outlineColor=source.outlineColor;
			dest.outlineWidth=source.outlineWidth;
			dest.overflowMode=source.overflowMode;
			dest.overrideColorTags=source.overrideColorTags;
			dest.pageToDisplay=source.pageToDisplay;
			dest.paragraphSpacing=source.paragraphSpacing;
			dest.parseCtrlCharacters=source.parseCtrlCharacters;
			dest.richText=source.richText;
			dest.verticalMapping=source.verticalMapping;
			dest.wordSpacing=source.wordSpacing;
			dest.wordWrappingRatios=source.wordWrappingRatios;
			dest.UpdateFontAsset();

		}
	}	
	
	private void Copy(ColumnLayout dest, ColumnLayout source){
		if (dest!=null && source!=null){
			dest.border=source.border;
			dest.horizontalAlignment=source.horizontalAlignment;
			dest.verticleAlignment=source.verticleAlignment;
			dest.spacing=source.spacing;
			dest.matchWidths=source.matchWidths;
			dest.matchHeights=source.matchHeights;
			dest.resizeContainerHeight=source.resizeContainerHeight;
			dest.resizeContainerWidth=source.resizeContainerWidth;
			
			Copy(dest.gameObject.GetComponentInChildren<Image>(),source.gameObject.GetComponentInChildren<Image>());
		}
	}	
	
	private void Copy(Toggle dest, Toggle source){
		if (dest!=null && source!=null){
			dest.interactable=source.interactable;
			dest.transition=source.transition;
			dest.targetGraphic=source.targetGraphic;
			dest.colors=source.colors;
			dest.spriteState=source.spriteState;
			dest.animationTriggers=source.animationTriggers;
			dest.navigation=source.navigation;
			dest.toggleTransition=source.toggleTransition;
			
			Copy(dest.image,source.image);
			
			Transform destBackground=dest.transform.Find("Background");
			Transform sourceBackground=source.transform.Find("Background");
			
			if (destBackground!=null && sourceBackground!=null){
				Copy(destBackground.GetComponent<Image>(),sourceBackground.GetComponent<Image>());
				Copy(destBackground.GetComponent<RectTransform>(),sourceBackground.GetComponent<RectTransform>());
				
				Transform destCheckMark=destBackground.Find("Checkmark");
				Transform sourceCheckMark=sourceBackground.Find("Checkmark");
				
				if (destCheckMark!=null && sourceCheckMark!=null){
					Copy(destCheckMark.GetComponent<Image>(),sourceCheckMark.GetComponent<Image>());
					Copy(destCheckMark.GetComponent<RectTransform>(),sourceCheckMark.GetComponent<RectTransform>());
				}
			}
			
			Copy(dest.gameObject.GetComponentInChildren<Text>(),source.gameObject.GetComponentInChildren<Text>());
			Copy(dest.gameObject.GetComponentInChildren<TextMeshProUGUI>(),source.gameObject.GetComponentInChildren<TextMeshProUGUI>());
			Copy(dest.gameObject.GetComponent<RectTransform>(),source.gameObject.GetComponent<RectTransform>());
		}
	}	
	
	private void Copy(InputField dest, InputField source){
		if (dest!=null && source!=null){
			Transform destText=dest.transform.Find("Text");
			Transform sourceText=source.transform.Find("Text");
			if (destText!=null && sourceText!=null){
				Copy(destText.GetComponent<Text>(),sourceText.GetComponent<Text>());
			}
			
			Transform destPlaceHolder=dest.transform.Find("Placeholder");
			Transform sourcePlaceHolder=source.transform.Find("Placeholder");
			if (destPlaceHolder!=null && sourcePlaceHolder!=null){
				Copy(destPlaceHolder.GetComponent<Text>(),sourcePlaceHolder.GetComponent<Text>());
			}
			
			CaretFix sourceCaretFix=source.GetComponent<CaretFix>();
			if (sourceCaretFix!=null){
				CaretFix destCaretFix=dest.gameObject.AddComponent<CaretFix>();
				destCaretFix.up=sourceCaretFix.up;
				destCaretFix.right=sourceCaretFix.right;
			}
			
			dest.interactable=source.interactable;
			dest.transition=source.transition;
			dest.colors=source.colors;
			dest.spriteState=source.spriteState;
			dest.animationTriggers=source.animationTriggers;
			dest.navigation=source.navigation;
			dest.lineType=source.lineType;
			dest.caretBlinkRate=source.caretBlinkRate;
			dest.caretWidth=source.caretWidth;
			dest.customCaretColor=source.customCaretColor;
			dest.selectionColor=source.selectionColor;
			dest.shouldHideMobileInput=source.shouldHideMobileInput;
			
			Copy(dest.GetComponent<Image>(),source.GetComponent<Image>());
			Copy(dest.gameObject.GetComponent<RectTransform>(),source.gameObject.GetComponent<RectTransform>());
			
		}
	}	
	
	private void Copy(Image dest, Image source){
		if (dest!=null && source!=null){
			dest.type=source.type;
			dest.sprite=source.sprite;
			dest.color=source.color;
			dest.material=source.material;
			dest.raycastTarget=source.raycastTarget;
			dest.fillCenter=source.fillCenter;
			dest.fillAmount=source.fillAmount;
			dest.fillClockwise=source.fillClockwise;
			dest.fillMethod=source.fillMethod;
			dest.fillOrigin=source.fillOrigin;
		}	
	}
	
	private void Copy(Transform dest, ContentSizeFitter sourceContentSizeFitter){
		if (sourceContentSizeFitter!=null){
			ContentSizeFitter destContentSizeFitter=dest.GetComponent<ContentSizeFitter>();
			if (destContentSizeFitter==null){
				destContentSizeFitter=dest.gameObject.AddComponent<ContentSizeFitter>();
			}
			destContentSizeFitter.horizontalFit=sourceContentSizeFitter.horizontalFit;
			destContentSizeFitter.verticalFit=sourceContentSizeFitter.verticalFit;
		}	
	}

	private void Copy(Text dest, Text source){
		if (dest!=null && source!=null){
			Copy(dest.transform,source.GetComponent<ContentSizeFitter>());
			
			dest.alignByGeometry=source.alignByGeometry;
			dest.alignment=source.alignment;
			dest.color=source.color;
			dest.font=source.font;
			dest.fontSize=source.fontSize;
			dest.fontStyle=source.fontStyle;
			dest.horizontalOverflow=source.horizontalOverflow;
			dest.lineSpacing=source.lineSpacing;
			dest.material=source.material;
			dest.resizeTextForBestFit=source.resizeTextForBestFit;
			dest.resizeTextMaxSize=source.resizeTextMaxSize;
			dest.resizeTextMinSize=source.resizeTextMinSize;
			dest.supportRichText=source.supportRichText;
			dest.verticalOverflow=source.verticalOverflow;
			dest.CalculateLayoutInputHorizontal();
			dest.CalculateLayoutInputVertical();
			Copy(dest.gameObject.GetComponent<RectTransform>(),source.gameObject.GetComponent<RectTransform>());
		}
	}		
	
	private void Copy(Button dest, Button source){
		if (dest!=null && source!=null){
			dest.transition=source.transition;
			dest.navigation=source.navigation;
			dest.spriteState=source.spriteState;
			dest.colors=source.colors;
			
			Image image=dest.gameObject.GetComponent<Image>();
			Text text=dest.gameObject.GetComponentInChildren<Text>();
			
			if (source.targetGraphic.GetType() == typeof(Image)){
				dest.targetGraphic=image;
			} else if (source.targetGraphic.GetType() == typeof(Text)){
				dest.targetGraphic=text;
			}
			
			Copy(dest.gameObject.GetComponent<Image>(),source.gameObject.GetComponent<Image>());
			Copy(dest.gameObject.GetComponentInChildren<Text>(),source.gameObject.GetComponentInChildren<Text>());
			Copy(dest.gameObject.GetComponentInChildren<TextMeshProUGUI>(),source.gameObject.GetComponentInChildren<TextMeshProUGUI>());
			Copy(dest.gameObject.GetComponent<RectTransform>(),source.gameObject.GetComponent<RectTransform>());
		}
	}
	
	private void Copy(RectTransform dest, RectTransform source){
		if (copyRectTransforms && dest!=null && source!=null){
			LayoutChildSizer layoutSizer=source.GetComponent<LayoutChildSizer>();
			if (layoutSizer!=null){
				layoutSizer.ResizeRectTransform(dest,source);
			}else{
				dest.pivot=source.pivot;
				dest.anchorMin=source.anchorMin;
				dest.anchorMax=source.anchorMax;
				dest.sizeDelta=source.sizeDelta;
			}
		}
	}
}
