using _1.Scripts.Champion;
using _1.Scripts.Helper;
using _1.Scripts.Vfxs;
using UnityEngine;

namespace _1.Scripts.Level.CharacterObstacle
{
    public class CharacterObstacle : MonoBehaviour
    {
        [SerializeField] private VfxActivator vfxActivator;
        [SerializeField] private MaterialChanger materialChanger;
        [SerializeField] private RagdollController ragdollController;
        [SerializeField] private DestroyWithDelay destroyWithDelay;
        
        
        
        public void Deactivate(Vector3 direction)
        {
            ragdollController.EnableRagdoll(true);
            ragdollController.AddForce(direction);
            vfxActivator.Activate();
            materialChanger.ChangeMaterial(materialChanger.disabledMaterial);
            destroyWithDelay.enabled = true;
        }
    }
}