﻿using System;
using System.Collections;
using UnityEngine;

namespace GBJAM10
{
    public class DelayedActivation : MonoBehaviour
    {
        public float delay;

        public GameObject target;

        private void Start()
        {
            StartCoroutine(DelayedActivate(delay, target));
        }

        private static IEnumerator DelayedActivate(float delay, GameObject target)
        {
            yield return new WaitForSeconds(delay);
            target.SetActive(true);
        }
    }
}