// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/
using System;
using UnityEngine;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Get Terrain Splat Texture Map Name In A Game Object Position.")]
	public class GetTerrainTexture : FsmStateAction
	{
		[RequiredField]
		public FsmOwnerDefault
			texturePosition;
		[RequiredField]
		public Terrain
			terrain;
		public FsmString mainTexture;
		
		public override Void Reset ()
		{
			texturePosition = null;
			terrain = null;
			mainTexture = null;
		}
		
		public override Void OnEnter ()
		{
			
			GetTexture ();
			Finish ();
		}
		
		public void GetTexture ()
		{
			var go = Fsm.GetOwnerDefaultTarget (texturePosition); // our game object
			
			TerrainData terrainData = terrain.terrainData; //terrain data
			var terrainPos = terrain.transform.position; //terrain position
			
			float x_floated = ((go.transform.position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth; // get terrain x
			float z_floated = ((go.transform.position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight; //get terrain y
			
			Debug.Log (x_floated);
			Debug.Log (z_floated);
			
			if (x_floated < 0.0f || z_floated < 0.0f) {
				mainTexture.Value = null;
				Debug.Log ("Less Than 0");
				Finish ();
			} else if (x_floated > terrainData.size.x * 10 || z_floated > terrainData.size.z * 10) {
				mainTexture.Value = null;
				Debug.Log ("Too Much Big");
				Finish ();
			} else {
				int x = Mathf.RoundToInt (x_floated);
				int z = Mathf.RoundToInt (z_floated);
				
				float [,,] splaptexture = terrainData.GetAlphamaps (x, z, 1, 1);
				
				for (var i = 0; i < terrainData.splatPrototypes.Length; i ++) { //for each splat map in terrain get his intensity value
					if (splaptexture [0, 0, i] > 0.5f) { // if splat map intensity is more than 0.5f we got our the main splat map
						mainTexture.Value = terrainData.splatPrototypes [i].texture.name; //set play maker texture value
					}
				}
			}
		}
	}
}