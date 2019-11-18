using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.ZplaySDK.Scripts.Extentions;

namespace Assets.ZplaySDK.Scripts.SocialNetworking
{
    public class SocialNetworksManager : MonoSingleton<SocialNetworksManager>
    {
        private readonly Dictionary<SocialNetworkType, List<Action<Boolean>>> _authenticationCallbacks = new Dictionary<SocialNetworkType, List<Action<Boolean>>>();

        public static event Action<SocialNetworkType> Authenticated;
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public void Initialize()
        {
            foreach (var socialNetworkType in Enum.GetValues(typeof(SocialNetworkType)).Cast<SocialNetworkType>().Distinct())
            {
                _authenticationCallbacks.Add(socialNetworkType, new List<Action<Boolean>>());
            }
            Authenticate(SocialNetworkType.Local, result =>
            {
                if (result)
                {
                    //登录成功可以进行操作
                }
                else
                {
                    //登录失败进行操作
                }
            });
        }

        public void Authenticate(SocialNetworkType socialNetworkType, Action<Boolean> callback = null)
        {
            if(socialNetworkType == SocialNetworkType.Dummy)
            {
                callback.SafeInvoke(true);
            }
            else if(socialNetworkType == SocialNetworkType.GooglePlayGames||socialNetworkType == SocialNetworkType.GameCenter)
            {
#if UNITY_ANDROID
                PlayGamesPlatform.InitializeInstance(PlayGamesClientConfiguration.DefaultConfiguration);
                // recommended for debugging:
                PlayGamesPlatform.DebugLogEnabled = true;
                // Activate the Google Play Games platform
                PlayGamesPlatform.Activate();
#else
#endif
                if(Social.localUser == null)
                {
                    Debug.Log("Local user is null");
                    callback.SafeInvoke(false);
                }
                else
                {
                    if (Social.localUser.authenticated)
                    {
                        callback.SafeInvoke(true);
                    }
                    else
                    {
                        Debug.Log("Trying to authenticate");
                        _authenticationCallbacks[socialNetworkType].Add(callback);
                        if (_authenticationCallbacks[socialNetworkType].Count == 1) //Exactly one to start only one authentication process at a time
                        {
                            Social.localUser.Authenticate(flag =>
                            {
                                if (flag)
                                {
                                    //登录成功处理相关操作
                                }

                                foreach (var authenticationCallback in _authenticationCallbacks[socialNetworkType])
                                {
                                    authenticationCallback.SafeInvoke(flag);
                                }
                                _authenticationCallbacks[socialNetworkType].Clear();
                                Authenticated.SafeInvoke(socialNetworkType);
                            });
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Unsupported SocialNetwork: " + socialNetworkType);
            }
        }
        public void ShowAchievementsUI(Action<Boolean> callback = null)
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
                callback.SafeInvoke(true);
            }
            else
            {
                callback.SafeInvoke(false);
            }
        }
        public void UnlockAchievements(String achievementId,Action<Boolean> callback = null)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(achievementId, 100.0f, callback);
            }
            else
            {
                callback.SafeInvoke(false);
            }
        }
        public void UnlockAchievements(String achievementId,int steps ,Action<Boolean> callback = null)
        {
#if UNITY_ANDROID
            if (Social.localUser.authenticated)
            {
                PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, callback);
            }
            else
            {
                callback.SafeInvoke(false);
            }
#endif
        }
        public void ShowLeaderboardUI(String leaderboardId = null ,Action<Boolean> callback = null)
        {
            if (Social.localUser.authenticated)
            {
                if (String.IsNullOrEmpty(leaderboardId))
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
#endif
                }
                callback.SafeInvoke(true);
            }
            else
            {
                callback.SafeInvoke(true);
            }
        }
        public void ReportLeaderboard(string leaderboard,long score,Action<Boolean> callback = null,String metadata = null)
        {
            if (Social.localUser.authenticated)
            {
                if (String.IsNullOrEmpty(metadata))
                {
                    Social.ReportScore(score, leaderboard, callback);
                }
                else
                {
#if UNITY_ANDROID
                    PlayGamesPlatform.Instance.ReportScore(score, leaderboard, metadata, callback);
#endif
                }
            }
            else
            {
                callback.SafeInvoke(false);
            }
        }
    }
}
