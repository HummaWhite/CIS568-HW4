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
        private Dictionary<string, bool> hasSnowBall;
        private Dictionary<string, bool> hasShield;
        // Start is called before the first frame update
        bool gameOver;
        int winnerId;
        public void Start()
        {
            this.scores = new Dictionary<string, int>();
            this.liveList = new Dictionary<string, int>();
            this.hasSnowBall = new Dictionary<string, bool>();
            this.hasShield = new Dictionary<string, bool>();
        }
        public void setGameOver()
        {
            gameOver = true;
        }
        public bool isGameOver()
        {
            if (scores.Count <= 2 || scores == null)
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
            return alive <= 2;
        }
        public void SetWinner(int winner)
        {
            winnerId = winner;
        }
        public int GetWinner()
        {
            int score = 0;
            int winnerId = -1;
            foreach (var player in this.scores)
            {
                int len1 = player.Key.Length;
                int id = player.Key[len1 - 1] - '0';
                if (id == 1) continue;
                if (player.Value > score)
                {
                    score = player.Value;
                    int len = player.Key.Length;
                    winnerId = player.Key[len - 1] - '0';
                }
                else if (player.Value == score) {
                    winnerId = -2;
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

        public bool GetHasSnowBall(string player) {
            if (this.hasSnowBall.ContainsKey(player))
            {
                return this.hasSnowBall[player];
            }
            else {
                return false;
            }
        }

        public void SetHasSnowBall(string player, bool hasBall)
        {
            if (this.hasSnowBall.ContainsKey(player))
                this.hasSnowBall[player] = hasBall;
            else
                this.hasSnowBall.Add(player, hasBall);
        }


        public bool GetHasShield(string player)
        {
            if (this.hasShield.ContainsKey(player))
            {
                return this.hasShield[player];
            }
            else
            {
                return false;
            }
        }

        public void SetHasShield(string player, bool hasBall)
        {
            if (this.hasShield.ContainsKey(player))
                this.hasShield[player] = hasBall;
            else
                this.hasShield.Add(player, hasBall);
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
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1) return;
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            var name = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentLife = this.GetLife(name);
            var hasBall = this.GetHasSnowBall(name);
            var hasShield = this.GetHasShield(name);

            GUILayout.Label($"Your Score : {this.GetScore(name)}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            GUILayout.Label($"Your Lives : {currentLife}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            if (hasBall)
            {
                GUILayout.Label($"Ready To Shoot!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            }
            else { 
                GUILayout.Label($"Pick A Ball!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            }

            if (hasShield)
            {
                GUILayout.Label($"Ready To Defend!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            }
            else
            {
                GUILayout.Label($"Pick A Shield!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            }

            foreach (var score in this.scores)
                GUILayout.Label($"{score.Key}: {score.Value}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
            if (gameOver)
            {
                if(winnerId == - 2) GUILayout.Label($"Player Draw", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
                else GUILayout.Label($"Player {winnerId} wins!", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 80 });
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
