using System;
using System.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ECS
{
    public class EffectController : MonoBehaviour
    {
        public static EntityManager EntityManager;

        [SerializeField] private GameObject[] effectPrefab;
        [SerializeField] private float minSpeed = 1.5f;
        [SerializeField] private float maxSpeed = 2.5f;
        [SerializeField] private uint effectsIterations = 40;
        [SerializeField] private float effectStep = 0.1f;
        [SerializeField] private float effectWidth = 0.5f;
        [SerializeField] private int objectsInLine = 3;

        private GameObject[] effectObjects;
        private Coroutine effectCoroutine;

        private void Start()
        {
            EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            effectObjects = new GameObject[effectPrefab.Length];
            for (int i = 0; i < effectObjects.Length; i++)
            {
                effectObjects[i] = Instantiate(effectPrefab[i]);
                effectObjects[i].SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (effectCoroutine != null)
                {
                    return;
                }

                effectCoroutine = StartCoroutine(EffectProcess(new Vector3(0f, 1.2f, -6f)));
            }
        }

        private IEnumerator EffectProcess(Vector3 startPoint)
        {
            var counter = 0;

            while (counter < effectsIterations)
            {
                foreach (var effectObject in effectObjects)
                {
                    effectObject.SetActive(true);
                }

                for (int i = 0; i < objectsInLine; i++)
                {
                    var x = Random.Range(-effectWidth, effectWidth) + startPoint.x;
                    var z = counter * effectStep + startPoint.z;

                    var position = new Vector3(x, 0.2f, z);
                    var direction = new float3(0f, 3f, 0f);
                    var speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
                    var obj = effectObjects[Random.Range(0, effectObjects.Length)];
                    obj.transform.position = position;
                    obj.transform.rotation = quaternion.identity;
                    
                    SetupObject(obj, direction, speed);
                }

                counter++;

                foreach (var effectObject in effectObjects)
                {
                    effectObject.SetActive(false);
                }
                yield return null;
            }

            effectCoroutine = null;
        }

        private void SetupObject(GameObject effectObj, float3 direction, float speed)
        {
            var conversionSettings =
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var effectEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(effectObj, conversionSettings);
            var movementData = new EffectMovementComponent()
            {
                direction = direction,
                speed = speed
                
            };
            EntityManager.AddComponent<EffectMovementComponent>(effectEntity);
            EntityManager.SetComponentData(effectEntity, movementData);
        }

    }
}