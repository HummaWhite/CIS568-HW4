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

            // Cast a ray from the touch point to the world. We use the camera position as the origin and the ray direction as the
            // velocity direction.
            var ray = this.GetComponent<Camera>().ScreenPointToRay(position);

            var player = FindObjectOfType<PlayerBox>();
            player.ShootSnowball(ray.direction, initialSpeed);
            /*var networkCommunication = FindObjectOfType<NetworkCommunication>();
            networkCommunication.IncrementScore();*/
        }
    }
}
