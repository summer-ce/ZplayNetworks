# Unity
## Scope of application
TalkingData App Analytics Unity SDK is applicable for apps/games developed for the Unity platform.

## Preparation for integration - create an app and get an app ID
App ID is a unique identifier used by the TalkingData Analytics platform to identify the independent app/game. Before you integrate the SDK, you need to create an app/game in the TalkingData report and get the corresponding app ID.
**Steps:**
1) Sign up at and sign in to https://www.talkingdata.com/, and select App Analytics;
2) Create an app and get an app ID;
3) For an already created app, view the app ID in "App management > Basic info".
**Note:**
TalkingData supports use of the same app ID across multiple platforms.

## Preparation for integration - descriptions of statistical standards
Basic statistical indicators - definitions:
1) New user: For the Android and iOS platforms, a "user" refers to a unique device.
2) Session: It refers to the entire process from when the app is opened to when it is closed. If the user presses the Home button to leave the app and then returns within 30s, it will be considered as a continuation of the previous session and not be counted as a new session.
3) Custom event: It refers to specific actions performed by the user or specific conditions met by the user in HTML5. For example, tapping the ad banner or making a payment, etc. The custom event is used to collect any data you expect to track.

## Quick integration - generate SDK
Generate the Unity SDK according to <a href="https://github.com/TalkingData/AppAnalytics_SDK_Unity" target="_blank"> TalkingData App Analytics Unity SDK Generation Guide</a>.

## Quick integration - import SDK
Copy the Assets folder in the generated Unity SDK and merge it with the Assets folder in the Unity project .

## Quick integration - configure permissions and add dependency frameworks
### Android
For the Android platform, the SDK requires appropriate permissions to work properly. The developer needs to add all the following permission declarations in the AndroidManifest.xml file.

|Permissions|Purpose|
|---|---|
|INTERNET|Allows the app to connect to the network and send statistics.|
|ACCESS\_NETWORK\_STATE|Allows the app to detect the network connection status and avoid data transmission in case of abnormal network status so as to save data traffic and power.|
|READ\_PHONE\_STATE|Allows the app to access mobile device information in a read-only manner and locate the unique gamer by the obtained information.|
|ACCESS\_WIFI\_STATE|Used to obtain the device's MAC address.|
|WRITE\_EXTERNAL\_STORAGE|Used to save and log device information.|
|GET\_TASKS (optional)|Checks whether the current app is the displayed app which can help gather more accurate statistics.|
|ACCESS\_FINE\_LOCATION (optional)|Used to identify the precise location where the app is being used.|
|ACCESS\_COARSE\_LOCATION (optional)|Used to identify the rough location where the app is being used.|

### iOS
For the iOS platform, the SDK requires appropriate dependency frameworks to work properly. The developer needs to add all the following dependency frameworks in the compiled Xcode project.

|Framework|Purpose|
|---|---|
|AdSupport.framework|Obtain advertisingIdentifier|
|CoreTelephony.framework|Obtain carrier ID|
|CoreMotion.framework|Support shake function|
|Security.framework|Store device identification|
|SystemConfiguration.framework|Detect network status|
|libc++.tbd|Support the latest C++ 11 standard|
|libz.tbd|Perform data compression|

## Quick integration - initialize SDK
After the app is opened (including app launch and app switch from the background to the foreground), call Talkingdataplugin.Sessionstarted. This interface completes the initialization of the statistics module and the creation of the statistics session, so the earlier it is called, the better.
When the app is closed (including app exit and app switch from the foreground to the background, e.g. pressing the home button or locking the screen), call TalkingDataPlugin.SessionStoped.
You can optionally fill in the name of the marketing channel for ChannelID, so you can view the data for corresponding channel in the data report. For each device, only the channel from which the device is activated for the first time will be recorded. Changing the channel package will not be counted as a new device. Please recompile and package for different channel IDs.

## Basic - basic statistics
After correctly completing the initialization call, the statistics of the app launch are automatically collected. For more information, see integration step - Initialize SDK.
Based on the app launch, the backend automatically computes the independent device and various indicators such as new, active, retained and upgraded.

## Basic - channel statistics
#### 1. Purpose and use
A channel marker is integrated into the app's installation package, so when the user installs and uses the app, corresponding data can be queried separately in the report by different source channels.
When you provide an app installation package for different distribution channels such as app stores or download stations, you can add a channel marker; you can also add special channel marker for special events to facilitate individual analysis.

**Precautions:**  
User channel attribution: For each device, only the channel from which the device is activated for the first time will be recorded. After the channel package is changed, when the same user uses the app, the device will not be counted as a new device, and the usage data will be contributed to the initial activation channel. If no channel marker is added or the channel marker used is the default value in the sample code, the user will be classified as from "unknown channel".

#### 2. Integrated mode description
Write the channel ID to the initialization.

## Basic - error report
#### 1. Purpose and use
The app's error log can help you fix bugs and improve the app. In the report, in addition to the number of errors, we also provide the error details: the time when the error occurs, the stack call and a reasonable classification of the error.

**Precautions:**  
1) The collection of error information consumes end user's data traffic, so automatic capture is turned off by default. You can turn it on as needed.
2) Method calls must be performed as soon as each platform is initialized.

#### 2. Interface and parameters
Automatically obtain exception information

```
TalkingDataPlugin.SetExceptionReportEnabled(true);
```

## Basic - location information optimization
#### 1. Purpose and use
By default, TalkingData uses the collected device's MCC (mobile country code) and user's network IP to determine the user's location. The geographic data may contain some errors.
If your app uses the user's location information, the information can be merged into the TalkingData data via the interface, allowing you to get a more accurate data report.

#### 2. Interface and parameters
Call the following method to submit the exact information you obtain:

```
TalkingDataPlugin.SetLocation(Latitude, longitude);
```

## Basic - account
### Registration
#### 1. Purpose and use
The registration interface is used to record the user's registration behavior when using the app. It is recommended to call this interface when the registration is successful.

#### 2. Interface and parameters
**Interface:**

```
public static void OnRegister(string accountId, TalkingDataAccountType type, string name)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|accountID|string|The account ID which is globally unique. If a null or empty string "" is passed, this event won't be generated.|
|type|TalkingDataAccountType|The type of passed account.<br>It supports anonymous account, explicitly registered account, third-party account and other reserved custom account types.<br>Public enum TalkingDataAccountType {<br><t>ANONYMOUS = 0, // anonymous<br>REGISTERED = 1, // explicitly registered account<br>SINA\_WEIBO = 2, // Sina Weibo account<br>QQ = 3, // QQ account<br>QQ\_WEIBO = 4, // Tencent Weibo account<br>ND91 = 5, // NetDragon 91 account<br>WEIXIN = 6, // WeChat account<br>// User-defined account type<br>TYPE1 = 11,<br>TYPE2 = 12,<br>TYPE3 = 13,<br>TYPE4 = 14,<br>TYPE5 = 15,<br>TYPE6 = 16,<br>TYPE7 = 17,<br>TYPE8 = 18,<br>TYPE9 = 19,<br>TYPE10 = 20<br>};|
|Name|string|Account nickname|

#### 3. Example
The user registers with a WeChat account. The account ID upon successful registration is 1234567 and the nickname: Sun. Call as follows:

```
TalkingDataPlugin.OnRegister ("1234567", TalkingDataAccountType.WEIXIN, "Sun");
```

### Login
#### 1. Purpose and use
The login interface is used to record the user's login behavior when using the app. The login interface is generally called in the following circumstances:
1) The user logs in automatically after successful registration. In this case, the registration and login interfaces are called in order;
2) The user manually logs in after the login information expires. In this case, the login interface is called;
3) The login information is not expired, and the login status is automatically determined every time the app is launched. In this case, the login interface can be called by the developer as needed.

#### 2. Interface and parameters
**Interface:**

```
public static void OnLogin(string accountId, TalkingDataAccountType type, string name)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|accountID|string|The account ID which is globally unique. If a null or empty string "" is passed, this event won't be generated.|
|type|TalkingDataAccountType|The type of passed account.<br>It supports anonymous account, explicitly registered account, third-party account and other reserved custom account types.<br>Public enum TalkingDataAccountType {<br><t>ANONYMOUS = 0, // anonymous<br>REGISTERED = 1, // explicitly registered account<br>SINA\_WEIBO = 2, // Sina Weibo account<br>QQ = 3, // QQ account<br>QQ\_WEIBO = 4, // Tencent Weibo account<br>ND91 = 5, // NetDragon 91 account<br>WEIXIN = 6, // WeChat account<br>// User-defined account type<br>TYPE1 = 11,<br>TYPE2 = 12,<br>TYPE3 = 13,<br>TYPE4 = 14,<br>TYPE5 = 15,<br>TYPE6 = 16,<br>TYPE7 = 17,<br>TYPE8 = 18,<br>TYPE9 = 19,<br>TYPE10 = 20<br>};|
|Name|string|Account nickname|

#### 3. Example
A user logs in with a WeChat account. The account ID is 1234567 and the nickname: Sun. Call as follows:

```
TalkingDataPlugin.OnLogin("1234567", TalkingDataAccountType.WEIXIN, "Sun");
```

## Advanced - page visit
#### 1. Purpose and use
This feature is used to help the developer gather statistics of the number of visits and stay time for each page in the app, and to track other pages accessed by the user after leaving the page, providing the basis for product optimization.

**Precautions:**  
1) The page entry interface and page exit interface must be called at the same time;
2) Keep track of all pages as much as possible to avoid abnormal analysis results caused by only tracking some pages when analyzing the user's jump path;

#### 2. Interface and parameters
**Interface:**  
Step 1: Call the TrackPageBegin method when entering the page:

```
public static void TrackPageBegin(string pageName)
```

Step 2: Call TrackPageEnd when leaving the page:

```
public static void TrackPageEnd(string pageName)
```

**Precautions:**  
`TrackPageBegin` and `TrackPageEnd` must be called in pair.

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|pageName|String|Page name. It may contain up to 64 Chinese characters, English letters, numbers and underscores. Spaces and other escape characters are not supported;|

#### 3. Example
The user uses the TalkingData app, stays on the homepage for 30 seconds, leaves the homepage and then accesses the settings page. Call as follows:

```
TalkingDataPlugin.TrackPageBegin("App's homepage");
TalkingDataPlugin.TrackPageEnd("App's homepage");
TalkingDataPlugin.TrackPageBegin("Settings page");
```
Note: The SDK automatically counts the stay time and the source page.

## Advanced - custom events (including Smart Analysis)
### 1. Purpose and use
Custom events are used to track any user behaviors that need to be understood, such as the user's clicking on a function button, filling in an input box, triggering an ad, etc.

#### 1) How to keep track of custom events
We offer two methods: "smart event" and "code event". The difference between the two and their advantages and disadvantages are described later. Please consider both code events and smart events when designing custom event tracking schemes.

#### 2) Custom event name (EventID)
In the TalkingData analysis platform, you do not need to define a custom event name in advance in the report. You can just write the event ID directly when configuring the smart event or calling the tracking code. EventID can be renamed in the event management page in the report.

#### 3) Number limit for custom events
##### 3.1) Smart events
No number limit. All smart events are calculated and analyzed by default.

##### 3.2) Code events
a) The system calculates the first 1,000 code events by default. After this limit is exceeded, the newly tracked code events are recorded in the "Pending events" list, and the calculation switch needs to be turned on manually for the system to compute them. Currently, batch switching-on for calculation is not supported, and only up to 10,000 code events can be calculated simultaneously;
b) "Pending events" location: "App management" > "Event management" > "Pending events". Data retained in "Pending events" is not involved in the calculation. Currently, only up to 100,000 pending events can be recorded simultaneously. However, some developers may introduce variables into custom event names, and if the variables are very discrete, the total number of custom events may exceed this limit. Consequently, the system will not be able to record more event names, making it impossible to find those events and switch the calculation on for them.
c) For code events that do not require further statistics gathering or pending events that do not need to be calculated, delete them in a timely manner. The deleted event are kept in the event recycle bin and can restored for calculation, but the data retained in the recycle bin is not involved in the calculation.

#### 4) Custom event labels and the number limit
Labels can be used to categorize the tracked events. By using the same EventID for multiple same-type or similar events to be tracked and assigning them different labels, you can distinguish the tracking of multiple events. EventID + label forms a specific event name. Proper classification of events facilitates the management and analysis of event data.
4.1) Label is not supported for smart events.
4.2) The label parameter is supported for code events, but it is not necessarily required to be called.
For each code event, the first 1,000 different labels are calculated by default. The excess labels will enter the pending list, and the calculation can be switched on in the Pending label management page. Only up to 2,000 different labels can be supported.

#### 5) Custom event parameters and the number limit
The event parameters can be used to add detailed description to the event, such as the scene, status or trigger of an event.
5.1) On-demand parameter configuration is not supported for smart events.
5.2) The parameters are supported for code events, but they are not necessarily required to be called.
Each code event supports up to 50 pairs of parameters simultaneously (10 pairs of parameters for Android v2.2.15 and below and iOS v2.2.27 and below). If the keys uploaded for the same code event are different due to version variance or other reasons, the report can display up to 100 keys. Each Key supports up to 1,000 different values.

### 2. Introduction to smart events
Smart event is a custom event tracking method that does not require pre-embedding and can be configured as needed at any time. Custom event tracking can be easily implemented at any time by setting up the report page after the App Analytics SDK is properly integrated.

This method has three advantages:
1) It greatly reduces the amount of codes for custom event tracking and shortens the development cycle;
2) It prevents duplication of work and erroneous data results caused by inconsistent data comprehension between the data requestor and the code developer;
3) It is required to be configured as needed after but not before the release of the app. This can eliminate the needs of re-release caused by data requirements and also avoid the situation where the lack of data has to be accepted due to the intention of avoiding re-release.

#### 2.1 Control support range
As different apps are implemented in different ways, we are naturally unable to support all control types. Currently, we support the following types of controls: Buttons, input boxes, radio buttons, check boxes and image clicking. They can be counted automatically.

#### 2.2 Smart event configuration steps
Step 1: Log in to the App Analytics platform and select the app to be configured to access the app report.
Step 2: Select the "App analysis" module, which is currently the default displayed module. If the selected app requires authorization, you may not have the right to view the module. In this case, contact the authorizer. Smart.png
Step 3: Shake to connect the device that has the app installed and follow the on-screen instructions to complete the setup. Save the configuration after completion.

#### 2.3 Precautions
1) You need to integrate the App Analytics SDK with version v2.0.X Beta or above;
2) Verify that the app ID is the same as the app ID of the app installed on the device.

### 3. Introduction to code events
For code events, the custom events can be tracked by calling the interface in the code for each custom event and passing the corresponding parameters.

Compared to smart events, code event tracking has the following advantages:
1) It is not restricted by the control type, so all types of controls are supported;
2) It supports setting up the event label;
3) It supports setting up event parameters.

### 4. Code event interface and parameters
**Interface:**  
1) If you only gather statistics of custom events without the need to set up the label and parameters, call the following method:

```
TalkingDataPlugin.TrackEvent("eventID");
```

2) Use the same Event ID to count similar event scenarios. When using Label to distinguish between specific scenarios, call the following methods:

```
TalkingDataPlugin.TrackEventWithLabel("eventID", "event_label");
```

3) If you use labels and event parameters to track events in more details, call the following methods:

```
TalkingDataPlugin.TrackEventWithParameters("eventID", "label", Your_dictionary);
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|eventID|String|Custom event name.<br>It may contain up to 64 Chinese characters, English letters, numbers and underscores. Spaces and other escape characters are not supported;
|event_label|String|Custom event label.<br>It may contain up to 64 Chinese characters, English letters, numbers and underscores. Spaces and other escape characters are not supported.
|parameters|Dictionary|The parameters and parameter values of the custom event.<br>Key is String; Value only supports strings and numbers; other types are considered as exceptions and discarded.<br>The report will show the frequency at which each value appears when the event occurs.<br>In case where the values are more discrete, such as promotional prices collected in the example, do not directly fill in the specific values. You should divide them into different ranges before passing (e.g. CNY9.9 can be defined into a price range of CNY5 - 10, so CNY5 - 10 can be passed); otherwise, the values may exceed the number limit on the platform. Discrete data also affects the data analysis.|

### 5. Code event examples
**Example 1:**  
We are tracking 5 marketing spaces on the homepage of an eCommerce app and collecting information about the marketing material categories and promotional prices. As the 5 scenarios are similar, so we use labels and add event parameters, and divide the parameter values into ranges (e.g. CNY9.9 can be defined into a price range of CNY5 - 10, so CNY5 - 10 can be passed) to avoid the values being too discrete and affecting the analysis and even exceeding the supported number limit. Call as follows:

```
// We can define EventID as click on the marketing space on the homepage and event_LABEL as specific space number
string recommandClick = "Click on the marketing space on the homepage";
Dictionary<string, object> dic = new Dictionary<string, object>();
dic.Add("Clothes", "Item category");
dic.Add("5~10", "price");
TalkingDataPlugin.TrackEventWithParameters(recommandClick, "The first ad slot", dic);
Dictionary<string, object> dic2 = new Dictionary<string, object>();
dic2.Add("Home appliances", "Item category");
dic2.Add("500~1000", "price");
TalkingDataPlugin.TrackEventWithParameters(recommandClick, "The third ad slot", dic2);
```

**Example 2:**  
We are recording the failure data of gamer for each checkpoint in a casual game and collecting specific information about the gamer. Call as follows:

```
// We can define EventID as lost battle
Dictionary<string, object> dic = new Dictionary<string, object>;
dic.Add("20-30", "Level");//Level range
dic.Add("Akaville in the swamp", "Checkpoint name"); //Checkpoint name
dic.Add("Active exit", "Cause of failure"); //Cause of failure
dic.Add("10000～12000", "coin"); //Amount of coins carried
TalkingDataPlugin.TrackEventWithParameters("Lost battle", null, dic);
```

## Advanced - standard events (in-app transactions)
### Place an order
#### 1. Purpose and use
The order placement interface is used to record the user's order placement behavior when using the app.
The order placement interface consists of three sub-interfaces: create order, add order details and successfully place order. The three sub-interfaces must be called in order; otherwise, it may not be able to generate the correct order data.

**Precautions:**  
1) OrderID is the key to identifying the transaction. Each order request needs a different OrderID; otherwise, it will be considered as a data duplication and discarded, resulting in order and income data deviation. If the order IDs for multiple successful order placements are identical, only the data from the first successful one will be recorded. The data from other ones will be treated as duplicates and discarded.
2) OrderID is structured and managed by yourself. You can use a way similar to UserID + timestamp + random number to define your own OrderID to ensure its uniqueness.

#### 2. Interface and parameters
##### a) Create order
**Interface:**

```
public static TalkingDataOrder CreateOrder(string orderId, int total, string currencyType)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|OrderID|string|Order ID. It is up to 64 characters, globally unique, and provided and maintained by the developer. (This ID is important. Contact customer service if there are any issues).<br>It is used to uniquely identify a transaction and for future account reconciliation with the system.<br>*If the order IDs for multiple successful payments are identical, only the data from the first successful one will be recorded. The data from other ones will be treated as duplicates and discarded.<br>If null or a empty string "" is passed, no order event is generated.|
|Total|int|The total amount of the order in cents. The currency is indicated by the "currencyType" after the amount.|
|CurrencyType|string|The currency of the order.<br>Please use the 3-letter code specified in ISO 4217 to mark the currency. For example, CNY, USD and EUR.<br>If you use other custom equivalents as cash, you can also use a 3-letter code that is not in ISO 4217 to pass the currency, and we will provide the currency rate setting function on the report page.|

##### b) Add order details (an order can have multiple items)
**Interface:**

```
public TalkingDataOrder AddItem(string itemId, string category, string name, int unitPrice, int amount)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|ItemID|string|Item ID|
|Category|string|Item category|
|Name|string|Item name|
|UnitPrice|int|Item's unit price in cents, e.g. 1300 = USD13|
|Amount|int|Quantity of items purchased|

##### c) Successfully place order
**Interface:**

```
public static void OnPlaceOrder(string accountId, TalkingDataOrder order)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|accountID|string|The account ID which is globally unique. If a null or empty string "" is passed, this event won't be generated.|
|Order|TalkingDataOrder|Order details|

#### 3. Example
User user001 successfully places an order. Order ID: orderId123; the order includes item 1: (ID: 007, TV, priced at CNY4999), and item 2: (ID: 008, refrigerator, priced at CNY3999); total: CNY8998. Call as follows:

```
TalkingDataOrder order = TalkingDataOrder.CreateOrder("orderId123", 899800, "CNY");
order.AddItem("007", "Home appliances", "TV", 499900, 1);
order.AddItem("008", "Home appliances", "Refrigerator", 399900, 1);
TalkingDataPlugin.OnPlaceOrder("user001", order);
```

### Successful order payment
#### 1. Purpose and use
The successful order payment interface is used to record the user's behavior of completing the order payment successfully.

#### 2. Interface and parameters
**Interface:**

```
public static void OnOrderPaySucc(string accountId, string payType, TalkingDataOrder order)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|accountID|string|The account ID which is globally unique. If a null or empty string "" is passed, this event won't be generated.|
|PayType|string|Payment method, such as Alipay, WeChat Pay, etc., up to 16 characters.|
|Order|TalkingDataOrder|Order details|

#### 3. Example
User user001 successfully completes order payment; the order includes item 1: (ID: 007, TV, priced at CNY4999), and item 2: (ID: 008, refrigerator, priced at CNY3999); total: CNY8998. Call as follows:

```
TalkingDataOrder order = TalkingDataOrder.CreateOrder("orderId123", 899800, "CNY");
order.AddItem("007", "Home appliances", "TV", 499900, 1);
order.AddItem("008", "Home appliances", "Refrigerator", 399900, 1);
TalkingDataPlugin.OnOrderPaySucc("user001", "Alipay", order);
```

### View items
#### 1. Purpose and use
This interface is used to describe the user's behavior of viewing the item details.

#### 2. Interface and parameters
**Interface:**

```
public static void OnViewItem(string itemId, string category, string name, int unitPrice)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|ItemID|string|Item ID|
|Category|string|Item category|
|Name|string|Item name|
|UnitPrice|int|Item's unit price in cents, e.g. 1300 = USD13|

#### 3. Example
The user views an item with ID 007 which is a TV priced at CNY4999 RMB in the category of home appliances. Call as follows:

```
TalkingDataPlugin.OnViewItem("007", "Home appliances", "TV", 499900);
```

### Add to cart
#### 1. Purpose and use
This interface is used to describe the user's behavior of adding an item to cart.

#### 2. Interface and parameters
**Interface:**

```
public static void OnAddItemToShoppingCart(string itemId, string category, string name, int unitPrice, int amount)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|Category|string|Item category|
|ItemID|string|Item ID|
|Name|string|Item name|
|UnitPrice|int|Unit price of the item in cents. For example: 600 EUR cents = EUR6. 100 USD cents = USD1.|
|Amount|int|Quantity of items|

#### 3. Example
The user adds an item with ID 007 to cart which is a TV priced at CNY4999 RMB in the category of home appliances. Call as follows:

```
TalkingDataPlugin.OnAddItemToShoppingCart("007", "Home appliances", "TV", 499900, 10);
```

### View cart
#### 1. Purpose and use
This interface is used to record the user's behavior of viewing the items in cart.

This interface consists of three sub-interfaces: create cart, add cart details and view cart. The three sub-interfaces must be called in order; otherwise, it may not be able to generate the correct cart viewing data.

#### 2. Interface and parameters
##### a) Create cart
**Interface:**

```
public static TalkingDataShoppingCart CreateShoppingCart()
```

**Parameter:**

N/A

##### b) Add cart details
**Interface:**

```
public TalkingDataShoppingCart AddItem(string itemId, string category, string name, int unitPrice, int amount)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|ItemID|string|Item ID|
|Category|string|Item category|
|Name|string|Item name|
|UnitPrice|int|Unit price of the item in cents. For example: 600 EUR cents = EUR6. 100 USD cents = USD1.|
|Amount|int|Quantity of items|

##### c) View cart
**Interface:**

```
public static void OnViewShoppingCart(TalkingDataShoppingCart shoppingCart)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|ShoppingCart|TalkingDataShoppingCart|Cart details. For more information, see Create cart.|

#### 3. Example
The user views the cart which contains two items: item 1 (ID: 007, TV, priced at CNY4999) and item 2 (ID: 008, refrigerator, priced at CNY3999). Call as follows:

```
TalkingDataShoppingCart shoppingCart = TalkingDataShoppingCart.CreateShoppingCart();
shoppingCart.AddItem("007", "Home appliances", "TV", 499900, 1);
shoppingCart.AddItem("008", "Home appliances", "Refrigerator", 399900, 1);
TalkingDataPlugin.OnViewShoppingCart(shoppingCart);
```

## Advanced - user quality assessment
#### 1. Purpose and use
With the user quality assessment function, abnormal devices among the covered devices can be identified based on device information verification, IP dispersion analysis and SDK attitude recognition. Abnormal devices can be further classified as silent devices and other abnormal devices.

"Silent device" refers to a device whose collected attitude data does not match the user behavioral attitude distribution pattern of normal devices when the anti-cheating switch is turned on in the SDK. This type of data can be used as a judging factor to identify abnormal devices and improve the accuracy of abnormal device identification.

**IMPORTANT:**  
1) The anti-cheating function interface is enabled by default. You can call the interface to disable it;
2) After it is enabled, the SDK will start the service in an independent process and implement sensor data collection and user behavior identification. The resulting data is used to analyze silent users in user quality assessment. This process consumes only a very small amount of data traffic and has no impact on the app experience.
3) Disabling this interface weakens the user quality analysis capability, but does not turn off the entire function. The system still continues to perform abnormal device analysis based on device information verification, IP dispersion and other factors.
4) Version 2.3.89 and above supports the configuration of anti-cheating switch to enhance user quality assessment.

#### 2. Interface description
**Interface:**

```
public static void SetAntiCheatingEnabled(bool enable)
```

#### 3. Example

```
TalkingDataPlugin.SetAntiCheatingEnabled(false);
```

## Advanced - push notification marketing
### 1. Purpose and use
The push notification marketing function provides the developers with a targeted push notification capability which, together with the capabilities to group, profile and predict the users, enable the developers to target different audience types with different push notifications, analyze data in real time, continuously compare results and optimize marketing activities, thus reducing undesired user annoyance and improving the conversion effectiveness.

The TalkingData platform not only offers the TalkingData push channel, but also works together with Getui, JPush and other push platforms to utilize their analysis capabilities to achieve real-time precise push. If you have already integrated those two third-party push channels into your app, you can use this method to quickly enable targeted push notification.

**Integration and setup steps:**  
1) Integrate the TalkingData SDK
2) Make the necessary calls in the code to ensure that the push function is correctly integrated. For details, refer to the integration and configuration documentation of each platform and each channel.
3) After configuring, perform push testing on the TalkingData platform.

**IMPORTANT:**  
SDK version requirements: Android V2.2.4 and above.

### 2) Integration and configuration
#### 2.1 Android
##### a) Integrate TalkingData Push
Add the following call method to enable your app to receive push notifications. Please note that your app will run long-term in the background while the push capability is obtained.

**Modify the AndroidManifest.xml file**

1) Add the necessary permissions to the push function:

```
<!--Allow the app to run on startup to receive push notifications-->
<uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED"></uses-permission>
<!--Send a persistent broadcast-->
<uses-permission android:name="android.permission.BROADCAST_STICKY"></uses-permission>
<!--Modify global system settings-->
<uses-permission android:name="android.permission.WRITE_SETTINGS"></uses-permission>
<!--Modify global system settings-->
<uses-permission android:name="android.permission.VIBRATE"></uses-permission>
<!--Detect Wi-Fi changes to control optimal heartbeat interval for different Wi-Fi networks and ensure push channel stability-->
<uses-permission android:name="android.permission.CHANGE_WIFI_STATE"></uses-permission>
<!--This permission is optional and is used to turn on the screen when a push notification is received-->
<uses-permission android:name="android.permission.WAKE_LOCK"></uses-permission>
```

2) Add the necessary Service for TalkingData

```
<service android:name="com.apptalkingdata.push.service.PushService" android:process=":push" android:exported="true"></service>
```

3) Add BroadCastReceiver required by TalkingData to support receiving messages:

```
<receiver android:name="com.apptalkingdata.push.service.PushServiceReceiver" android:exported="true">
	<intent-filter>
		<action android:name="android.intent.action.CMD"></action>
		<action android:name="android.talkingdata.action.notification.SHOW"></action>
		<action android:name="android.talkingdata.action.media.MESSAGE"></action>
		<action android:name="android.intent.action.BOOT_COMPLETED"></action>
		<action android:name="android.net.conn.CONNECTIVITY_CHANGE"></action>
		<action android:name="android.intent.action.USER_PRESENT"></action>
	</intent-filter>
</receiver>
<receiver android:name="com.tendcloud.tenddata.TalkingDataAppMessageReceiver" android:enabled="true">
	<intent-filter>
		<action android:name="android.talkingdata.action.media.SILENT"></action>
		<action android:name="android.talkingdata.action.media.TD.TOKEN"></action>
	</intent-filter>
	<intent-filter>
		<action android:name="com.talkingdata.notification.click"></action>
		<action android:name="com.talkingdata.message.click"></action>
	</intent-filter>
</receiver>
```

##### b) Use TalkingData together with third-party push platforms
TalkingData supports the delineation of targeted user groups from the platform, and together with third-party push platforms, it can directly send push notifications and collect real-time effectiveness data; currently, two third-party push platforms are supported: Getui and JPush. If you already use a third-party push platform, this approach will make it easier for you to implement targeted push notification and verify the effectiveness.

**Integrate third-party push platforms**  
You need to apply for an account on the third-party push platform and integrate the third-party push capability. After integration, you need to carry out testing to make sure that the third-party push platform can push the notifications.

Add BroadCastReceiver required by TalkingData App Analytics

1) Getui

```
<receiver android:name="com.tendcloud.tenddata.TalkingDataAppMessageReceiver" android:enabled="true">
	<intent-filter>
		<!--Must be added-->
		<action android:name="com.talkingdata.notification.click"></action>
		<action android:name="com.talkingdata.message.click"></action>
	</intent-filter>
	<intent-filter>
		<!--Note: H0DPYSxUkR9NFoWnvff656 should be replaced with the developer's own AppID-->
		<action android:name="com.igexin.sdk.action.H0DPYSxUkR9NFoWnvff656"></action>
	</intent-filter>
</receiver>
```

2) JPush

```
<receiver android:name="com.tendcloud.tenddata.TalkingDataAppMessageReceiver" android:enabled="true">
	<intent-filter>
		<!--Must be added-->
		<action android:name="com.talkingdata.notification.click"></action>
		<action android:name="com.talkingdata.message.click"></action>
	</intent-filter>
	<intent-filter>
		<!--If JPush is used, this must be added-->
		<action android:name="cn.jpush.android.intent.REGISTRATION"></action>
		<action android:name="cn.jpush.android.intent.MESSAGE_RECEIVED"></action>
		<category android:name="Name of your app package"></category>
	</intent-filter>
</receiver>
```

3) Configure the push key

After completing the integration described above, you will also need to configure the push-related keys on the TalkingData platform. Please go to "Push marketing" > "Push configuration" to add those configurations to the Android platform.

#### 2.2 iOS
For the iOS platform, push uses Apple's official APNS channel. You need to apply for Apple's APNS service before combining it with TalkingData.

##### 1) Register (access to iOS' push system)
Before using push marketing, please make sure that the app can receive remote push notifications normally. Please refer to the following code for registering with remote notification:

```
void Start () {
	TalkingDataPlugin.SessionStarted("your_app_id", "your_channel_id");
#if UNITY_IPHONE
#if UNITY_5
	UnityEngine.iOS.NotificationServices.RegisterForNotifications(
		UnityEngine.iOS.NotificationType.Alert |
		UnityEngine.iOS.NotificationType.Badge |
		UnityEngine.iOS.NotificationType.Sound);
#else
	NotificationServices.RegisterForRemoteNotificationTypes(
		RemoteNotificationType.Alert |
		RemoteNotificationType.Badge |
		RemoteNotificationType.Sound);
#endif
#endif
	// other code
}
```

##### 2) Pass DeviceToken
In the `Update` method, call the `SetDeviceToken` method:

```
void Update () {
#if UNITY_IPHONE
	TalkingDataPlugin.SetDeviceToken();
#endif
	// other code
}
```

##### 3) Pass message
In the `Update` method, call the `HandlePushMessage` method:

```
void Update () {
#if UNITY_IPHONE
	TalkingDataPlugin.HandlePushMessage();
#endif
	// other code
}
```

## Advanced - eAuth
### 1. Purpose and use
eAuth is a stable SMS verification service. It is simple to integrate and provides a full range of capabilities from sending verification codes to security verification. We offer up to 10,000 SMS verification messages/day free of charge.

eAuth is perfect for the following scenarios:
1) Service-type apps, as they require a mobile number for the user to register;
2) Any apps, as they can communicate with the user better if the user's mobile number is provided;
3) In case of forgotten password, SMS verification can be used as a temporary login method.

eAuth can be enabled and integrated by following the steps below:
1) Enable eAuth in the report and obtain the key ID;
2) Initialize eAuth;
3) Interface call: Apply for "SMS verification code".
4) Interface call: Verify "SMS verification code".

**IMPORTANT:**  
eAuth is integrated in the App Analytics SDK. Version 2.1 or above is required.

### 2. Enable the function and obtain the key IDs
Step 1: On the App Analytics platform, create or select a product. Find the eAuth tab and click "Enable now" to enable it for the product.
Step 2: After the function is enabled, the system automatically assigns key IDs. Click "Show" to view them.

**Key ID description:**  
1) App ID: This is the ID used to uniquely identify the app and the same as the app ID in the statistical analysis report;
2) Secret ID: This is the security code needed for the integration on the client and used to avoid others imitating the app to send SMS messages;
3) Server Security: This identification code is used for identity authentication when the server send SMS messages.

### 3. Initialize eAuth
Because this is an additional feature of the App Analytics SDK, the developer has to separately call eAuth's initialization method.

**Interface:**

In the `Start` method, call the init method to pass `App ID` and `Secret ID`:

```
public static void Init(string appKey, string secretId)
```

**Example:**

```
void Start () {
	TalkingDataSMSPlugin.Init("App ID", "Secret ID");
}
```

### 4. Request "SMS verification code" interface
This method is used to initiate a request to the TalkingData eAuth server for SMS verification code. If the request is normal, the server will notify the carrier channel to send the verification code.

To prevent malicious requests, the system restricts the requests for a single app as below:
1) For each mobile number, multiple SMS messages cannot be requested within 60 seconds;
2) For each mobile number, up to three SMS verification messages can be requested within 10 minutes;
3) For each mobile number, up to five SMS verification messages can be requested within 24 hours;
4) For each device, up to five SMS verification messages can be requested within 24 hours.

**Interface:**

```
// Request verification code
public static void ApplyAuthCode(string countryCode, string mobile, SuccDelegate succMethod, FailedDelegate failedMethod)
// Request verification code again
public static void ReapplyAuthCode(string countryCode, string mobile, string requestId, SuccDelegate succMethod, FailedDelegate failedMethod)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|countryCode|string|Country code of the mobile number; it uses the generic country codes without a plus sign; currently, only "Mainland China" is supported.<br>Usually, this is required to be entered by the user on the client, e.g. 86.|
|mobile|string|The user's mobile number; for Mainland China, it is an 11-digit number.<br>Usually, this is required to be entered by the user on the client.|
|requestId|string|RequestId is the ID of the corresponding request returned by the system each time the send SMS interface is called.<br>It is used for the situation where the user doesn't receive the verification code and requests it to be sent. Enter it only when calling the reapplyAuthCode interface. When the RequestId parameter is present, the system will find the same original verification code and send it again. If without this parameter, the system will send a new verification code.|
|succMethod|SuccDelegate|The callback interface when verification code successfully requested.|
|failedMethod|FailedDelegate|The callback interface when verification code request fails.|

**Example 1:**

The user uses a Mainland China mobile number 13800138000 to register.

```
void OnApplyBtnClick {
	TalkingDataSMSPlugin.SuccDelegate succMethod = new TalkingDataSMSPlugin.SuccDelegate(this.OnApplySucc);
	TalkingDataSMSPlugin.FailedDelegate failedMethod = new TalkingDataSMSPlugin.FailedDelegate(this.OnApplyFailed);
	TalkingDataSMSPlugin.ApplyAuthCode("86", "13800138000", succMethod, failedMethod);
}

void OnApplySucc(string requestId) {
	Debug.Log ("OnApplySucc:" + requestId);
}

void OnApplyFailed(int errorCode, string errorMessage) {
	Debug.Log ("OnApplyFailed:" + errorCode + " " + errorMessage);
}
```

**Example 2:**

During registration, the reception is poor and the user fails to receive the SMS message. After waiting 60 seconds, the user taps the "Resend" button.

```
void OnReapplyBtnClick() {
	TalkingDataSMSPlugin.SuccDelegate succMethod = new TalkingDataSMSPlugin.SuccDelegate(this.OnApplySucc);
	TalkingDataSMSPlugin.FailedDelegate failedMethod = new TalkingDataSMSPlugin.FailedDelegate(this.OnApplyFailed);
	TalkingDataSMSPlugin.ReapplyAuthCode("86", "13800138000", "request_id", succMethod, failedMethod);
}

void OnApplySucc(string requestId) {
	Debug.Log ("OnApplySucc:" + requestId);
}

void OnApplyFailed(int errorCode, string errorMessage) {
	Debug.Log ("OnApplyFailed:" + errorCode + " " + errorMessage);
}
```

### 5. Verify "SMS verification code" interface

This method is used for the app to request the verification code from the server and receive the verification result.
    
Description: The verification code is valid for 30 minutes. If verification is not successful within this period, it will expire.

**Interface:**

```
// Send a request to the SDK after the user enters the verification code
public static void VerifyAuthCode(string countryCode, string mobile, string authCode, SuccDelegate succMethod, FailedDelegate failedMethod)
```

**Parameter:**

|Parameter|Type|Description|
|---|---|---|
|countryCode|string|Country code selected by the user|
|mobile|string|Mobile number entered by the user|
|authCode|string|SMS verification code entered by the user|
|succMethod|SuccDelegate|The callback interface when verification code successfully verified.|
|failedMethod|FailedDelegate|The callback interface when verifying verification code fails.|

**Example:**

The user enters the correct verification code 243573 to verify the mobile number 13800138000.

```
void OnApplyBtnClick() {
	TalkingDataSMSPlugin.SuccDelegate succMethod = new TalkingDataSMSPlugin.SuccDelegate(this.OnVerifySucc);
	TalkingDataSMSPlugin.FailedDelegate failedMethod = new TalkingDataSMSPlugin.FailedDelegate(this.OnVerifyFailed);
	TalkingDataSMSPlugin.VerifyAuthCode("86", "13800138000", "auth_code", succMethod, failedMethod);
}

void OnVerifySucc(string requestId) {
	Debug.Log ("OnVerifySucc:" + requestId);
}

void OnVerifyFailed(int errorCode, string errorMessage) {
	Debug.Log ("OnVerifyFailed:" + errorCode + " " + errorMessage);
}
```

### 6. Special notes on RequestId

Each time a request or verification is successful, a RequestID will be returned. This ID has many uses worth noting.
When requesting SMS verification code, using the RequestID can properly handle the "resend" issue. In case of resending, the user will not receive multiple different verification codes. This prevents the user from being confused about which code to use.
When verification is successful, the RequestID helps distinguish between multiple SMS verification requests sent at the same time from the same mobile number. In this way, SMS verification can be made possible at the same time in various scenarios at different security levels without causing confusion or leading to security issues.
   
**Details of the codes returned by the interface (RequestId):**

|Returned code|Description|
|---|---|---|
|451|SECERT\_CHECK\_FAIL|
|452|EXCEED\_PHONE\_APP\_DAY\_QUOTA|
|453|APP\_KEY\_CHECK\_FAIL|
|455|ALREADY\_LOCKED|
|456|LACK\_REQUST\_PARAM|
|457|APPLY\_TOO\_FREQUENTLY|
|458|EXCEED\_DEVICE\_APP\_DAY\_QUOTA|
|459|EXCEED\_PHONE\_DAY\_QUOTA|
|460|VERIFY\_FAIL\_NOT\_AMTCH|
|461|VERIFY\_FAIL\_EXPIRED|
|462|VERIFY\_FAIL\_USED|
|463|BEEN\_LOCKED|
|471|REGTISTER\_JSON\_INVALID|
|472|DUPLICATE\_REGISTER\_OR\_ADD|
|473|SERVER\_ERROR\_IN\_REGISTER|
|474|INVALID\_DOMAIN|
|475|INVALID\_APPKEY|
|480|EXCEED\_DEVICE\_DAY\_QUOTA|
|481|EXCEED\_APP\_DAY\_QUOTA|
|482|EXCEED\_PHONE\_SP_VISIT\_FREQUENCY\_LIMIT|
|483|EXCEED\_DEVELOPER\_DAY\_QUOTA|
|484|INVALID\_QUERY\_PARAM|
|485|NO\_QUERY\_RESULT|
|486|INVALID\_PHONE\_NUM|
|487|EXCEED\_QUOTA|
|551|SP\_SERVER\_FAIL|
|552|DECOMPRESS\_HTTP\_BODY\_FAIL|
|553|NO\_SUCH\_RECORD|
|554|CHECK\_CRC\_FAIL|
|557|SERVER\_STORAGE\_ERROR|
|558|JSON\_PARSE\_ERROR|
|559|SERVER\_IO\_FAIL|
|560|SERVER\_GEN\_DECRET\_FAIL|
|561|CHECK\_QUOTA\_BUILD\_KEY\_FAIL|
|570|NO\_MONTH\_DB|
|601|Only one verification code can be requested every 60 seconds.|
|602|Verification request too frequent. Try again later.|
|604|Internal error with the SDK|
|605|Error with the parameters passed by the interface|
|Other|Refer to NSURLError|


