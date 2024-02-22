using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UIElements;

namespace MyFirstARGame
{
    public class PlayerBox : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials;
        public int playerID;
        public GameObject snowball;
        public GameObject shield;
        public bool hasSnowBall = false;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Awake()
        {
            var photonView = this.transform.GetComponent<PhotonView>();
            // photon player id
            playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        
            if (this.materials.Length > 0)
            {
                var material = this.materials[playerID % this.materials.Length];
                this.transform.GetComponent<Renderer>().material = material;
            }
        }

        public Vector3 GetSnowballHoldingPosition()
        {
            return transform.position + transform.forward * 1.0f;
        }

        public Vector3 GetShieldHoldingPosition()
        {
            return transform.position + transform.forward * 0.5f;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (snowball != null)
            {
                Vector3 pos = GetSnowballHoldingPosition();
                PhotonView photonView = this.GetComponent<PhotonView>();
                photonView.RPC("UpdateSnowBall", photonView.Controller, pos);
                //snowball.transform.SetPositionAndRotation(GetSnowballHoldingPosition(), transform.rotation);
                //snowball.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.PickedUp;
            }

            if (shield != null)
            {
                Vector3 pos = GetShieldHoldingPosition();
                PhotonView photonView = this.GetComponent<PhotonView>();
                photonView.RPC("UpdateShield", RpcTarget.All, pos, transform.rotation);
                //var rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                //shield.transform.SetPositionAndRotation(GetShieldHoldingPosition(), transform.rotation * rotation);
                //shield.GetComponent<Shield>().state = Shield.State.PickedUp;
            }
        }

        public void TakeSnowball()
        {
            hasSnowBall = true;
            //var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            //snowball = PhotonNetwork.Instantiate("Projectile", GetSnowballHoldingPosition(), transform.rotation, data: initialData);
            //var rigidbody = snowball.GetComponent<Rigidbody>();
            //rigidbody.useGravity = false;

            //FindObjectOfType<NetworkCommunication>().GetComponent<NetworkCommunication>().DecrementSnowball();

            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.SetHasSnowBall(true);
        }

        public void TakeShield(GameObject shield)
        {
            //this.shield = shield;
            shield.GetComponent<PhotonView>().RPC("DestroyShield", RpcTarget.All);
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.SetHasShield(true);
            //this.shield.GetComponent<Shield>().state = Shield.State.PickedUp;
        }

        public void ShootSnowball(Vector3 direction, float initialSpeed)
        {
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            bool hasBall = networkCommunication.getHasSnowBall();
            if (!hasBall) return;

            PhotonView photonView = snowball.GetComponent<PhotonView>();
            photonView.RPC("DestroyProjectile", photonView.Controller);
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            // Cast a ray from the touch point to the world. We use the camera position as the origin and the ray direction as the
            // velocity direction.

            //var projectile = PhotonNetwork.Instantiate("Projectile", GetSnowballHoldingPosition(), Quaternion.identity, data: initialData);
            //var rigidbody = projectile.GetComponent<Rigidbody>();
            //rigidbody.velocity = direction * initialSpeed;

            hasSnowBall = false;
            snowball = null;
        }

        public void TakeDamage(int id)
        {
            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            var player = $"Player {id}";
            bool shieldNum = networkCommunication.GetComponent<Scoreboard>().GetHasShield(player);
            if (shieldNum)
            {
                networkCommunication.GetComponent<PhotonView>().RPC("Network_SetHasShield", RpcTarget.All, player, false);
                return;
            }
            var currentLife = networkCommunication.GetComponent<Scoreboard>().GetLife(player);
            networkCommunication.GetComponent<PhotonView>().RPC("Network_SetPlayerLife", RpcTarget.All, player, currentLife - 1);
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
                    TakeDamage(GetComponent<PhotonView>().Owner.ActorNumber);
                    photonView.RPC("DestroyProjectile", RpcTarget.All);
                }
            }
            else if (collision.collider.CompareTag("Shield"))
            {
                var shield = collision.collider.GetComponent<Shield>();
                var photonView = collision.collider.GetComponent<PhotonView>();

                if (shield.state == Shield.State.OnGround)
                {
                    TakeShield(collision.collider.gameObject);
                }
            }
        }

        [PunRPC]
        public void UpdateSnowBall(Vector3 pos) {
            if (GetComponent<PhotonView>().IsMine)
            {
                snowball.transform.position = pos;
                snowball.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.PickedUp;
            }
        }

        [PunRPC]
        public void UpdateShield(Vector3 pos, Quaternion playerRotation)
        {
            if (GetComponent<PhotonView>().IsMine) {
                var rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                shield.transform.SetPositionAndRotation(pos, playerRotation * rotation);
                shield.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.PickedUp;
            }
            
        }

        [PunRPC]
        public void ShootSnowBallRPC(Vector3 direction, float initialSpeed, Vector3 forward)
        {
            direction = forward;

            snowball.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.Attack;
            var rigidbody = snowball.GetComponent<Rigidbody>();
            rigidbody.velocity = direction * initialSpeed;
            rigidbody.useGravity = true;
        }
    }
}
