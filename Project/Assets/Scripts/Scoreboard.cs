using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace MyFirstARGame
{
    public class Scoreboard : MonoBehaviour
    {
        private Dictionary<string, int> scores;
        public int GetNumberOfPlayers()
        {
            return scores.Count;
        }
        private Dictionary<string, int> liveList;
        // Start is called before the first frame update
        bool gameOver;
        int winnerId;
        public void Start()
        {
            this.scores = new Dictionary<string, int>();
            this.liveList = new Dictionary<string, int>();
        }
        public void setGameOver()
        {
            gameOver = true;
        }
        public bool isGameOver()
        {
            if (scores.Count <= 1 || scores == null)
            {
                return false;
            }
            int alive = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (this.GetLife($"Player {player.ActorNumber}") > 0)
                {
                    alive++;
                }
            }
            return alive <= 1;
        }
        public void SetWinner(int winner)
        {
            winnerId = winner;
        }
        public int GetWinner()
        {
            int score = 0;
            int winnerId = -1;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (this.GetScore($"Player {player.ActorNumber}") > score)
                {
                    score = this.GetScore($"Player {player.ActorNumber}");
                    winnerId = player.ActorNumber;
                }
            }
            return winnerId;
        }
        public int GetScore(string player)
        {
            if (scores == null)
            {
                Debug.Log("Score is null");
                return 0;
            }
            if (this.scores.ContainsKey(player))
            {
                return this.scores[player];
            }
            else
            {
                return -1;
            }
        }

        public void SetScore(string player, int score)
        {
            if (this.scores.ContainsKey(player))
                this.scores[player] = score;
            else
                this.scores.Add(player, score);
        }

        public int GetLife(string player)
        {
            return this.liveList.ContainsKey(player) ? this.liveList[player] : 0;
        }

        public void SetLife(string player, int score)
        {
            if (this.liveList.ContainsKey(player))
                this.liveList[player] = score;
            else
                this.liveList.Add(player, score);
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            var name = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentLife = this.GetLife(name);

            GUILayout.Label($"Your Score : {this.GetScore(name)}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });
            GUILayout.Label($"Your Lives : {currentLife}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });
            foreach (var score in this.scores)
                GUILayout.Label($"{score.Key}: {score.Value}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });
            if (gameOver)
            {
                GUILayout.Label($"Player {winnerId} wins!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
