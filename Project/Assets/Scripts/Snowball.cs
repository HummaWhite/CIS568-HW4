using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    public class Snowball : MonoBehaviour
    {
        public float DeathTime = 10.0f;
        private float timer = 0;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (timer > DeathTime)
            {
                PhotonNetwork.Destroy(gameObject);
                return;
            }

            var color = GetComponent<Renderer>().material.color;
            color.a = Mathf.SmoothStep(1, 0, timer / DeathTime);
            GetComponent<Renderer>().material.color = color;

            timer += Time.deltaTime;
        }
    }
}
