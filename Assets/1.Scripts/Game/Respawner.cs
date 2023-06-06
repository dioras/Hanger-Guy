using System.Collections;
using UnityEngine;
using _1.Scripts.Champion;
using _1.Scripts.GameEvent;
using _1.Scripts.Rope;

namespace _1.Scripts.Game
{
	public class Respawner: MonoBehaviour
	{
		[SerializeField] 
		private RopeLength ropeLength;
		[SerializeField] 
		private RopeCollision ropeCollision;
		private Vector3 deathPos;



		private void Awake()
		{
			EventRepository.ChampionHealthChanged.AddListener(OnChampionHealthChanged);
		}

		private void OnDestroy()
		{
			EventRepository.ChampionHealthChanged.RemoveListener(OnChampionHealthChanged);
		}




		public void Respawn(int partsNum, float invincDuration)
		{
			var championHealth = FindObjectOfType<ChampionHealth>();
		
			championHealth.RestoreParts(partsNum);
			
			this.ropeLength.ResetLength();
			this.ropeCollision.SetRotation(Vector3.zero);
			
			StartCoroutine(SetStartTransformState(championHealth));
			StartCoroutine(Invincibility(invincDuration));
			StartCoroutine(ConnectProcess(championHealth));
		}

		private IEnumerator SetStartTransformState(ChampionHealth championHealth)
		{
			foreach (var rigidbody in championHealth.Rigidbodies)
			{
				rigidbody.velocity = Vector3.zero;
				rigidbody.angularVelocity = Vector3.zero;
				rigidbody.isKinematic = true;
			}
			
			Physics.Raycast(this.deathPos, Vector3.up, out var topHit, 500f, LayerMask.GetMask("Ignore Rope"));
			Physics.Raycast(this.deathPos, -Vector3.up, out var bottomHit, 500f, LayerMask.GetMask("Default"));

			var mediumYTopAndBot = 0f;

			if (bottomHit.transform)
			{
				mediumYTopAndBot = topHit.point.y - (topHit.distance + bottomHit.distance) / 2f;
			}
			else
			{
				mediumYTopAndBot = topHit.point.y - 5f;
			}
			
			championHealth.transform.position = new Vector3(this.deathPos.x, mediumYTopAndBot, this.deathPos.z);
				
			championHealth.GetComponent<CopyTransform>().ApplyStartTransformStateZ();

			yield return new WaitForSeconds(0.2f);
			
			foreach (var rigidbody in championHealth.Rigidbodies)
			{
				rigidbody.isKinematic = false;
			}
		}

		private IEnumerator Invincibility(float duration)
		{
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Base Char"), LayerMask.NameToLayer("Obstacles"), true);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Child Char"), LayerMask.NameToLayer("Obstacles"), true);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Base Char"), LayerMask.NameToLayer("Saw"), true);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Child Char"), LayerMask.NameToLayer("Saw"), true);
			
			yield return new WaitForSeconds(duration);
			
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Base Char"), LayerMask.NameToLayer("Obstacles"), false);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Child Char"), LayerMask.NameToLayer("Obstacles"), false);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Base Char"), LayerMask.NameToLayer("Saw"), false);
			Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Child Char"), LayerMask.NameToLayer("Saw"), false);
		}

		private IEnumerator ConnectProcess(ChampionHealth championHealth)
		{
			while (!this.ropeLength.ScaleUp(1000, LayerMask.GetMask("Ignore Rope")))
			{
				yield return new WaitForFixedUpdate();
			}

			this.ropeCollision._isFirst = true;
		}
		
		private void OnChampionHealthChanged(ChampionHealth championHealth, int health, int prevHealth)
		{
			if (health == 0)
			{
				this.deathPos = championHealth.GetComponent<Collider>().bounds.center;
			}
		}
	}
}