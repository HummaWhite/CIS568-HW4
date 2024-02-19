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
        private Dictionary<string, int> shieldList;
        // Start is called before the first frame update
        void Start()
        {
            this.scores = new Dictionary<string, int>();
            this.shieldList = new Dictionary<string, int>();
        }

        public int GetScore(string player)
        {
            return this.scores.ContainsKey(player) ? this.scores[player] : 0;
        }

        public void SetScore(string player, int score)
        {
            if (this.scores.ContainsKey(player))
                this.scores[player] = score;
            else
                this.scores.Add(player, score);
        }
        public int GetShield(string player) 
        {
            return this.shieldList.ContainsKey(player) ? this.shieldList[player] : 0;
        }

        public void SetShield(string player, int score)
        {
            if (this.shieldList.ContainsKey(player))
                this.shieldList[player] = score;
            else
                this.shieldList.Add(player, score);
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            //display number of shield
            var name = $"Player {PhotonNetwork.LocalPlayer.ActorNumber}";
            var currentShield = this.GetShield(name);

            GUILayout.Label($"Your Shield : {currentShield}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });
            foreach (var score in this.scores)
                GUILayout.Label($"{score.Key}: {score.Value}", new GUIStyle { normal = new GUIStyleState { textColor = Color.black }, fontSize = 120 });

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
