/// @creator: Slipp Douglas Thompson
/// @license: WTFPL
/// @why: Because this functionality should be built-into Unity.
/// @intended project path: Assets/Plugins/iOS/AppDelegateLoudspeaker.h
/// @intersource: https://gist.github.com/capnslipp/036f7c98f5ccf42e8428
#pragma once

#include "PluginBase/AppDelegateListener.h"


@interface AppDelegateLoudspeaker : NSObject<AppDelegateListener>

+ (AppDelegateLoudspeaker *)sharedInstance;

@property (readonly) NSDictionary* storedNotification;
@property (readonly) NSString* storedUrl;
@property (readonly) bool madeUnityConnection;

- (void)unityReadyForMessages;
- (void)sendNotification:(NSDictionary*) notification;
- (void)setNotification:(NSDictionary*) notification;
- (void)setUrl:(NSString*) url;

@end
