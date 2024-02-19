using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Composites;

namespace MyFirstARGame
{
    public class GameManager : MonoBehaviour
    {
        public static bool roomCreated = false;

        public GameObject snowballPrefab;
        public GameObject snowflakePrefab;
        public GameObject shieldPrefab;
        public GameObject groundPrefab;
        public float snowflakeSpawnTime = 0.5f;
        public float shieldSpawnTime = 5.0f;
        public float snowballSpawnTime = 1.0f;
        public Vector3 snowflakeSpawnRange = new Vector3(5, 5, 5);
        float snowflakeTimer = 0;
        float shieldTimer = 0;
        float snowballTimer = 0;
        bool publicObjectGenerated = false;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (!roomCreated)
                return;

            GeneratePublicObject();
            SpawnSnowflake();
            SpawnShield();
            SpawnSnowball();

            snowflakeTimer += Time.deltaTime;
            shieldTimer += Time.deltaTime;
            snowballTimer += Time.deltaTime;
        }

        void GeneratePublicObject()
        {
            if (publicObjectGenerated)
                return;

            PhotonNetwork.Instantiate(groundPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, data: null);
            publicObjectGenerated = true;
        }

        void SpawnSnowflake()
        {
            if (snowflakeTimer < snowflakeSpawnTime)
                return;

            var pos = new Vector3(Random.Range(-snowflakeSpawnRange.x, snowflakeSpawnRange.x), snowflakeSpawnRange.y, Random.Range(-snowflakeSpawnRange.z, snowflakeSpawnRange.z));
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            var snowflake = PhotonNetwork.Instantiate(snowflakePrefab.name, pos, Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)), data: initialData);

            var rigid = snowflake.GetComponent<Rigidbody>();
            rigid.AddForce(new Vector3(0, -4.0f, 0), ForceMode.VelocityChange);

            snowflakeTimer = 0;
        }

        void SpawnShield()
        {
            if (shieldTimer < shieldSpawnTime)
                return;

            var pos = new Vector3(Random.Range(-snowflakeSpawnRange.x, snowflakeSpawnRange.x), snowflakeSpawnRange.y, Random.Range(-snowflakeSpawnRange.z, snowflakeSpawnRange.z));
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            var shield = PhotonNetwork.Instantiate(shieldPrefab.name, pos, Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)), data: initialData);

            shieldTimer = 0;
        }

        void SpawnSnowball()
        {
            if (snowballTimer < snowballSpawnTime)
                return;

            var pos = new Vector3(Random.Range(-snowflakeSpawnRange.x, snowflakeSpawnRange.x), 0.5f, Random.Range(-snowflakeSpawnRange.z, snowflakeSpawnRange.z));
            var initialData = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };

            var snowball = PhotonNetwork.Instantiate(snowballPrefab.name, pos, Quaternion.identity, data: initialData);

            snowballTimer = 0;
        }
    }
}
