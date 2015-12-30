// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Rotates a GameObject based on mouse movement with smooth damp! ;) Minimum and Maximum values can be used to constrain the rotation.")]
	public class MouseLookSmooth : FsmStateAction
	{
		public enum RotationAxes
		{
			MouseXAndY = 0,
			MouseX = 1,
			MouseY = 2
		}

		[RequiredField]
		[Tooltip("The GameObject to rotate.")]
		public FsmOwnerDefault
			gameObject;
		[Tooltip("The axes to rotate around.")]
		public RotationAxes
			axes = RotationAxes.MouseXAndY;
		[RequiredField]
		public FsmFloat
			sensitivityX;
		[RequiredField]
		public FsmFloat
			sensitivityY;
		[Tooltip("Clamp rotation around X axis.")]
		public FsmFloat
			minimumX;
		[Tooltip("Clamp rotation around X axis.")]
		public FsmFloat
			maximumX;
		[Tooltip("Clamp rotation around Y axis.")]
		public FsmFloat
			minimumY;
		[Tooltip("Clamp rotation around Y axis.")]
		public FsmFloat
			maximumY;
		public float dampingTime = 0.1f;
		[Tooltip("Repeat every frame.")]
		public bool
			everyFrame;
		Quaternion originalRotation;
		Vector3 followVelocity;
		Vector3 targetAngles;
		Vector3 followAngles;

		public override void Reset ()
		{
			gameObject = null;
			axes = RotationAxes.MouseXAndY;
			sensitivityX = 15f;
			sensitivityY = 15f;
			minimumX = -80f;
			maximumX = 80;
			minimumY = -360;
			maximumY = 360;
			dampingTime = 0.1f;
			everyFrame = true;
		}

		public override void OnEnter ()
		{

			var go = Fsm.GetOwnerDefaultTarget (gameObject);

			originalRotation = go.transform.localRotation;

			if (go == null) {
				Finish ();
				return;
			}

			// Make the rigid body not change rotation
			// TODO: Original Unity script had this. Expose as option?
			if (go.rigidbody) {
				go.rigidbody.freezeRotation = true;
			}

			DoMouseLook ();

			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			DoMouseLook ();
		}

		void DoMouseLook ()
		{

			var go = Fsm.GetOwnerDefaultTarget (gameObject);

			if (go == null) {
				return;
			}

			var transform = go.transform;

			switch (axes) {
			case RotationAxes.MouseXAndY:
					
				transform.localEulerAngles = new Vector3 (GetYRotation (), GetXRotation (), 0);
				break;
				
			case RotationAxes.MouseX:

				transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, GetXRotation (), 0);
				break;

			case RotationAxes.MouseY:

				transform.localEulerAngles = new Vector3 (GetYRotation (), transform.localEulerAngles.y, 0);
				break;
			}
			transform.localRotation = originalRotation;

			if (targetAngles.y > 180) {
				targetAngles.y -= 360;
				followAngles.y -= 360;
			}
			if (targetAngles.x > 180) {
				targetAngles.x -= 360;
				followAngles.x -= 360;
			}
			if (targetAngles.y < -180) {
				targetAngles.y += 360;
				followAngles.y += 360;
			}
			if (targetAngles.x < -180) {
				targetAngles.x += 360;
				followAngles.x += 360;
			}

			// smoothly interpolate current values to target angles
			followAngles = Vector3.SmoothDamp (followAngles, targetAngles, ref followVelocity, dampingTime);

			// update the actual gameobject's rotation
			go.transform.localRotation = originalRotation * Quaternion.Euler (-followAngles.x, followAngles.y, 0);

		}

		float GetXRotation ()
		{
			targetAngles.x += Input.GetAxis ("Mouse Y") * sensitivityX.Value;
			targetAngles.x = Mathf.Clamp (targetAngles.x, minimumX.Value, maximumX.Value);

			return targetAngles.x;
		}

		float GetYRotation ()
		{
			targetAngles.y += Input.GetAxis ("Mouse X") * sensitivityY.Value;
			targetAngles.y = Mathf.Clamp (targetAngles.y, minimumY.Value, maximumY.Value);

			return targetAngles.y;
		}
	}
}