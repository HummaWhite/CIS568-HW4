using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyFirstARGame
{
    public class Scoreboard : MonoBehaviour
    {
        private Dictionary<string, int> scores;
        // Start is called before the first frame update
        void Start()
        {
            this.scores = new Dictionary<string, int>();
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

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

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
