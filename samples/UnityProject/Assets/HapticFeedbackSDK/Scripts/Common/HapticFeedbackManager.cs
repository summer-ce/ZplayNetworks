using Assets.ZplaySDK.Scripts.Extentions;
#if UNITY_IPHONE
using UnityEngine.iOS;
#endif
public class HapticFeedbackManager : MonoSingleton<HapticFeedbackManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
    public void HapticFeedbackTrigger(FeedbackType feedbackType)
    {
        switch (feedbackType)
        {
            case FeedbackType.SelectionChange:
                SelectionChangeHapticFeedback(feedbackType);
                break;
            case FeedbackType.ImpactLight:
                ImpactLightHapticFeedback(feedbackType);
                break;
            case FeedbackType.ImpactMedium:
                ImpactMediumHapticFeedback(feedbackType);
                break;
            case FeedbackType.ImpactHeavy:
                ImpactHeavyHapticFeedback(feedbackType);
                break;
            case FeedbackType.Success:
                SuccessHapticFeedback(feedbackType);
                break;
            case FeedbackType.Warning:
                WarningHapticFeedback(feedbackType);
                break;
            case FeedbackType.Failure:
                FailureHapticFeedback(feedbackType);
                break;
            default:
                break;
        }
    }

    private void SelectionChangeHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateOneShot(16);
#endif
    }
    private void ImpactLightHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateOneShot(20);
#endif
    }
    private void ImpactMediumHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateOneShot(26);
#endif
    }
    private void ImpactHeavyHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateOneShot(32);
#endif
    }
    private void SuccessHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateWaveform(new long[4] { 0, 18, 150, 30 }, -1);
#endif
    }
    private void WarningHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateWaveform(new long[4] { 0, 26, 150, 18 }, -1);
#endif
    }
    private void FailureHapticFeedback(FeedbackType feedbackType)
    {
#if UNITY_IPHONE
        iOSHapticFeedback.Instance.Trigger(feedbackType);
#elif UNITY_ANDROID
        AndroidHapticFeedback.Instance.CreateWaveform(new long[8] { 0, 18, 70, 18, 70, 30, 70, 7 }, -1);
#endif
    }


#if (UNITY_IPHONE || UNITY_IOS)
    DeviceGeneration[] m_tapticPhones = {
                DeviceGeneration.iPhone7,
                DeviceGeneration.iPhone7Plus,
                DeviceGeneration.iPhone8,
                DeviceGeneration.iPhone8Plus,
                DeviceGeneration.iPhoneX,
                DeviceGeneration.iPhoneUnknown, //Unity 2018.1 doesnt support iPhone Maxs. 2018.2.12 and 2018.3 should once it is released.
			};
#endif
    public bool DeviceSupportsHaptic()
    {
#if UNITY_EDITOR
        return true;
#elif UNITY_IPHONE
				DeviceGeneration generation = UnityEngine.iOS.Device.generation;
				foreach(DeviceGeneration gen in m_tapticPhones)
				{
					if(gen == generation)
					{
						return true;
					}
				}
				return false;
#elif UNITY_ANDROID
            return  AndroidHapticFeedback.Instance.HasVibrator();;
#endif
    }
}
