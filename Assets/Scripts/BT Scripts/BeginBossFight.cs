using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQ
{
    public class BeginBossFight : MonoBehaviour
    {
        BossManager bossManager;
        private void Start()
        {
            bossManager = FindObjectOfType<BossManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                
            }
        }
    }
}
