//
//  ProductLocaleHandler.h
//  
//
//
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>


@interface ProductLocaleHandler : NSObject <SKProductsRequestDelegate> {
 @private
    NSArray *_cachedAppleProducts;
    NSArray *_unfinishedTransactions;
}

@property (nonatomic, readonly, retain) NSArray *cachedAppleProducts;
@property (nonatomic, readonly, retain) NSArray *unfinishedTransactions;

- (void)fetchAppleProductsWithIDs:(NSArray *)appleProductIDs
                completionHandler:(void (^)(NSError *error, NSArray *appleProducts))completionHandler;
+ (id)sharedManager;

@end
