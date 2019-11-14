# HapticFeedBack plugin for Unity
* This sdk contains Android and IOS vibration feedback haptic functions.  
* IOS support system IOS10.0 or above, iPhone7 and above  
* Android support device Android 8.0 or higher 
* In “Used Feedback Types” you can define which vibration types you are going to use. There are seven different feedback types

## The following vibration types
### Selection Change 
### Impact  
* Light 
* Medium 
* Heavy 
### Notifications
* Success
* Warning 
* Failure
## To related function, call:  
__HapticFeedbackManager.Instance. HapticFeedbackTrigger(FeedbackType feedbackType);__
*Check if the device supports vibration,call：
__Bool =HapticFeedbackManager.Instance.DeviceSupportsHaptic();__







