using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace YandexSDK.Scripts
{
    public static class YandexGamesManager
    {
        [DllImport("__Internal")]
        private static extern string getPlayerName();

        [DllImport("__Internal")]
        private static extern string getPlayerPhotoURL();

        [DllImport("__Internal")]
        private static extern void requestReviewGame();
        
        [DllImport("__Internal")]
        private static extern int getReviewStatus();
        
        [DllImport("__Internal")]
        private static extern void savePlayerData(string data);
        
        [DllImport("__Internal")]
        private static extern string loadPlayerData();
        
        [DllImport("__Internal")]
        private static extern void setToLeaderboard(int value);
        
        [DllImport("__Internal")]
        private static extern string getLang();

        /// <summary>
        /// User name on the Yandex Games platform
        /// </summary>
        /// <returns>Player username, null on errors</returns>
        public static string GetPlayerName()
        {
            return getPlayerName();
        }

        /// <summary>
        /// User avatar on the Yandex platform 
        /// </summary>
        /// <returns>Avatar texture, null on errors</returns>
        public static async Task<Texture2D> GetPlayerPhoto()
        {
            var request = UnityWebRequestTexture.GetTexture(getPlayerPhotoURL());
            request.SendWebRequest();
            while (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.result is not (UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError))
                return ((DownloadHandlerTexture)request.downloadHandler).texture;
            Debug.Log(request.error);
            return null;
        }

        /// <summary>
        /// Shows the game rating window when there are no errors
        /// </summary>
        public static void RequestReviewGame()
        {
            requestReviewGame();
        }

        /// <summary>
        /// Current status of the player's rating of the game
        /// </summary>
        /// <returns>
        /// Unknown - request has not been sent; an error on Yandex side
        /// CanReview - request is possible
        /// NoAuth - user is unauthorized
        /// GameRated - user has rated the game
        /// ReviewAlreadyRequested - request was already sent and user action is pending
        /// ReviewWasRequested - request already sent, user did something: put a rating or close the pop-up
        /// </returns>
        public static ReviewStatus GetReviewStatus()
        {
            return (ReviewStatus)getReviewStatus();
        }

        public static void SavePlayerData(PlayerData playerData)
        {
            try
            {
                string json = JsonUtility.ToJson(playerData);
                savePlayerData(json);
            }
            catch
            {
                // ignored
            }
        }

        public static PlayerData LoadPlayerData()
        {
            try
            {
                return JsonUtility.FromJson<PlayerData>(loadPlayerData());
            }
            catch (Exception)
            {
                return new PlayerData();
            }
            
        }

        public static void SetToLeaderboard(int value)
        {
            try
            {
                setToLeaderboard(value);
            }
            catch
            {
                // ignored
            }
        }

        public static Language GetLanguage()
        {
            try
            {
                string lang = getLang();
                return lang switch
                {
                    "ru" => Language.Russian,
                    "tr" => Language.Turkey,
                    _ => Language.English
                };
            }
            catch (Exception)
            {
                return Language.English;
            }
            
        }
    }
}