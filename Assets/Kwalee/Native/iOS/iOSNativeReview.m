#import "iOSNativeReview.h"
#import <StoreKit/StoreKit.h>

@implementation iOSNativeReview
{
}

# pragma mark - C API

bool requestReview()
{
    if([UIDevice currentDevice].systemVersion.floatValue >= 10.3)
    {
        [SKStoreReviewController requestReview];
        return true;
    }
    return false;
}

int getNWindows()
{
    return [UIApplication sharedApplication].windows.count;
}

@end
