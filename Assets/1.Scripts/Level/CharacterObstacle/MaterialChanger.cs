using UnityEngine;

namespace _1.Scripts.Level.CharacterObstacle
{
    public class MaterialChanger : MonoBehaviour
    {
        public Material disabledMaterial;

        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        
        
        
        public void ChangeMaterial(Material material)
        {
            meshRenderer.material = material;
        }
    }
}