using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;

namespace MyFirstARGame
{
    public class PlayerBox : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials;
        public int playerID;
        public GameObject snowball;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Awake()
        {
            var photonView = this.transform.GetComponent<PhotonView>();
            playerID = Mathf.Max((int)photonView.InstantiationData[0], 0);
        
            if (this.materials.Length > 0)
            {
                var material = this.materials[playerID % this.materials.Length];
                this.transform.GetComponent<Renderer>().material = material;
            }
        }

        public Vector3 GetForwardHoldingPosition()
        {
            return transform.position + transform.forward * 1.0f;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (snowball != null)
            {
                snowball.transform.SetPositionAndRotation(GetForwardHoldingPosition(), transform.rotation);
                snowball.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.PickedUp;
            }
        }

        public void TakeSnowball()
        {
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            snowball = PhotonNetwork.Instantiate("Projectile", GetForwardHoldingPosition(), transform.rotation, data: initialData);
            var rigidbody = snowball.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
        }

        public void ShootSnowball(Vector3 direction, float initialSpeed)
        {
            if (snowball == null) return;

            direction = transform.forward;

            snowball.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.Attack;
            var rigidbody = snowball.GetComponent<Rigidbody>();
            rigidbody.velocity = direction * initialSpeed;
            rigidbody.useGravity = true;

            snowball = null;
        }

        public void TakeDamage()
        {
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Snowball"))
            {
                var ball = collision.collider.GetComponent<ProjectileBehaviour>();
                var photonView = collision.collider.GetComponent<PhotonView>();

                if (ball.state == ProjectileBehaviour.State.OnGround && this.snowball == null)
                {
                    TakeSnowball();
                    photonView.RPC("DestroyProjectile", RpcTarget.All);
                }
                else if (ball.state == ProjectileBehaviour.State.Attack)
                {
                    TakeDamage();
                    photonView.RPC("DestroyProjectile", RpcTarget.All);
                }
            }
        }
    }
}
