#import "UserDefaultsManager.h"

#pragma mark UserDefaultsManager

static UserDefaultsManager *_instance = [UserDefaultsManager sharedInstance];

@interface UserDefaultsManager()
@end

@implementation UserDefaultsManager

#pragma mark Object Initialization

+ (UserDefaultsManager *)sharedInstance
{
    return _instance;
}

+ (void)initialize {
    if(!_instance) {
        _instance = [[UserDefaultsManager alloc] init];
    }
}

#pragma mark Create/Destroy

- (id)init
{
    self = [super init];
    
    if (!self)
    {
        return nil;
    }
    
   // [self copyDefaults:[[NSUserDefaults alloc] initWithSuiteName:@"group.com.kwalee.playPhotoLive"] destination:[NSUserDefaults standardUserDefaults]];
    
    return self;
}

- (void)dealloc
{
#if !__has_feature(objc_arc)
    [super dealloc];
#endif
}

- (void)copyDefaults:(NSUserDefaults*)source destination:(NSUserDefaults*)destination
{
    //NSArray *keys = [[source dictionaryRepresentation] allKeys];
    //
    //for(NSString* key in keys)
    //{
    //    [destination setObject:[source objectForKey:key] forKey:key];
    //}
    
    //[destination synchronize];
}

extern "C" void _CopyStandardDefaultsToShared()
{
    UserDefaultsManager *manager = [UserDefaultsManager sharedInstance];
    
    [manager copyDefaults:[NSUserDefaults standardUserDefaults] destination:[[NSUserDefaults alloc] initWithSuiteName:@"group.com.kwalee.playPhotoLive"]];
}

@end
