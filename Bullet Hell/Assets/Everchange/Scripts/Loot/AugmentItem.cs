using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loot
{
    public class AugmentItem : GameItem
    {
        protected override void Awake()
        {
            base.Awake();
        }

        //Just be a variable to read
        public Augment augmentType;
        public enum Augment
        {
            shield,
            rateOfFire,
            piercing,
        }
    }
}