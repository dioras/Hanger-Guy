using System;
using System.Collections.Generic;
using UnityEngine;

namespace _1.Scripts.Champion
{
	public class StartTransformState
	{
		public Rigidbody Rigidbody { get; set; }
		public Vector3 StartPosition { get; set; }
		public Quaternion StartRotation { get; set; }
		public float StartMass { get; set; }
	}

	public class CopyTransform: MonoBehaviour
	{
		private List<StartTransformState> startTransformsState;
		
		
	
	
		private void Awake()
		{
			var rigidbodies = GetComponentsInChildren<Rigidbody>();

			startTransformsState = new List<StartTransformState>(rigidbodies.Length);

			foreach (var rigidbody in rigidbodies)
			{
				startTransformsState.Add(new StartTransformState
				{
					Rigidbody = rigidbody,
					StartPosition = rigidbody.transform.localPosition,
					StartRotation = rigidbody.transform.localRotation,
					StartMass = rigidbody.mass
				});
			}
		}
		
		

		public void ApplyStartTransformState()
		{
			foreach (var startTransformState in startTransformsState)
			{
				startTransformState.Rigidbody.transform.localPosition = startTransformState.StartPosition;
				startTransformState.Rigidbody.transform.localRotation = startTransformState.StartRotation;
			}
		}
		
		public void ApplyStartTransformStateZ()
		{
			if (startTransformsState == null)
			{
				return;
			}
		
			foreach (var startTransformState in startTransformsState)
			{
				if (startTransformState.Rigidbody.gameObject.layer == LayerMask.NameToLayer("Base Char"))
				{
					startTransformState.Rigidbody.transform.localPosition = new Vector3(startTransformState.Rigidbody.transform.localPosition.x,
						startTransformState.Rigidbody.transform.localPosition.y, startTransformState.StartPosition.z);
					startTransformState.Rigidbody.transform.localRotation = startTransformState.StartRotation;
					startTransformState.Rigidbody.mass = startTransformState.StartMass;
				}
				else
				{
					startTransformState.Rigidbody.transform.localPosition = startTransformState.StartPosition;
					startTransformState.Rigidbody.transform.localRotation = startTransformState.StartRotation;
					startTransformState.Rigidbody.mass = startTransformState.StartMass;
				}
			}
		}
	}
}