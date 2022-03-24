//
//  ProductLocaleHandler
//
//
//

#import "ProductLocaleHandler.h"


char *const C_SHARP_CLASS_NAME = "DeviceSettingsManager";

typedef void (^ProductLocaleHandlerFetchProductsCompletionHandler)(NSError *error, NSArray *appleProducts);

@interface ProductLocaleHandler () {
    ProductLocaleHandlerFetchProductsCompletionHandler _fetchProductsCompletionHandler;
}

@property (nonatomic, retain) NSArray *cachedAppleProducts;
@property (nonatomic, copy) ProductLocaleHandlerFetchProductsCompletionHandler fetchProductsCompletionHandler;

- (SKProduct *)appleProductWithID:(NSString *)productID;

@end

static ProductLocaleHandler *sharedMyManager = nil;

@implementation ProductLocaleHandler

@synthesize cachedAppleProducts = _cachedAppleProducts;
@synthesize fetchProductsCompletionHandler = _fetchProductsCompletionHandler;


+ (id)sharedManager {
    @synchronized(self) {
        if(sharedMyManager == nil)
        {
            sharedMyManager = [[super allocWithZone:NULL] init];
        }
    }
    return sharedMyManager;
}

#pragma mark - Memory managemnet

- (id)init
{
    if ((self = [super init])) {
        _cachedAppleProducts = [[NSArray alloc] init];
    }
    
    return self;
}

- (void)dealloc
{
}

#pragma mark - Product handling

- (void)fetchAppleProductsWithIDs:(NSArray *)appleProductIDs
                completionHandler:(void (^)(NSError *error, NSArray *appleProducts))completionHandler
{
    self.fetchProductsCompletionHandler = completionHandler;
    
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:[NSSet setWithArray:appleProductIDs]];
    request.delegate = self;
    [request start];
}

- (SKProduct *)appleProductWithID:(NSString *)productID
{
    SKProduct *selectedProduct = nil;
    
    for (SKProduct *product in self.cachedAppleProducts) {
        if ([product.productIdentifier isEqualToString:productID]) {
            selectedProduct = product;
            break;
        }
    }
    
    return selectedProduct;
}

#pragma mark - Caching

- (void)cacheAppleProducts:(NSArray *)appleProducts
{
    NSMutableSet *appleProductSetToCache = [NSMutableSet setWithArray:appleProducts];
    [appleProductSetToCache addObjectsFromArray:self.cachedAppleProducts];
    
    self.cachedAppleProducts = [NSArray arrayWithArray:[appleProductSetToCache allObjects]];
}


#pragma mark - SKProductsRequestDelegate protocol

- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
    [self cacheAppleProducts:response.products];
    
    if (self.fetchProductsCompletionHandler) {
        self.fetchProductsCompletionHandler(nil, response.products);
        self.fetchProductsCompletionHandler = nil;
    }
}

- (void)request:(SKRequest *)request didFailWithError:(NSError *)error
{
    if (self.fetchProductsCompletionHandler) {
        self.fetchProductsCompletionHandler(error, nil);
        self.fetchProductsCompletionHandler = nil;
    }
}

@end

extern "C" void _RequestProductLocale(char* productID)
{
    ProductLocaleHandler *manager = [ProductLocaleHandler sharedManager];
    
    NSArray *productIDs = @[[NSString stringWithUTF8String:productID]];
    
    [manager fetchAppleProductsWithIDs:productIDs
                                          completionHandler:^(NSError *error, NSArray *appleProducts){
                                              
                                              NSNumberFormatter *numberFormatter = [[NSNumberFormatter alloc] init];
                                              [numberFormatter setFormatterBehavior:NSNumberFormatterBehavior10_4];
                                              [numberFormatter setNumberStyle:NSNumberFormatterCurrencyStyle];
                                              
                                              for (SKProduct *appleProduct in appleProducts)
                                              {
                                                  NSLocale *storeLocale = appleProduct.priceLocale;
                                                  NSString *storeCountry = (NSString*)CFLocaleGetValue((CFLocaleRef)storeLocale, kCFLocaleCountryCode);
                                                  
                                                  UnitySendMessage(C_SHARP_CLASS_NAME, "_OnProductLocaleFetched", [storeCountry UTF8String] );
                                                  break;
                                              }
                                          }];
}
