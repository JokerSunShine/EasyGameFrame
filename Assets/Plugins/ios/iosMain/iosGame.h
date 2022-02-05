extern "C"
{
  //初始化SDK
  void TL_initSdk();

  //登录
  void LY_Login();

  //切换账号
  void TL_switchAccountLogin();

  //退出
  void TL_logout();

  //支付
  void LY_Py(const char *data);

  //得到包名
  char *csGetPackageName();

  //数据上报
  void csSubmitGameData(const char *info);

  //得到Xcode版本号
  char *csGetXcodeVersion();

  //得到UDID
  char *csGetUDID();
  
  //得到手机厂商
  char *csGetDeviceBrand();
  
  //得到手机型号
  char *csGetSystemModel();

  //获取手机电量
  float getBattleryLevel();

}