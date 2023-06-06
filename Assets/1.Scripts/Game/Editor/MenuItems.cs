using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using _1.Scripts.Perks;
using _1.Scripts.Skins;

namespace _1.Scripts.Game.Editor
{
	public class BonesPair
	{
		public string TrName { get; set; }
		public string RagdollName { get; set; }


		public BonesPair(string trName, string ragdollName)
		{
			TrName = trName;
			RagdollName = ragdollName;
		}
	}

	public class MenuItems
	{
		private static readonly BonesPair[] BonesPairs = 
		{
			new BonesPair("hip", "pelvis"),
			new BonesPair("upperLeg.L", "leftHips"),
			new BonesPair("lowerLeg.L", "leftKnee"),
			new BonesPair("foot.L", "leftFoot"),
			new BonesPair("upperLeg.R", "rightHips"),
			new BonesPair("lowerLeg.R", "rightKnee"),
			new BonesPair("foot.R", "rightFoot"),
			new BonesPair("upperArm.L", "leftArm"),
			new BonesPair("lowerArm.L", "leftElbow"),
			new BonesPair("upperArm.R", "rightArm"),
			new BonesPair("lowerArm.R", "rightElbow"),
			new BonesPair("stomach", "middleSpine"),
			new BonesPair("head", "head")
		};
	
		[MenuItem("Tools/Clear PlayerPrefs")]
		private static void ClearPlayerPrefs()
		{
			PlayerPrefs.DeleteAll();

			var skinsPaths = Directory.GetFiles($"{Application.dataPath}/2.Content/Skins/", "*.asset",
				SearchOption.AllDirectories);

			for (var i = 0; i < skinsPaths.Length; i++)
			{
				skinsPaths[i] = "Assets" + skinsPaths[i].Replace('\\', '/' )
					                .Replace(Application.dataPath, "");
			}

			foreach (var skinPath in skinsPaths)
			{
				var skin = AssetDatabase.LoadAssetAtPath<Skin>(skinPath);

				if (skin.Id == 0)
				{
					continue;
				}
				
				skin.CurrentViewCount = 0;
			}
			
			Debug.Log("PlayerPrefs cleared.");
		}
		
		
		[MenuItem("Tools/Add bones")]
		public static void AddBonesToRagdoll()
		{
			var windowName = "Create Ragdoll";

			var window = EditorWindow.GetWindow<EditorWindow>(windowName);

			if (!window.GetType().Name.Equals("RagdollBuilder"))
			{
				Debug.LogError("Open \"Create Ragdoll\" window!");
				return;
			}

			window.Show();

			var trs = Selection.activeGameObject.transform.GetComponentsInChildren<Transform>();

			foreach (var tr in trs)
			{
				var pair = BonesPairs.FirstOrDefault(p => p.TrName.Equals(tr.name));

				if (pair == null)
				{
					continue;
				}

				var boneName = pair.TrName;

				var fieldName = BonesPairs.First(p => p.TrName.Equals(boneName)).RagdollName;

				foreach (var field in window.GetType().GetFields())
				{
					if (field.Name.Equals(fieldName))
					{
						field.SetValue(window, tr);
					}
				}
			}
		}

		[MenuItem("Tools/Replace Renderer")]
		public static void ReplaceSkinnedOnMeshRenderer()
		{
			foreach (var gameObject in Selection.gameObjects)
			{
				var skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();

				if (skinnedMeshRenderer == null)
				{
					Debug.LogError($"Skinned mesh renderer not found! {gameObject.name}");

					continue;
				}

				var meshFilter = ObjectFactory.AddComponent<MeshFilter>(gameObject);

				meshFilter.mesh = skinnedMeshRenderer.sharedMesh;

				var meshRenderer = ObjectFactory.AddComponent<MeshRenderer>(gameObject);

				meshRenderer.sharedMaterial = skinnedMeshRenderer.sharedMaterial;

				Object.DestroyImmediate(skinnedMeshRenderer);
			}
		}

		[MenuItem("Tools/SetRightHandToMagnetPerk")]
		public static void SetRightHandToMagnetPerk()
		{
			foreach (var gameObject in Selection.gameObjects)
			{
				var magnetPerk = gameObject.GetComponent<MagnetPerk>();

				var magnetParentField = magnetPerk.GetType().GetProperty("MagnetParent");

				Transform handR = null;

				var trs = gameObject.transform.GetComponentsInChildren<Transform>();

				foreach (var tr in trs)
				{
					if (tr.name.Equals("hand.R"))
					{
						handR = tr;

						break;
					}
				}

				if (magnetParentField == null)
				{
					Debug.LogError($"Hand R not found! {gameObject.name}");

					continue;
				}
				
				magnetParentField.SetValue(magnetPerk, handR);
			}
		}
		
		public static T GetReference<T>(object inObj, string fieldName) where T : class
		{
			return GetField(inObj, fieldName) as T;
		}


		public static T GetValue<T>(object inObj, string fieldName) where T : struct
		{
			return (T)GetField(inObj, fieldName);
		}


		public static void SetField(object inObj, string fieldName, object newValue)
		{
			FieldInfo info = inObj.GetType().GetField(fieldName);
			if (info != null)
				info.SetValue(inObj, newValue);
		}


		private static object GetField(object inObj, string fieldName)
		{
			object ret = null;
			FieldInfo info = inObj.GetType().GetField(fieldName);
			if (info != null)
				ret = info.GetValue(inObj);
			return ret;
		}
	}
}