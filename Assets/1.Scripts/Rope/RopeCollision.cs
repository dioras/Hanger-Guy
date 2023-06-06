using UnityEngine;
using _1.Scripts.Champion;

namespace _1.Scripts.Rope
{
    public class RopeCollision : MonoBehaviour
    {
        public bool IsObstacle { get; private set; }
        
        [SerializeField] private Vector3 firstForceDisconnect = new Vector3(50f, 0f, 0f);
        [SerializeField] private GameObject hitContact;
        [SerializeField] private Vector3 offsetPointContact;
        [SerializeField] private HingeJoint startJoint;
        [SerializeField] private IRopeConnectable ropeConnectable;
        
        private Joint _joint;
        public bool _isFirst = true;
        private new Rigidbody rigidbody;




        private void Awake()
        {
            this.rigidbody = GetComponent<Rigidbody>();
            
            ConnectObstacle(new Vector3(-1.018023f, 6.075849f, 0.9870001f));
        }

        private void Update()
        {
            if (this._joint)
            {
                this._joint.connectedAnchor = this._joint.connectedAnchor;
            }
        }




        public void Connect(Vector3 point)
        {
            ConnectObstacle(point);
            Destroy(Instantiate(this.hitContact, point + this.offsetPointContact, Quaternion.identity), 1f); // VFX
        }
        
        public void Disconnect()
        {
            this.IsObstacle = false;

            if (this._joint)
            {
                Destroy(this._joint);
            }

            if (this._isFirst)
            {
                this._isFirst = false;

                this.ropeConnectable.BodyForRopeConnection.AddForce(this.firstForceDisconnect, ForceMode.Impulse);
            }
        }
        
        public void SetRotation(Vector3 rotation)
        {
            this.rigidbody.freezeRotation = true;
            this.transform.rotation = Quaternion.Euler(rotation);
        }

        public void ResetRotation()
        {
            this.rigidbody.freezeRotation = true;
            this.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
        }

        public void SetBody(IRopeConnectable ropeConnectable)
        {
            this.startJoint.connectedBody = ropeConnectable.BodyForRopeConnection;
            this.ropeConnectable = ropeConnectable;
        }

        public void SetStartJointAnchor(Vector3 anchor)
        {
            this.startJoint.anchor = anchor;
        }
        
        public void SetStartJointConnectedAnchor(Vector3 anchor)
        {
            this.startJoint.connectedAnchor = anchor;
        }

        private void ConnectObstacle(Vector3 point)
        {
            this.IsObstacle = true;
            
            this._joint = this.gameObject.AddComponent<HingeJoint>();
            
            this._joint.anchor = new Vector3(0f, .02f, 0f);
            this._joint.axis = new Vector3(0f, 0f, 1f);
            this._joint.autoConfigureConnectedAnchor = false;
            this._joint.connectedAnchor = point;

            this.rigidbody.freezeRotation = false;
        }
    }
}