//
//  XYPlatform.h
//  XYPlatform
//
//  Created by Eason on 21/04/2014.
//  XY SDK v1.4.0 iPhoneOS XCode5
//  全民奇迹

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "XYPlatformDefines.h"


#pragma mark XYPlatform 基本信息

@interface XYPlatform : NSObject

/**
 *   @brief 获取XYPlatform实例
 */
+ (XYPlatform*)defaultPlatform;

/**
 *   @brief 获取SDK版本号
 */
+ (NSString*)sdkVersion;

@end


#pragma mark XYPaltform 初始化配置

@interface XYPlatform (XYConfiguration)

/**
 *	@brief  平台初始化方法
 *
 *  @param  appId 游戏在接入联运分配的appId
 *
 *	@param	appkey 游戏在接入联运时分配的appkey, 用于标记一个游戏
 *
 *  @param  isAccept 检查游戏版本更新升级，若检查更新失败（网络错误或因后台报错）是否继续游戏，建议传YES(即：继续游戏，可能跳过强制更新），可根据版本需要传
 *
 *	@param	orientation	 游戏的初始方向
 */
- (void) initializeWithAppId:(NSString *) appId appKey:(NSString *) appKey statisticsAPPid:(NSString *)statisticsAPPid isContinueWhenCheckUpdateFailed:(BOOL)isAccept;

/**
 *  @brief  platform 初始化之后，获取 appid
 */
@property (nonatomic, strong, readonly) NSString *XY_APP_ID;

/**
 * @brief platform 初始化之后，获取 appkey
 */
@property (nonatomic, strong, readonly) NSString *XY_APP_KEY;
/**
 * @brief platform 初始化之后，获取 appkey
 */
@property (nonatomic, strong, readonly) NSString *Statistics_APP_ID;


/**
 * @brief 初始化方向
 */
@property (nonatomic, assign, readonly) UIInterfaceOrientation XYInterfaceOrientation;

/**
 * @brief 是否 debug 模式
 */
@property (nonatomic, assign, readonly) BOOL isDebugModel;

/**
 *  @brief 设定初始时游戏的方向
 *      1、其中设置的方向需要在 app plist文件Supported interface orientations 中支持，否则会Assert
 *      2、UIInterfaceOrientation, 设置 UIInterfaceOrientationLandscapeLeft 或者 UIInterfaceOrientationLandscapeRight，平台页面仅支持横屏幕。
 *      3、设置 UIInterfaceOrientationPortrait ，平台仅支持Portrait方向
 *      4、若不设置，平台页面会根据设备方向以及plist中配置适配方向做自适应，
 *      5、可不设置
 */
- (void)XYSetScreenOrientation:(UIInterfaceOrientation)orientation;

/**
 *  @brief 设置调试模式
 *
 *  @param debug YES 为测试环境, NO为 正式环境
 */
- (void)XYSetDebugModel:(BOOL)debug;


/**
 * @brief 打印log到控制台
 */
- (void)XYSetShowSDKLog:(BOOL)isShow;


@property (nonatomic, assign, readonly) BOOL isShowAd;

@end


#pragma mark-- 用户部分，登录、注册、登出、token、uid 等

@interface XYPlatform (XYUserCenter)

/**
 *  @brief 检查用户登录状态，如不再登录状态，自动登录
 *
 *  @param iflag 默认传0
 *
 *  @result 预留，默认为0
 */
- (int) XYAutoLogin:(int) iflag;


/**
 *  @brief XYPlatform 登录界面入口, 进入登录 or 注册页面
 *
 *  @param lFlag 标识(按位标识)预留, 默认为0
 *
 *  @result 错误码, 预留, 默认为0
 */
- (int)XYUserLogin:(int)lFlag;



/**
 *  @brief XYPlatform 注册界面入口
 *
 *  @param rFlag 标识(按位标识)预留, 默认为0
 *
 *  @result 错误码, 预留, 默认为0
 */
- (int)XYUserRegister:(int)rFlag;


/*
 * @brief 判断玩家登录状态，异步回调
 */
- (void)XYIsLogined:(void (^)(BOOL isLogined)) bLogined;


/**
 * 创建一个随机用户
 */
- (int)XYCreateRandomAccount:(int) cFlag;


/**
 *  @brief XYPlatform 注销, 即退出登录
 *
 *  @param lFlag 标识(按位标识), 0: 表示注销但保存本地信息;   1:表示注销, 并清除自动登录
 *
 *  @result 错误码, 预留, 默认为0
 */
- (int)XYLogout:(int)lFlag;


/**
 *  @brief 获取本次登录的token, 登录或注册之后返回, 有效期为7天
 */
- (NSString*) XYToken;

/**
 *  @brief 获取登录的openuid, 用于标记一个用户
 */
- (NSString*)XYOpenUID;

/**
 *  @brief 当前登录用户名
 */
- (NSString *) XYLoginUserAccount;


/**
 * @brief 切换帐号
 */
- (void) XYSwitchAccount;

/**
 *  检查当前是否为游客登录状态
 *
 *  @return 当前是否为游客登录。
 */
- (BOOL) XYIsGuestLogined;


/**
 * @brief 游客账户注册。注册完毕，游客账户变为正式账户，UID不会变，游戏方可以在比如用户升级到一定级别或者其他需要时强制玩家注册
 *
 * @param rFlag
 *
 * @result 默认返回0  返回1表示当前不是游客账户或者没有登录，不弹出游客注册页面
 */

- (int) XYGuestRegister:(int)rFlag;


@end



#pragma mark-- 各种中心

@interface XYPlatform(Center)

/**
 * @brief 进入个人中心
 *
 * @param uFlag 预留标识 传0
 *
 */

- (void) XYEnterUserCenter:(int) uFlag;


/**
 * @brief 进入论坛BBS
 *
 * @param bFlag 预留标识 传0
 *
 */

- (void) XYEnterAppBBS:(int) bFlag;


@end


#pragma mark-- GameCenter

@interface XYPlatform (XYGameCenter)

@end
