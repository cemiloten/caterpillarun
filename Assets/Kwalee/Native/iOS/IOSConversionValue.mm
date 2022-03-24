#import <StoreKit/StoreKit.h>

extern "C" void _UpdateConversionValue(int value)
{
    NSLog(@"UpdateConversionValue value: %d", value);
    if (@available(iOS 14.0, *)) {
        [SKAdNetwork updateConversionValue:value];
    } else {
        NSLog(@"UpdateConversionValue not available on this iOS version");
    }
}
