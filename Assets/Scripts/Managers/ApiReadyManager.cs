using YandexSDK.Scripts;

namespace SkibidiRunner.Managers
{
    public class ApiReadyManager : MonoBehaviourInitializable
    {
        public override void Initialize()
        {
            YandexGamesManager.ApiReady();
        }
    }
}