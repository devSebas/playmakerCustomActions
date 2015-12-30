// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the layer and the layer of its children too")]
	public class SetTagRecursive : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault
			gameObject;
		[RequiredField]
		[UIHint(UIHint.Tag)]
		[Tooltip("Set Tag To...")]
		public FsmString
			tag;
		public bool children;

		public override void Reset ()
		{
			gameObject = null;
			tag = null;
			children = false;
		}
				
		public override void OnEnter ()
		{
			DOSetTag ();	
		}
		
		public void DOSetTag ()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget (gameObject);
			if (go == null)
				return;
			go.tag = tag.Value;
			
			if (children) {
				foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
		
					trans.gameObject.tag = tag.Value;
		
				}
			}
			Finish ();
		}
	}
}