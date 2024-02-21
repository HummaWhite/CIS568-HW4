using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace MyFirstARGame
{
    public class PlayerBox : MonoBehaviour
    {
        [SerializeField]
        private Material[] materials;
        public int playerID;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void Awake()
        {
            var photonView = this.transform.GetComponent<PhotonView>();
            playerID = Mathf.Max((int)photonView.InstantiationData[0], 0);
        
            if (this.materials.Length > 0)
            {
                var material = this.materials[playerID % this.materials.Length];
                this.transform.GetComponent<Renderer>().material = material;
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
