using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    public class Shield : MonoBehaviour
    {
        public enum State
        {
            Falling, OnGround, PickedUp
        }

        public float DeathTime = 10.0f;
        private float timer = 0;
        public State state = State.Falling;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (timer > DeathTime)
            {
                if (GetComponent<PhotonView>().IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
                return;
            }

            if (state != State.PickedUp)
            {
                var color = GetComponent<Renderer>().material.color;
                color.a = Mathf.Lerp(1, 0, timer / DeathTime);
                GetComponent<Renderer>().material.color = color;
                timer += Time.deltaTime;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Ground"))
            {
                state = State.OnGround;
            }
        }


        [PunRPC]
        public void DestroyShield()
        {
            if (GetComponent<PhotonView>().IsMine/*|| PhotonNetwork.IsMasterClient*/)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
