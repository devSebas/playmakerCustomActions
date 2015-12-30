// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Set the layer on all children of a GameObject. Optionally filter by component.")]
	public class SetLayerOnChildren : FsmStateAction
	{
		[RequiredField]
		[Tooltip("GameObject Parent")]
		public FsmOwnerDefault
			gameObject;
		[RequiredField]
		[UIHint(UIHint.Layer)]
		[Tooltip("Set layer To...")]
		public int
			layer;
		[UIHint(UIHint.ScriptComponent)]
		[Tooltip("Only set the layer on children with this component.")]
		public FsmString
			filterByComponent;
		
		public override void Reset ()
		{
			gameObject = null;
			layer = 0;
			filterByComponent = null;
		}

		private Type componentFilter;
		
		public override void OnEnter ()
		{
			Setlayer (Fsm.GetOwnerDefaultTarget (gameObject));
			
			Finish ();
		}
		
		void Setlayer (GameObject parent)
		{
			if (parent == null) {
				return;
			}

			if (string.IsNullOrEmpty (filterByComponent.Value)) { // do all children
				foreach (Transform child in parent.transform) {
					child.gameObject.layer = layer;
				}
			} else {
				UpdateComponentFilter ();

				if (componentFilter != null) { // filter by component
					var root = parent.GetComponentsInChildren (componentFilter);
					foreach (var child in root) {
						child.gameObject.layer = layer;
					}
				}
			}

			Finish ();
		}

		void UpdateComponentFilter ()
		{
			componentFilter = ReflectionUtils.GetGlobalType (filterByComponent.Value);

			if (componentFilter == null) { // try adding UnityEngine namespace
				componentFilter = ReflectionUtils.GetGlobalType ("UnityEngine." + filterByComponent.Value);
			}

			if (componentFilter == null) {
				Debug.LogWarning ("Couldn't get type: " + filterByComponent.Value);
			}
		}
	}
}