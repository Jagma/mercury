using UnityEngine;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using System.Linq;
using System.Reflection;

namespace SupportClasses {
	public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
			
		public delegate void OnLeftClickEvent();
		public event OnLeftClickEvent OnLeftClick;
		public delegate void OnRightClickEvent();
		public event OnRightClickEvent OnRightClick;

		public UnityEngine.Object sourceClass;
		public string method;

		public void Construct (OnLeftClickEvent leftAction, OnRightClickEvent rightAction) {
			OnLeftClick += leftAction;
			OnRightClick += rightAction;
		}

		public void OnPointerEnter(PointerEventData eventData) {

		}

		public void OnPointerExit(PointerEventData eventData) {

		}

		public void OnPointerClick(PointerEventData eventData) {
			if (eventData.button == PointerEventData.InputButton.Left) {
				//AudioSource.PlayClipAtPoint(SoundManager.menu, Camera.main.transform.position, 0.5f);
				if (OnLeftClick != null) {
					OnLeftClick.Invoke();
				}
			} else if (eventData.button == PointerEventData.InputButton.Right) {
				if (OnRightClick != null) {
					OnRightClick.Invoke();
				}
			}

			if (method != null && method != "") {
				//System.Type.GetType(sourceClass.name).GetMethod(method).Invoke(method, );
				System.Type.GetType(sourceClass.name).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Invoke(sourceClass, new object[0]);
			}
		}
	}
}