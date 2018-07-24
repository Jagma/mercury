using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Reflection;
using System;

namespace SupportClasses {
	[CustomEditor(typeof(PointerEvents))]
	public class PointerEventsEditor : Editor {
		static string[] methods;
		static string[] ignoreMethods = new string[] { "Start", "Update" };

		private Swing.Editor.EditorCoroutine runningCoroutine;
		
		static PointerEventsEditor() {
		}

		public IEnumerator GetMethods () {
			PointerEvents obj = target as PointerEvents;
			Debug.Log("try");
			yield return System.Type.GetType(obj.sourceClass.name);
			Debug.Log("pass");

			try {
				methods = System.Type.GetType(obj.sourceClass.name)
					.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static) // Instance methods, both public and private/protected
					.Where(x => x.DeclaringType == System.Type.GetType(obj.sourceClass.name)) // Only list methods defined in our own class
					.Where(x => x.GetParameters().Length == 0) // Make sure we only get methods with zero argumenrts
					.Where(x => !ignoreMethods.Any(n => n == x.Name)) // Don't list methods in the ignoreMethods array (so we can exclude Unity specific methods, etc.)
					.Select(x => x.Name)
					.ToArray();
			} catch (Exception e) {
				Debug.Log("Unable to load methods: object may not contain any " + e);
			}

			yield return null;
		}
		
		public override void OnInspectorGUI() {
			PointerEvents obj = target as PointerEvents;

			if (obj.sourceClass != null && (methods == null || methods.Length == 0)) {
				if (runningCoroutine != null) runningCoroutine.stop();
				runningCoroutine = Swing.Editor.EditorCoroutine.start(GetMethods());
			}

			EditorGUI.BeginChangeCheck();
			obj.sourceClass = EditorGUILayout.ObjectField(obj.sourceClass, typeof(UnityEngine.Object), true);
			if (EditorGUI.EndChangeCheck()) {
				if (runningCoroutine != null) runningCoroutine.stop();
				runningCoroutine = Swing.Editor.EditorCoroutine.start(GetMethods());
			}

			if (obj.sourceClass != null && methods != null) {
				if (obj != null) {
					int index = 0;
					
					try {
						index = methods
							.Select((v, i) => new { Name = v, Index = i })
							.First(x => x.Name == obj.method)
							.Index;
					} catch {
						index = 0;
					}
					
					obj.method = methods[EditorGUILayout.Popup(index, methods)];
				}
			}


		}
	}
}