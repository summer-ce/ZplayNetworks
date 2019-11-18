namespace Assets.ZplaySDK.Scripts.SocialNetworking
{
    public enum SocialNetworkType
    {
        GooglePlayGames,
        GameCenter,
        Dummy,
        Local =
#if UNITY_EDITOR || UNITY_STANDALONE
        Dummy,
#elif UNITY_ANDROID
        GooglePlayGames,
#elif UNITY_IOS
        GameCenter,
#endif
    }
}