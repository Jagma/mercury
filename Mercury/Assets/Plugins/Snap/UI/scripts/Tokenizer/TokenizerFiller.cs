using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TokenizerFiller : MonoBehaviour {

    List<TextItem> textItems=new List<TextItem>();
	
	void Start () {
		FindAllTextItems();
		FillTokens();
	}
	
	public void FillTokens(){
		foreach (TextItem textItem in textItems){
			textItem.FillLangage();
		}
	}
	
	private void FindAllTextItems(){
		FindAllTextItems(RemoveFieldsWithATokenIgnore(GetComponentsInChildren<Text>(true)));
		FindAllTextItems(RemoveFieldsWithATokenIgnore(GetComponentsInChildren<TextMeshProUGUI>(true)));
	}
	
	private Text[] RemoveFieldsWithATokenIgnore(Text[] fields){
		for (int t=0;t<fields.Length;t++){
			Transform parent=fields[t].transform;
			while (parent!=transform){
				if (parent.GetComponent<TokenIgnoreAllChildren>()!=null){
					fields[t]=null;
					break;
				}
				parent=parent.parent;
			}
		}
		return fields;
	}
	
	private TextMeshProUGUI[] RemoveFieldsWithATokenIgnore(TextMeshProUGUI[] fields){
		for (int t=0;t<fields.Length;t++){
			Transform parent=fields[t].transform;
			while (parent!=transform){
				if (parent.GetComponent<TokenIgnoreAllChildren>()!=null){
					fields[t]=null;
					break;
				}
				parent=parent.parent;
			}
		}
		return fields;
	}
	
	private void FindAllTextItems(Text[] fields){
		if (fields!=null){
			foreach (Text field in fields){
				if (field!=null){
					TextItem textItem=new TextItem(field);
					textItems.Add(textItem);
					Tokenizer.AddAllDataTokenChangeListener(textItem,field.text,Tokenizer.FindAdditionalSources(field.transform));
				}
			}
		}
	}
	
	private void FindAllTextItems(TextMeshProUGUI[] fields){
		if (fields!=null){
			foreach (TextMeshProUGUI field in fields){
				if (field!=null){
					TextItem textItem=new TextItem(field);
					textItems.Add(textItem);
					Tokenizer.AddAllDataTokenChangeListener(textItem,field.text,Tokenizer.FindAdditionalSources(field.transform));
				}
			}
		}
	}
	
	public void OnDestroy(){
		foreach (TextItem textItem in textItems){
			Tokenizer.RemoveDataTokenChangeListener(textItem);
		}
	}
	
	class TextItem : DataTokenChangeListener
    {
		string origionalString;
		Text text;
		TextMeshProUGUI textMesh;
		
		public TextItem(Text text){
			this.text=text;
			origionalString=text.text;
		}
		
		public TextItem(TextMeshProUGUI textMesh){
			this.textMesh=textMesh;
			origionalString=textMesh.text;
		}
		
		public void DataTokenChanged(string token,object newvalue){
			FillLangage();
		}
		
		public void FillLangage(){
			if (text!=null){
				text.text=Tokenizer.ResolveTokens(origionalString,Tokenizer.FindAdditionalSources(text.transform));
				text.SetLayoutDirty();
			}
			if (textMesh!=null){
				textMesh.text=Tokenizer.ResolveTokens(origionalString,Tokenizer.FindAdditionalSources(textMesh.transform));
				textMesh.SetLayoutDirty();
			}
		}
	}

}
