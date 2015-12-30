// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Flips the value of a Bool Variable in another FSM.")]
	public class FsmBoolFlip : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault
			gameObject;
		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString
			fsmName;
		[RequiredField]
		[UIHint(UIHint.FsmBool)]
		[Tooltip("The name of the FSM variable.")]
		public FsmString
			variableName;
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool
			everyFrame;
		GameObject goLastFrame;
		PlayMakerFSM fsm;
		
		public override void Reset ()
		{
			gameObject = null;
			fsmName = "";
		}

		public override void OnEnter ()
		{
			DoSetFsmBool ();
			
			if (!everyFrame) {
				Finish ();
			}		
		}

		void DoSetFsmBool ()
		{
			var go = Fsm.GetOwnerDefaultTarget (gameObject);
			if (go == null) {
				return;
			}
			
			if (go != goLastFrame) {
				goLastFrame = go;
				
				// only get the fsm component if go has changed
				
				fsm = ActionHelpers.GetGameObjectFsm (go, fsmName.Value);
			}			
			
			if (fsm == null) {
				LogWarning ("Could not find FSM: " + fsmName.Value);
				return;
			}
			
			var fsmBool = fsm.FsmVariables.FindFsmBool (variableName.Value);
			
			if (fsmBool != null) {
				fsmBool.Value = !fsmBool.Value;
			} else {
				LogWarning ("Could not find variable: " + variableName.Value);
			}
		}
		
		public override void OnUpdate ()
		{
			DoSetFsmBool ();
		}
		
	}
}