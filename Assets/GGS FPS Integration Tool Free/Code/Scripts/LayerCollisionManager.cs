using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    public class LayerCollisionManager : MonoBehaviour
    {
        public int Default { get; set; }
        public int FirstPerson { get; set; }
        public int ThirdPerson { get; set; }
        public int Projectile { get; set; }
        public int NonPlayer { get; set; }
        public int EffectToPlayer { get; set; }
        public int EffectFromPlayer { get; set; }
        public int ThirdPersonBarrier { get; set; }

        void Start()
        {
            Default = LayerMask.NameToLayer("Default");
            FirstPerson = LayerMask.NameToLayer("FirstPerson");
            ThirdPerson = LayerMask.NameToLayer("ThirdPerson");
            Projectile = LayerMask.NameToLayer("Projectile");
            NonPlayer = LayerMask.NameToLayer("NonPlayer");
            EffectToPlayer = LayerMask.NameToLayer("EffectToPlayer");
            EffectFromPlayer = LayerMask.NameToLayer("EffectFromPlayer");
            ThirdPersonBarrier = LayerMask.NameToLayer("ThirdPersonBarrier");

            Physics.IgnoreLayerCollision(Default, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(Default, EffectFromPlayer);
            Physics.IgnoreLayerCollision(Default, EffectToPlayer);
            Physics.IgnoreLayerCollision(Default, FirstPerson);

            Physics.IgnoreLayerCollision(FirstPerson, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(FirstPerson, EffectFromPlayer);
            Physics.IgnoreLayerCollision(FirstPerson, EffectToPlayer);
            Physics.IgnoreLayerCollision(FirstPerson, NonPlayer);
            Physics.IgnoreLayerCollision(FirstPerson, Projectile);
            Physics.IgnoreLayerCollision(FirstPerson, ThirdPerson);
            Physics.IgnoreLayerCollision(FirstPerson, FirstPerson);

            Physics.IgnoreLayerCollision(ThirdPerson, EffectFromPlayer);
            Physics.IgnoreLayerCollision(ThirdPerson, Projectile);

            Physics.IgnoreLayerCollision(Projectile, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(Projectile, EffectToPlayer);
            Physics.IgnoreLayerCollision(Projectile, NonPlayer);
            Physics.IgnoreLayerCollision(Projectile, Projectile);

            Physics.IgnoreLayerCollision(NonPlayer, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(NonPlayer, EffectFromPlayer);
            Physics.IgnoreLayerCollision(NonPlayer, EffectToPlayer);

            Physics.IgnoreLayerCollision(EffectToPlayer, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(EffectToPlayer, EffectFromPlayer);
            Physics.IgnoreLayerCollision(EffectToPlayer, EffectToPlayer);

            Physics.IgnoreLayerCollision(EffectFromPlayer, ThirdPersonBarrier);
            Physics.IgnoreLayerCollision(EffectFromPlayer, EffectFromPlayer);

            Physics.IgnoreLayerCollision(ThirdPersonBarrier, ThirdPersonBarrier);
        }
    }
}

