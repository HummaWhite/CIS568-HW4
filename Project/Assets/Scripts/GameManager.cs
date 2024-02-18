using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace MyFirstARGame
{
    public class GameManager : MonoBehaviour
    {
        public GameObject snowballPrefab;
        public GameObject snowflakePrefab;
        public GameObject shieldPrefab;
        float deltaTime = 0;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //temporarily used for generating snowflake
            /*deltaTime += Time.deltaTime;
            if (deltaTime > 1) {
                deltaTime = 0;
                Vector3 pos = GameObject.Find("Main Camera").transform.position;
                pos += GameObject.Find("Main Camera").transform.forward * 0.5f;
                var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
                snowflakePrefab.GetComponent<Rigidbody>().useGravity = false;
                PhotonNetwork.Instantiate(this.snowflakePrefab.name, pos, Quaternion.Euler(90, 0, 0), data: initialData);
            }*/
        }
    }
}
