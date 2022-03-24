#include <CoreTelephony/CTTelephonyNetworkInfo.h>
#include <CoreTelephony/CTCarrier.h>
#include <AppTrackingTransparency/ATTrackingManager.h>
#import "Unity/UnityInterface.h"
#if ADS_KWALEE != 1
#import<FBAudienceNetwork/FBAdSettings.h>
#endif
#import <FBSDKCoreKit/FBSDKSettings.h>

extern "C" int _GetNumAppleKeyboards()
{
    NSUserDefaults * defs = [NSUserDefaults standardUserDefaults];
    NSArray *arrayOfKeyboardNames = [defs arrayForKey:@"AppleKeyboards"];
    return (int)arrayOfKeyboardNames.count;
}

extern "C" char* _GetAppleKeyboard(int idx)
{
    char* ret = NULL;
    NSUserDefaults * defs = [NSUserDefaults standardUserDefaults];
    NSArray *arrayOfKeyboardNames = [defs arrayForKey:@"AppleKeyboards"];
    if( arrayOfKeyboardNames && idx >= 0 && idx < arrayOfKeyboardNames.count)
    {
        ret = AllocCString(arrayOfKeyboardNames[idx]);
    }
    
    return ret;
}

extern "C" char* _GetLanguageCode()
{
    char* ret = NULL;
    
    NSString *s = [[NSLocale preferredLanguages] objectAtIndex:0];
    ret = AllocCString(s);
    
    return ret;
}

extern "C" char* _GetCountryCodeFromNetwork()
{
    char* ret = NULL;
    
    CTTelephonyNetworkInfo *network_Info = [CTTelephonyNetworkInfo new];
    CTCarrier *carrier = network_Info.subscriberCellularProvider;
    
    ret = AllocCString(carrier.isoCountryCode);
    
    return ret;
}

extern "C" char* _GetRegion()
{
    char* ret = NULL;
    
    NSLocale* currentLocale = [NSLocale currentLocale];
    ret = AllocCString(currentLocale.countryCode);
    
    NSLog(@"currentLocale.countryCode = %s", ret);
    
    return ret;
}

extern "C" void _SetAdvertiserTrackingEnabled(bool trackingEnabled)
{
    if (@available(iOS 14, *)) {
        NSLog(@"FBAdSettings setAdvertiserTrackingEnabled: %@", trackingEnabled ? @"YES" : @"NO");        
        #if ADS_KWALEE != 1
        [FBAdSettings setAdvertiserTrackingEnabled: trackingEnabled];
        NSLog(@"&&&& FBAdSettings: %@", trackingEnabled ? @"YES" : @"NO");
        #endif
        [FBSDKSettings setAdvertiserTrackingEnabled: trackingEnabled];
    }
}

extern "C" void _RequestIDFA()
{
    if (@available(iOS 14, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            bool restricted = (status == ATTrackingManagerAuthorizationStatusRestricted);
            bool trackingEnabled = (status == ATTrackingManagerAuthorizationStatusAuthorized);
            NSLog(@"&&&& ATTrackingManagerAuthorizationStatus trackingEnabled: %u, restricted: %u", trackingEnabled, restricted);
            
            _SetAdvertiserTrackingEnabled(trackingEnabled);
            
            NSString* statusString = [NSString stringWithFormat:@"%lu", (unsigned long)[ATTrackingManager trackingAuthorizationStatus]];
            UnitySendMessage("DeviceSettingsManager", "_OnIDFAReceived", [statusString UTF8String]);
        }];
    } else {
        // not ios 14, return undetermined
        NSString* resultString = [NSString stringWithFormat:@"%d", 0];
        UnitySendMessage("DeviceSettingsManager", "_OnIDFAReceived", [resultString UTF8String]);
    }
}

extern "C" int _IsSoftIDFAAvailable()
{
    int available = 0;
    
    if (@available(iOS 14.5, *)) {
        available = 1;
    }
    
    return available;
}

extern "C" int _IsHardIDFAAvailable()
{
    int available = 0;
    
    if (@available(iOS 14.0, *)) {
        available = 1;
    }
    
    return available;
}
