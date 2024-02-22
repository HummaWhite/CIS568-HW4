namespace MyFirstARGame
{
    using UnityEngine;
    using Photon.Pun;

    /// <summary>
    /// Launches projectiles from a touch point with the specified <see cref="initialSpeed"/>.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ProjectileLauncher : PressInputBase
    {
        [SerializeField]
        private Rigidbody projectilePrefab;

        [SerializeField]
        private float initialSpeed = 25;

        public ProjectileBehaviour projectBehavior;

        protected override void OnPressBegan(Vector3 position)
        {
            if (this.projectilePrefab == null || !NetworkLauncher.Singleton.HasJoinedRoom)
                return;

            // Ensure user is not doing anything else.
            var uiButtons = FindObjectOfType<UIButtons>();
            if (uiButtons != null && (uiButtons.IsPointOverUI(position) || !uiButtons.IsIdle))
                return;

            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            var networkCommunication = FindObjectOfType<NetworkCommunication>();
            bool hasSnowBall = networkCommunication.getHasSnowBall();

            if (hasSnowBall) {
                var ray = this.GetComponent<Camera>().ScreenPointToRay(position);
                //var player = FindObjectOfType<PlayerBox>();
                //player.ShootSnowball(ray.direction, initialSpeed);
                

                var projectile = PhotonNetwork.Instantiate(this.projectilePrefab.name, ray.origin + ray.direction * 1.0f, Quaternion.identity, data: initialData);
                var rigidbody = projectile.GetComponent<Rigidbody>();
                projectile.GetComponent<ProjectileBehaviour>().state = ProjectileBehaviour.State.Attack;
                rigidbody.velocity = ray.direction * initialSpeed;

                networkCommunication.SetHasSnowBall(false);
            }
            

            

            //FindObjectsBytag
            /*var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.IncrementScore();*/
        }
    }
}
