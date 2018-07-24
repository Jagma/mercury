using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[InitializeOnLoad]
public class SnapUIToolbox : EditorWindow {

	//items
	private static Texture2D iconButton;
	private static Texture2D iconLabel;
	private static Texture2D iconToggle;
	private static Texture2D iconTextField;
	private static Texture2D iconImage;
	private static Texture2D iconSlider;
	private static Texture2D iconScrollbar;
	private static Texture2D iconSpacer;

	//layouts
	private static Texture2D iconColumnLayout;
	private static Texture2D iconRowLayout;
	private static Texture2D iconGridLayout;
	private static Texture2D iconCardLayout;

	private static TreeNode baseTreeNode;
	
	private static GameObject draggedObject;
	private static Vector2 buttonSize = new Vector2(32, 32);
	
	private static System.Type pointerEvents;

	//public SnapUIToolbox () {
	//	Init();
	//}

	[MenuItem ("Window/SnapUI Toolbox")]
	static void Init () {
		//SnapUIToolbox window;
		//window = (SnapUIToolbox)EditorWindow.GetWindow(typeof(SnapUIToolbox));
		//window.Show();
		//EditorWindow.GetWindow<SnapUIToolbox>(typeof(SnapUIToolbox)).Show();
		System.Reflection.Assembly asm = typeof(UnityEditor.EditorWindow).Assembly;
		System.Type wndType = asm.GetType("UnityEditor.SceneHierarchyWindow");
		UnityEditor.EditorWindow wnd = UnityEditor.EditorWindow.GetWindow(wndType, false,  "Hierarchy", false);
		EditorWindow.GetWindow<SnapUIToolbox>(wndType).Show();

		foreach (System.Reflection.Assembly a in System.AppDomain.CurrentDomain.GetAssemblies()) {
			string[] parts = a.FullName.Split(",".ToCharArray());
			if (parts[0] == "Assembly-CSharp") {
				System.Type[] types = a.GetTypes();
				foreach (System.Type t in types) {
					if (t.FullName == "SupportClasses.PointerEvents") {
						pointerEvents = t;
					}
				}
			}
		}

		LoadIcons();
	}

	static void LoadIcons () {
		iconButton = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Button)).image as Texture2D;
		iconLabel = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Text)).image as Texture2D;
		iconToggle = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Toggle)).image as Texture2D;
		iconTextField = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.InputField)).image as Texture2D;
		iconImage = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Image)).image as Texture2D;
		iconSlider = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Slider)).image as Texture2D;
		iconScrollbar = EditorGUIUtility.ObjectContent(null, typeof(UnityEngine.UI.Scrollbar)).image as Texture2D;
		iconSpacer = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("BoxSpacer")[0]), typeof(Texture2D));

		iconColumnLayout = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("ColumnLayoutIcon")[0]), typeof(Texture2D));
		iconRowLayout = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("RowLayoutIcon")[0]), typeof(Texture2D));
		iconGridLayout = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("GridLayout")[0]), typeof(Texture2D));
		iconCardLayout = (Texture2D)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("CardLayout")[0]), typeof(Texture2D));
	}

	void OnInspectorUpdate() {
		LoadIcons();
	}

	void OnGUI () {
		//EditorApplication.hierarchyWindowItemOnGUI

		if (Event.current.type == EventType.MouseDown) {
		}
		if (Event.current.type == EventType.MouseUp) {
			if (draggedObject != null) {
				draggedObject = null;
			}
		}
		if (Event.current.type == EventType.MouseDrag) {
			if (draggedObject != null) {
				DragAndDrop.PrepareStartDrag();
				Object[] draggedObjects = new Object[1] {(Object)draggedObject};
				DragAndDrop.objectReferences = draggedObjects;
				DragAndDrop.StartDrag("Drag");
			}
		}

		int offsetX = 0, offsetY = 0;

		//GUI.Label(new Rect(offsetX * buttonSize.x, offsetY * buttonSize.y, buttonSize.x * 5, buttonSize.y), "Images");

		ToolboxButton(offsetX, offsetY, iconButton, CreateSnapButton);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconLabel, CreateSnapLabel);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconToggle, CreateSnapToggle);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconTextField, CreateSnapTextField);

		offsetY++;

		offsetX = 0;
		ToolboxButton(offsetX, offsetY, iconImage, CreateSnapImage);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconSlider, CreateSnapSlider);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconScrollbar, CreateSnapScrollbar);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconSpacer, CreateSnapSpacer);

		offsetY++;

		offsetX = 0;
		ToolboxButton(offsetX, offsetY, iconColumnLayout, CreateSnapColumnLayoutPanel);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconRowLayout, CreateSnapRowLayoutPanel);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconGridLayout, CreateSnapGridLayoutPanel);
		offsetX++;
		ToolboxButton(offsetX, offsetY, iconCardLayout, CreateSnapCardLayoutPanel);
	}

	public delegate GameObject Action ();
	void ToolboxButton (int x, int y, Texture2D image, Action action) {
		Rect rect = new Rect(x * buttonSize.x, y * buttonSize.y, buttonSize.x, buttonSize.y);
		GUI.Box(rect, new GUIContent(image?image:null, "Temp"));
		if (Event.current.type == EventType.MouseDown) {
			if (rect.Contains(Event.current.mousePosition)) {
				draggedObject = action();
			}
		}
	}

	static GameObject GetParentObject () {
		GameObject selected = Selection.activeGameObject;
		Canvas canvas = (Canvas)GameObject.FindObjectOfType(typeof(Canvas));
		if (canvas == null) {
			GameObject canvasObject = new GameObject("Canvas");
			canvasObject.AddComponent<RectTransform>();
			canvas = canvasObject.AddComponent<Canvas>();
			canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
			canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();
		}
		
		if (selected != null && selected.transform.root.GetComponent<Canvas>()) {
			return selected;
		}
		return canvas.gameObject;
	}

	static GameObject RecursivelySearchForLayout (GameObject current) {
		if (current.GetComponent(typeof(Layout)) != null) {
			Layout layout = current.GetComponent(typeof(Layout)) as Layout;
			layout.DoLayout();
			return current;
		}
		if (current.transform.parent != null) {
			return RecursivelySearchForLayout(current.transform.parent.gameObject);
		}
		return null;
	}

	[MenuItem ("GameObject/UI/SnapUI/Button")]
	static GameObject CreateSnapButton () {
		GameObject newObject = new GameObject("Button (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Image>();
		newObject.AddComponent<UnityEngine.UI.Button>();
		newObject.AddComponent<SupportClasses.PointerEvents>();

		GameObject textObject = new GameObject("Text");
		textObject.transform.parent = newObject.transform;
		RectTransform rect = textObject.AddComponent<RectTransform>();
		rect.anchorMin = new Vector2(0, 0);
		rect.anchorMax = new Vector2(1, 1);
		rect.pivot = new Vector2(0.5f, 0.5f);
		rect.anchoredPosition = new Vector2(0, 0);
		rect.sizeDelta = new Vector2(0, 0);
		textObject.AddComponent<CanvasRenderer>();
		UnityEngine.UI.Text textComponent = textObject.AddComponent<UnityEngine.UI.Text>();
		textComponent.text = "Button";
		textComponent.color = Color.black;

		newObject.AddComponent(pointerEvents);

		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Label")]
	static GameObject CreateSnapLabel () {
		GameObject newObject = new GameObject("Label (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Text>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Toggle")]
	static GameObject CreateSnapToggle () {
		GameObject newObject = new GameObject("Toggle (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Toggle>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/TextField")]
	static GameObject CreateSnapTextField () {
		GameObject newObject = new GameObject("Text Field (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.InputField>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Image")]
	static GameObject CreateSnapImage () {
		GameObject newObject = new GameObject("Image (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Image>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Slider")]
	static GameObject CreateSnapSlider () {
		GameObject newObject = new GameObject("Slider (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Slider>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Scrollbar")]
	static GameObject CreateSnapScrollbar () {
		GameObject newObject = new GameObject("Scrollbar (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.Scrollbar>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Spacer")]
	static GameObject CreateSnapSpacer () {
		GameObject newObject = new GameObject("Spacer (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Panel/Column Layout")]
	static GameObject CreateSnapColumnLayoutPanel () {
		GameObject newObject = new GameObject("Column (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		//newObject.AddComponent<ColumnLayout>();
		newObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();

		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Panel/Row Layout")]
	static GameObject CreateSnapRowLayoutPanel () {
		GameObject newObject = new GameObject("Row (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		//newObject.AddComponent<RowLayout>();
		newObject.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Panel/Grid Layout")]
	static GameObject CreateSnapGridLayoutPanel () {
		GameObject newObject = new GameObject("Grid (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		newObject.AddComponent<UnityEngine.UI.GridLayoutGroup>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}

	[MenuItem ("GameObject/UI/SnapUI/Panel/Card Layout")]
	static GameObject CreateSnapCardLayoutPanel () {
		GameObject newObject = new GameObject("Card (SnapUI)");
		newObject.transform.parent = GetParentObject().transform;
		newObject.AddComponent<RectTransform>();
		newObject.AddComponent<CanvasRenderer>();
		
		Selection.activeGameObject = newObject;
		return newObject;
	}
}

public class TreeNode {
	public GameObject myObject;
	public TreeNode parent;
	public List<TreeNode> children = new List<TreeNode>();

	public TreeNode (GameObject myObject) {
		this.myObject = myObject;
	}

	public TreeNode (GameObject myObject, TreeNode parent) {
		this.myObject = myObject;
		this.parent = parent;
	}

	public TreeNode (GameObject myObject, TreeNode parent, List<TreeNode> children) {
		this.myObject = myObject;
		this.parent = parent;
		this.children = children;
	}

	public void SetParent (TreeNode parent) {
		this.parent = parent;
	}

	public void SetChildren (List<TreeNode> children) {
		this.children = children;
	}
}
