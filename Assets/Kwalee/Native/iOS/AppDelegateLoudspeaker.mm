#import "AppDelegateLoudspeaker.h"

#pragma mark AppDelegateLoudspeaker

#define kPushNotificationsDidRegister @"PushNotificationsDidRegister"

static AppDelegateLoudspeaker *_instance = [AppDelegateLoudspeaker sharedInstance];

@interface AppDelegateLoudspeaker()
@end

@implementation AppDelegateLoudspeaker

#pragma mark Object Initialization

@synthesize storedUrl;
@synthesize storedNotification;
@synthesize madeUnityConnection;

char *const C_SHARP_CLASS_NAME = "AppDelegateLoudspeaker";

+ (AppDelegateLoudspeaker *)sharedInstance
{
    return _instance;
}

+ (void)initialize {
    if(!_instance) {
        _instance = [[AppDelegateLoudspeaker alloc] init];
    }
}

- (void)unityReadyForMessages
{
    madeUnityConnection = true;
    
    if (storedNotification)
    {
        [self sendNotification:storedNotification];
    }
    
    if (storedUrl)
    {
        UnitySendMessage( C_SHARP_CLASS_NAME, "_BroadcastUIApplicationDidOpenURL", [storedUrl UTF8String]);
    }
}

- (void)sendNotification:(NSDictionary*) notification
{
    if (madeUnityConnection)
    {
        NSString* strRet;
    
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:notification
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:&error];
    
        if (!jsonData)
        {
            NSLog(@"NSJSONSerialization: error: %@", error.localizedDescription);
            strRet = @"{}";
        }
        else
        {
            strRet = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        }
    
        UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidReceiveRemoteNotificationNotification", [strRet UTF8String]);
    }
    else
    {
        [self setNotification:notification];
    }
}

- (void)setNotification:(NSDictionary*) notification
{
    storedNotification = notification;
}

- (void)setUrl:(NSString*) url
{
    storedUrl = url;
}

#pragma mark Create/Destroy

- (id)init
{
    self = [super init];
    
    if (!self)
    {
        return nil;
    }
    
    madeUnityConnection = false;
    
    UnityRegisterAppDelegateListener(self);
    
    return self;
}

- (void)dealloc
{
    UnityUnregisterAppDelegateListener(self);
    
#if !__has_feature(objc_arc)
    [super dealloc];
#endif
}

- (void)didRegisterForRemoteNotificationsWithDeviceToken: (NSNotification*)notification
{    
    NSString *rawToken = [NSString stringWithFormat:@"%@", notification.userInfo];
    NSString *formattedToken = [[rawToken stringByTrimmingCharactersInSet:[NSCharacterSet symbolCharacterSet]] stringByReplacingOccurrencesOfString:@" " withString:@""];
    
    if (formattedToken)
    {
        UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidRegisterForRemoteNotificationsNotification", [formattedToken UTF8String]);
    }
    
    if (![[UIApplication sharedApplication] respondsToSelector:@selector(application:didRegisterUserNotificationSettings:)])
    {
        [[NSNotificationCenter defaultCenter] postNotificationName:kPushNotificationsDidRegister object:nil];
    }
}

- (void)didFailToRegisterForRemoteNotificationsWithError:(NSError *)error
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidFailToRegisterForRemoteNotificationsNotification", [error.description UTF8String]);
}

- (void)didReceiveRemoteNotification:(NSNotification*)notification
{
    AppDelegateLoudspeaker *loudspeaker = [AppDelegateLoudspeaker sharedInstance];
    
    [loudspeaker sendNotification:notification.userInfo];
}

- (void)didReceiveLocalNotification:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidReceiveLocalNotificationNotification", "");
}

- (void)onOpenURL:(NSNotification*)notification
{
    NSDictionary* userInfo = notification.userInfo;
    
    NSURL* url = [userInfo objectForKey:@"url"];
    
    UnitySendMessage( C_SHARP_CLASS_NAME, "_BroadcastUIApplicationDidOpenURL", [url.absoluteString UTF8String]);
}

- (void)applicationDidReceiveMemoryWarning:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidReceiveMemoryWarningNotification", "");
}

- (void)applicationSignificantTimeChange:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationSignificantTimeChangeNotification", "");
}

- (void)applicationWillChangeStatusBarFrame:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationWillChangeStatusBarFrameNotification", "");
}

- (void)applicationWillChangeStatusBarOrientation:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationWillChangeStatusBarOrientationNotification", "");
}

#pragma mark LifeCycleListener Handlers

- (void)didFinishLaunching:(NSNotification*)notification
{
    AppDelegateLoudspeaker *loudspeaker = [AppDelegateLoudspeaker sharedInstance];
    
    NSDictionary *remoteNotification = [notification.userInfo objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
    if (remoteNotification)
    {
        [loudspeaker setNotification:remoteNotification];
    }
    
    if ([notification.userInfo objectForKey:UIApplicationLaunchOptionsURLKey]) {
        NSURL *url = [notification.userInfo objectForKey:UIApplicationLaunchOptionsURLKey];
        
        [loudspeaker setUrl:url.absoluteString];
    }
    
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0; // clear the badge
}

- (void)didBecomeActive:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidBecomeActiveNotification", "");
}

- (void)willResignActive:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationWillResignActiveNotification", "");
}

- (void)didEnterBackground:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationDidEnterBackgroundNotification", "");
}

- (void)willEnterForeground:(NSNotification*)notification
{
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0; // clear the badge
    
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationWillEnterForegroundNotification", "");
}

- (void)willTerminate:(NSNotification*)notification
{
    UnitySendMessage( C_SHARP_CLASS_NAME , "_BroadcastUIApplicationWillTerminateNotification", "");
}

extern "C" void _UnityReadyToRecieveMessages()
{
    AppDelegateLoudspeaker *loudspeaker = [AppDelegateLoudspeaker sharedInstance];
    
    [loudspeaker unityReadyForMessages];
}

@end
