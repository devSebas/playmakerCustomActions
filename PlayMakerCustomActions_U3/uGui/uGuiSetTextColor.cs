// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using UnityEngine;
using uUI = UnityEngine.UI;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("uGui")]
	[Tooltip("Set text color.")]
	public class uGuiSetTextColor : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(uUI.Image))]
		[Tooltip("The GameObject with the image ui component.")]
		public FsmOwnerDefault gameObject;
		
		[RequiredField]
		[UIHint(UIHint.FsmColor)]
		[Tooltip("Color value to set.")]
		public FsmColor color;

		private uUI.Text _color;

		public override void Reset()
		{
			color = null;
		}
		
		public override void OnEnter()
		{
			
			GameObject _go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (_go!=null)
			{
				_color = _go.GetComponent<uUI.Text>();
			}
			
			DoSetColorValue();

			Finish();
		}
		
		void DoSetColorValue()
		{
			_color.color = color.Value;
		}
		
	}
}