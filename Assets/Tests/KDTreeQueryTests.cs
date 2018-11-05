﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataStructures.Tests {

    public enum QType {

        ClosestPoint,
        KNearest,
        Radius,
        Interval
    }


    public class KDTreeQueryTests : MonoBehaviour {

        public QType QueryType;

        public int K = 13;

        [Range(0f, 1f)]
        public float Radius = 0.1f;

        public Vector3 IntervalSize = new Vector3(0.2f, 0.2f, 0.2f);

        Vector3[] pointCloud;
        KDTree tree;

        DataStructures.Query.KDQuery query;

        void Awake() {

            pointCloud = new Vector3[10000];


            for(int i = 0; i < pointCloud.Length / 2; i++) {

                pointCloud[i] = new Vector3(

                    (Random.value + Random.value) / 2f,
                    (Random.value + Random.value) / 2f,
                    (Random.value + Random.value) / 2f
                );
            }

            Vector3 U = Random.onUnitSphere;
            Vector3 V = Random.onUnitSphere;

            for(int i = pointCloud.Length / 2; i < pointCloud.Length; i++) {

                pointCloud[i] = Vector3.one * 1f + Random.value * U + Random.value * V + Random.insideUnitSphere * 0.1f;
            }


            tree = KDTreeBuilder.Instance.Build(pointCloud);

            query = new Query.KDQuery();
        }


        private void OnDrawGizmos() {

            if(query == null) {
                return;
            }

            Vector3 size = 0.01f * Vector3.one;

            var resultIndices = new List<int>();

            switch(QueryType) {

                case QType.ClosestPoint: {

                    resultIndices.Add(query.ClosestPoint(tree, transform.position));
                }
                break;

                case QType.KNearest: {

                    query.KNearest(tree, transform.position, K, resultIndices);
                }
                break;

                case QType.Radius: {

                    query.Radius(tree, transform.position, Radius, resultIndices);
                }
                break;

                case QType.Interval: {

                    query.Interval(tree, transform.position - IntervalSize, transform.position + IntervalSize, resultIndices);
                }
                break;

                default:
                break;
            }

            for(int i = 0; i < pointCloud.Length; i++) {


                Gizmos.DrawCube(pointCloud[i], size);
            }

            Color found = Color.red;
            found.a = 0.5f;
            Gizmos.color = found;

            for(int i = 0; i < resultIndices.Count; i++) {

                Gizmos.DrawCube(pointCloud[resultIndices[i]], 2f * size);
            }

        }
    }
}