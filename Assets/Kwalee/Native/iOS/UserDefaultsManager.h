#pragma once

@interface UserDefaultsManager : NSObject

+ (UserDefaultsManager *)sharedInstance;

- (void)copyDefaults:(NSUserDefaults*)source destination:(NSUserDefaults*)destination;

@end
