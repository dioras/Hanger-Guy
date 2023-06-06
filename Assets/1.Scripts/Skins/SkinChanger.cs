using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.Level.CharacterObstacle;
using _1.Scripts.Rope;

namespace _1.Scripts.Skins
{
	[Serializable]
	public class SkinBody
	{
		[field:SerializeField]
		public Skin Skin { get; set; }
		[field:SerializeField]
		public GameObject Body { get; set; }
	}

	public class SkinChanger: MonoBehaviour
	{
		public static SkinChanger Instance { get; private set; }
	
		[SerializeField] private RopeLength ropeLength;
		[SerializeField] private RopeCollision ropeCollision;
		[SerializeField] private FollowCamera followCamera;
		[SerializeField] private List<SkinBody> skinBodies;
		private Skin currentSkin;
		



		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}
			else
			{
				if (Instance != this)
				{
					Destroy(this.gameObject);
				}

				return;
			}
		}

		private void Start()
		{
			this.currentSkin = PlayerSkinService.Instance.CurrentSkin;
			SetSkin(this.currentSkin);
			
			var currSkinBody = this.skinBodies.Single(s => s.Skin == this.currentSkin).Body;
			this.followCamera.Target = currSkinBody.transform;
		}
		
		


		public void SetSkin(Skin skin)
		{
			var prevSkin = this.currentSkin;
			this.currentSkin = skin;
			
			var currSkinBody = GetSkinBody(this.currentSkin);

			if (prevSkin != null)
			{
				var prevSkinBody = GetSkinBody(prevSkin);

				currSkinBody.transform.position = prevSkinBody.transform.position;
				currSkinBody.transform.rotation = prevSkinBody.transform.rotation;
				
				prevSkinBody.SetActive(false);
			}
			
			currSkinBody.GetComponent<CopyTransform>().ApplyStartTransformStateZ();

			currSkinBody.SetActive(true);

			var ropeConnectable = currSkinBody.GetComponent<IRopeConnectable>();

			this.ropeLength.SetBody(ropeConnectable);
			this.ropeCollision.SetBody(ropeConnectable);
			
			var layoutControllers = FindObjectsOfType<LayoutController>().ToList();

			var currSkinRigidbody = currSkinBody.GetComponent<Rigidbody>();
			
			layoutControllers.ForEach(c => c.SetBody(currSkinRigidbody));
		}

		public GameObject GetSkinBody(Skin skin)
		{
			return this.skinBodies.SingleOrDefault(s => s.Skin == skin)?.Body;
		}
	}
}