using UnityEngine;

namespace Game.Core
{
    // Container component placed on the persistent GameObject spawned at boot.
    // Holds references to MonoBehaviour services so they survive scene loads.
    public sealed class PersistentRoot : MonoBehaviour
    {
        public SceneLoader Scenes;
        public AudioManager Audio;
    }
}
