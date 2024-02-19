namespace MyFirstARGame
{
    using UnityEngine;
    using Photon.Pun;

    /// <summary>
    /// Controls projectile behaviour. In our case it currently only changes the material of the projectile based on the player that owns it.
    /// </summary>
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Material[] projectileMaterials;

        public float deathTime = 10.0f;
        public int playerNumber = 0;
        private float timer = 0;

        void Start()
        {
            GetComponent<Renderer>().material.color = Color.white;
        }

        void FixedUpdate()
        {
            if (timer > deathTime)
                Die();

            var color = GetComponent<Renderer>().material.color;
            color.a = Mathf.SmoothStep(1, 0, timer / deathTime);
            GetComponent<Renderer>().material.color = color;

            timer += Time.deltaTime;
        }

        private void Awake()
        {
            // Pick a material based on our player number so that we can distinguish between projectiles. We use the player number
            // but wrap around if we have more players than materials. This number was passed to us when the projectile was instantiated.
            // See ProjectileLauncher.cs for more details.
            var photonView = this.transform.GetComponent<PhotonView>();
            var playerId = Mathf.Max((int)photonView.InstantiationData[0], 0);
            playerNumber = (int)playerId;
            if (this.projectileMaterials.Length > 0)
            {
                var material = this.projectileMaterials[playerId % this.projectileMaterials.Length];
                this.transform.GetComponent<Renderer>().material = material;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("SnowFlake") && PhotonNetwork.LocalPlayer.ActorNumber == playerNumber)
            {
                var networkCommunication = FindObjectOfType<NetworkCommunication>();
                networkCommunication.IncrementScore();
                PhotonNetwork.Destroy(collision.gameObject);
                Die();
            }
            else if (collision.collider.CompareTag("Shield") && PhotonNetwork.LocalPlayer.ActorNumber == playerNumber) {
                var networkCommunication = FindObjectOfType<NetworkCommunication>();
                networkCommunication.IncrementShield();
                this.transform.GetComponent<PhotonView>();
                PhotonNetwork.Destroy(collision.gameObject);
                Die();
            }
        }

        private void Die()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
