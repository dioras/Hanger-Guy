using UnityEngine;
using _1.Scripts.Champion;

namespace _1.Scripts.Rope
{
    public class RopeLength : MonoBehaviour
    {
        public bool IsClick { get; private set; }
        
        [SerializeField] private float growthDelta = 1f;
        //Максимальная скорость поднятия веревки
        [SerializeField] private float maxSpeed = 75f;
        //Минимальная скорость поднятия веревки
        [SerializeField] private float minSpeed = 20f;
        //Скорость поднятия веревки
        [SerializeField] private float ropeSpeed;
        //Максимальная скорость персонажа
        [SerializeField] private float maxCharacterSpeed;
        [SerializeField] private IRopeConnectable ropeConnectable;
        [SerializeField] private float mixScale = 50f;
        [SerializeField] private RopeCollision RopeCollision;

        [SerializeField] private Transform tr;
        [SerializeField] private float rayDistance;
        [SerializeField] private float rayBound = .5f;
        
        private bool _a;
        private new Rigidbody rigidbody;




        private void OnEnable()
        {
            this.IsClick = true;
            this._a = false;
            
            this.RopeCollision.Disconnect();
            ResetLength();
        }

        private void Awake()
        {
            this.rigidbody = GetComponent<Rigidbody>();
            
            this.ropeSpeed = maxSpeed;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.RopeCollision.ResetRotation();
            }
            if (Input.GetMouseButton(0))
            {
                this.IsClick = true;                
                
                if (!this.RopeCollision.IsObstacle & !_a)
                {
                    ScaleUp(this.growthDelta, LayerMask.GetMask("Ignore Rope", "Obstacles"));
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.IsClick = false;
                this._a = false;
                
                this.RopeCollision.Disconnect();
                ResetLength();
            }

            this.ropeSpeed = CalculateRopeSpeed();

            if (NeedScaleDown())
            {
                Scale(-this.ropeSpeed);
            }
        }



        public void SetBody(IRopeConnectable ropeConnectable)
        {
            this.ropeConnectable = ropeConnectable;
        }

        public bool NeedScaleDown()
        {
            return this.transform.localScale.y > this.mixScale && this.RopeCollision.IsObstacle;
        }

        public bool ScaleUp(float scaleSpeed, LayerMask layerMask)
        {
            if (!this.RopeCollision.IsObstacle)
            {
                Scale(scaleSpeed);
                    
                return TryConnect(this.tr.position, this.transform.up, layerMask);
            }

            return false;
        }

        public bool TryConnect(Vector3 origin, Vector3 dir, LayerMask layerMask)
        {
            var ray = new Ray(origin, dir);
            
            if (Physics.Raycast(ray.origin, ray.direction, out var hitInfo, this.rayDistance, layerMask) 
                && hitInfo.distance <= this.rayBound) 
            {
                this._a = true;
                this.RopeCollision.Connect(hitInfo.point);

                return true;
            }

            return false;
        }
        
        public void Scale(float speed)
        {
            var scale = this.transform.localScale;
            
            scale.y += speed * Time.fixedDeltaTime;

            this.transform.localScale = scale;
        }

        public void ResetLength()
        {
            this.transform.localScale = new Vector3(1f, .01f, 1f);;
        }

        public float CalculateRopeSpeed()
        {
            if (this.ropeConnectable.BodyForRopeConnection.velocity.magnitude <= 0)
            {
                if (this.ropeSpeed <= this.maxSpeed)
                {
                    this.ropeSpeed = this.maxSpeed;
                }
            }
            else if (this.ropeConnectable.BodyForRopeConnection.velocity.magnitude > 0 
                & this.ropeConnectable.BodyForRopeConnection.velocity.magnitude < this.maxCharacterSpeed)
            {
                //Вычесление скорости поднятия веревки: МинСкорВеревки * 100% /(СкорПерсонажа * 100% / макСкорПерсонажа)
                //this.minSpeed * 100 / (this.Character.velocity.magnitude * 100 / this.maxCharacterSpeed);
                this.ropeSpeed = Mathf.Lerp(this.minSpeed, this.maxSpeed, this.ropeConnectable.BodyForRopeConnection.velocity.magnitude / 10f);
            }
            else
            {
                this.ropeSpeed = this.minSpeed;
            }

            return this.ropeSpeed;
        }
    }
}