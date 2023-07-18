using System.Threading.Tasks;
using UnityEngine;
using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class PlayerDataLoader : MonoBehaviourInitializable
    {
        public override void Initialize()
        {
            if(LocalYandexData.Instance.YandexDataLoaded) return;
            YandexGamesManager.LoadPlayerData(gameObject.name, nameof(OnPlayerDataReceived));
        }

        public void OnPlayerDataReceived(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Failed to load player data");
            }
            else
            {
                LocalYandexData.Instance.SetPlayerData(JsonUtility.FromJson<PlayerData>(json));
            }
        }
    }
}