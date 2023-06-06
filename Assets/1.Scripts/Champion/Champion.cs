using UnityEngine;

namespace _1.Scripts.Champion
{
    public class Champion : MonoBehaviour, IRopeConnectable
    {
        [field: SerializeField] public Rigidbody BodyForRopeConnection { get; set; }
    }
}