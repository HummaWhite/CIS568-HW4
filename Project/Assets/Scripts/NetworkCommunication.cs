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
        

        int numOfLives = 5;

        // Start is called before the first frame update
        void Start()
        {
            scoreboard.Start();
            gameManager.Start();
        }

        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (this.scoreboard.GetScore($"Player {player.ActorNumber}") < 0)
                {
                    this.photonView.RPC("Network_SetPlayerLife", RpcTarget.All, $"Player {player.ActorNumber}", this.numOfLives);
                    this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, $"Player {player.ActorNumber}", 0);
                }
            }
            if(scoreboard.isGameOver())
            {
                int winner = scoreboard.GetWinner();
                this.photonView.RPC("Network_SetGameOver", RpcTarget.All, true, winner);
            }
        }


        public void IncrementScore()
        {
            var player = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(player);
            this.photonView.RPC("Network_SetPlayerScore", RpcTarget.All, player, currentScore + 1);
        }

        public void DecrementSnowball()
        {
            gameManager.snowballCount--;
        }

        [PunRPC]
        public void Network_SetPlayerScore(string player, int newScore)
        {
            Debug.Log($"Player {player} scored!");
            this.scoreboard.SetScore(player, newScore);
        }


        [PunRPC]
        public void Network_SetPlayerLife(string player, int newScore)
        {
            this.scoreboard.SetLife(player, newScore);
        }

        [PunRPC]
        public void Network_SetGameOver(bool isGameOver, int winner)
        {
            Debug.Log($"Game Over!");
            this.scoreboard.setGameOver();
            this.scoreboard.SetWinner(winner);
        }
        
        public void UpdateForNewPlayer(Photon.Realtime.Player player)
        {
            var name = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentScore = this.scoreboard.GetScore(name);
            this.photonView.RPC("Network_SetPlayerScore", player, name, currentScore + 1);
            this.photonView.RPC("Network_SetPlayerLife", player, name, numOfLives);
        }
    }

}