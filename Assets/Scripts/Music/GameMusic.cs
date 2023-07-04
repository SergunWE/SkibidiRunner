using UnityEngine;

namespace SkibidiRunner.Music
{
    [CreateAssetMenu(menuName = "Music/Game Music")]
    public class GameMusic : ScriptableObject
    {
        [field:SerializeField] public AudioClip Song { get; private set; }
        [field:SerializeField] public float SongBpm { get; private set; }
        [field:SerializeField] public float FirstBeatOffset { get; private set; }
    }
}