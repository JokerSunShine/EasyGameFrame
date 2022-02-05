extern "C"
{
  void YvChatSDKInit(char *appId, bool isTest, char *observer);

  void YvChatSDKInitWithenvironment(int environment, char *appId, char *observer);

  void loginWithSeq(char *seq);

  void loginBindingWithTT(char *tt, char *seq);

  void logout();

  void releaseResource();

  void chatMic(bool onOff, char *expand);

  void ChatMicWithTimeLimit(int timeLimit, char *expand);

  void setPausePlayRealAudio(bool isPause);

  bool isPausePlayRealAudio();

  void micModeSettingReq(int modeType);

  void YvSetLogLevel(int logLevel);

  void sendTextMessage(char *text, char *expand);

  void sendVoiceMessage(char *filePath, int voiceDuration, char *expand);

  void sendVoiceMessageWithVoiceUrl(char *voiceUrl, int voiceDuration, char *expand);

  void sendRichMessage(char *text, char *filePath, int voiceDuration, char *expand);

  void setIsAutoPlayVoiceMessage(bool isAutoPlayVoiceMessage);

  bool isAutoPlayVoiceMessage();

  void uploadVoiceFile(char *voiceFilePath, int fileRetainTimeType, char *expand);

  void httpVoiceRecognizeReqAndUploadVoiceFile(int recognizeLanguage, int outputTextLanguageType, char *voivoiceFilePath, int voiceDuration, int fileRetainTimeType, char *expand);

  void speechDiscernByUrl(int recognizeLanguage, int outputTextLanguageType, char *UrlFilePath, char *expand);

  void downloadVoiceFileToCacheWhenWifi(char *voiceUrl);

  void queryHistoryMsgReqWithPageIndex(int pageIndex, int pageSize);

  void ChatGetTroopsListReq();

  void audioTools_Init(char *observer);

  //		/*!
  //     	@method
  //    	@brief 开始语音录制
  //     	@param minMillseconds 识别录音最短时间(录音少于该时间不做处理);
  //     	@param maxMillseconds 录音最长时间(超过该时间会自动停止录制);
  //     	*/

  void audioTools_startRecord(int minMillseconds, int maxMillseconds);

  //		/*!
  //    	 @method
  //     	@brief 停止语音录制
  //     	*/
  void audioTools_stopRecord();

  //		/*!
  //     	@method
  //     	@brief 查询是否正在录制
  //     	*/
  bool audioTools_isRecording();

  //		/*!
  //     	@method
  //     	@brief 播放语音文件
  //     	@param 语音文件绝对路径
  //     	*/
  int audioTools_playAudio(char *filePath);

  //		/*!
  //     	@method
  //     	@brief 在线播放语音
  //     	@param 语音文件下载url
  //     	*/
  int audioTools_playOnlineAudio(char *fileUrl);

  //		/*!
  //     	@method
  //     	@brief 主动停止语音播放
  //     	*/

  void audioTools_stopPlayAudio();

  //		/*!
  //     	@method
  //     	@brief 查询当前是否正在播放
  //     	*/

  bool audioTools_isPlaying();

  //		/*!
  //     	@method
  //    	 @brief 删除文件
  //
  //     	@param filePath -文件路径
  //     	*/

  bool audioTools_deleteFile(char *filePath);

  //		/*!
  //     	@method
  //    	 @brief 是否开启音量回调
  //
  //     	@param enable -true 开启  false 关闭
  //     	*/

  bool audioTools_setMeteringEnabled(bool filePath);

  //		[DllImport("__Internal");]
  //		  char* createRecordAudioFilePath ();

  void synthesizeSentence(char *text, char *per);
}