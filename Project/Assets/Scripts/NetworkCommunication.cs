namespace MyFirstARGame
{
    using Photon.Pun;
    using UnityEngine;
    
    /// <summary>
    /// You can use this class to make RPC calls between the clients. It is already spawned on each client with networking capabilities.
    /// </summary>
    public class NetworkCommunication : MonoBehaviourPun
    {
        [SerializeField]
        private Scoreboard scoreboard;
        [SerializeField]
        private GameManager gameManager;
        
        // Start is called before the first frame update
        void Start()
        {
            gameManager.Start();
        }

        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
        }

        public void IncrementShield()
        {
            var player = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentShield = this.scoreboard.GetShield(player);
            this.photonView.RPC("Network_SetPlayerShield", RpcTarget.All, player, currentShield + 1);
        }

        public void IncrementScore()
        {
            var player = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(player);
            this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, player, currentScore + 1);
        }

        [PunRPC]
        public void Network_SetPlayerScore(string player, int newScore)
        {
            Debug.Log($"Player {player} scored!");
            this.scoreboard.SetScore(player, newScore);
        }

        [PunRPC]
        public void Network_SetPlayerShield(string player, int newScore)
        {
            Debug.Log($"Player {player} shield!");
            this.scoreboard.SetShield(player, newScore);
        }

        public void UpdateForNewPlayer(Photon.Realtime.Player player)
        {
            var name = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(name);
            this.photonView.RPC("Network_SetPlayerScore", player, name, currentScore + 1);
        }
    }

}