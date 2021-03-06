# Zplay Play Games plugin for Unity
Copyright (c)2019 Zplay Inc. All rights reserved.

The Zplay Play Games plugin for Unity is an open-source project whose goal is to provide a plugin that allows game developers to integrate with the Zplay Play Games API from a game written in Unity.

## Overview
The Zplay Play Game Plugin for Unity lets you edit the Zplay Play Game API through Unity's compiler window interface. The plugin is supported for iOS and Android platforms. The following feature plugins are introduced:
 
 * Zplay Base SDK        
 * Flurry statistics     
 * Applyer statistics     
 * Haptic feed back     
 * Unbiased time     
 * Android runtime permissions     
 * Unity logs viewe      
 * Google and IOS Purchasr    
 * Googl and IOS SocialNetworking   
 
 ### Features 
 * Easy project setup for compiler windows (integrated into unity)     
 * No need to overwrite / quickly import unitypackage through the compiler window panel    
 * No need to overwrite / quickly remove Unitypackage through the compiler window panel    

### System requirements
* Unity® 5 or above.   
* To deploy on Android and IOS    
* Importing AndroidRuntimPermissions must import ZplaybaseSDK    
* AndroidRuntimPermissions depends on the ExtentionMethods.cs in the ZplaybaseSDK package.    
* ExtentionMethods.cs is an Action event extension class that developers can perform on their own.   
	
## Configure Your Game
Import the latest zplay unity plugin from current-build. The zplay tab will appear above the unity editor. Click on the tab and the following will appear:  

![click Get Resources](source/docgen/Zplay.png "Show the resources data")

* Deleate All PlayerPrefs  
__Delete all player prefs files for this project__
* About Zplay SDK  
__Jump zplay SDK feedback page__  
* Documentation…   
__Jump to sdk access document directory__  
* Manager SDKs…     

![click Get Resources](source/docgen/ZplaySDKManagr.png "Show the resources data")

__Note: Each time you import the corresponding .unitypackage, please click on DelUPK. This button works for me to delete the .unitypakcage file you downloaded, which has reduced your package size.__

## UnbiaseedTime  
* Unity Anti-Cheat Time Plugin
* To add time,call：
__Example:UnbiasedTime.Instance.Now().AddSeconds(10f);__

## [Zplay Base]
__Zplay Base must be imported, this is the default base package for zplay__
* Zplay Base contains     
__Some extension libraries available for development in the zplay package can be modified or called by you. If you need, please check it yourself.__

## [AndroidRuntimPermissions](AndroidRuntimPermissions.md)

## [Flurry、Appflyer、TalkingData](Statistics.md)

## [HapticFeedBack](HapticFeedBack.md)

## [Unity-logs-Viewer](Unity-logs-Viewer.md)

## [GP_IOS_Purchasr、GP_IOS_SocialNetworking](GP_IOS_Purchasr_SocialNetWorking.md)
__This is the google play and ios payment and login list__







