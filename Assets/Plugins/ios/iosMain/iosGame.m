    //初始化sdk
  void TL_initSdk(){}

  //登录
  void LY_Login(){

       //SDK处理完成后调用OnSDKLoginSuc(string jsonData)方法 传入参数 json数据
       UnitySendMessage("IOSSDKCallback", "OnSDKLoginSuc", "---登录测试----");
  }

  //切换账号
  void TL_switchAccountLogin(){}

  //退出
  void TL_logout(){

        //SDK退出后调用OnSDKLogoutSuc(string json)方法 传入参数 json数据
       UnitySendMessage("IOSSDKCallback", "OnSDKLogoutSuc", "---退出测试----"); 
  }

  //支付 
  void LY_Py(const char *data){

  }
  
//二次校验成功
void cs_SecondVerifySuccess(const char *text){

}

  //得到包名
  char *csGetPackageName(){
      return strdup([@"test" UTF8String]);
  }

    //数据上报
  void csSubmitGameData(const char *info){}

  //得到Xcode版本号
  char *csGetXcodeVersion(){
      return strdup([@"test" UTF8String]);
  }

  //得到UDID
  char *csGetUDID(){
      return strdup([@"123465" UTF8String]);
  }

  //得到手机厂商
  char *csGetDeviceBrand(){
      return strdup([@"test" UTF8String]);
  }
  
  //得到手机型号
  char *csGetSystemModel(){
      return strdup([@"iPhone7,2" UTF8String]);
  }

  //得到手机电量
  float getBattleryLevel(){
　　　　[[UIDevice currentDevice] setBatteryMonitoringEnabled:YES];
　　　　return [[UIDevice currentDevice] batteryLevel];
　}
