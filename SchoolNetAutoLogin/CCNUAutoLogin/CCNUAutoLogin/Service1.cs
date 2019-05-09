using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Jil;

namespace CCNUAutoLogin
{
    public partial class Service1 : ServiceBase
    {
        private Timer _timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timeInterval = 10 * 60 * 1000;//每10分钟
            try
            {
                _timer = new Timer(new TimerCallback(state =>
                {
                    var isOnline = PingBaidu();
                    if (!isOnline)
                    {
                        Login();
                    }
                }), null, 0, timeInterval);
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"Timer 出错，{e.Message}");
            }

        }

        /// <summary>
        /// 获取用户名、密码、运营商
        /// </summary>
        /// <returns></returns>
        private LoginInfo GetLoginInfo()
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            var config = Path.Combine(appDir, "./CCNUAutoLogin.Conf");
            if (!File.Exists(config))
            {
                using (StreamWriter sw = new StreamWriter(config))
                {
                    sw.Write(JSON.Serialize(new LoginInfo
                    {
                        UserName = "用户名",
                        Password = "密码",
                        Type = "校园网"
                    },new Options(true)));
                }

                LogHelper.WriteError($"配置文件不存在，请按格式补充{config}内容");
                throw new IOException("配置文件不存在");
            }

            string jsonInfo;
            using (StreamReader sr = new StreamReader(config))
            {
                jsonInfo = sr.ReadToEnd();
            }

            LoginInfo loginInfo;
            try
            {
                loginInfo = JSON.Deserialize<LoginInfo>(jsonInfo);
            }
            catch (Exception e)
            {
                LogHelper.WriteError($"配置文件读取出错，请重新填写！{e.Message}");
                using (StreamWriter sw = new StreamWriter(config, false))
                {
                    sw.Write(JSON.Serialize(new LoginInfo
                    {
                        UserName = "用户名",
                        Password = "密码",
                        Type = "校园网"
                    }, new Options(true)));
                }
                throw;
            }
            return loginInfo;
        }

        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            var loginInfo = GetLoginInfo();
            var suffix = "0";
            if (loginInfo.Type.Contains("电信"))
            {
                suffix = "1";
            }
            else if (loginInfo.Type.Contains("移动"))
            {
                suffix = "2";
            }
            else if (loginInfo.Type.Contains("联通"))
            {
                suffix = "3";
            }


            string postString = $"DDDDD={loginInfo.UserName}&upass={loginInfo.Password}&suffix={suffix}&0MKKey=123";
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            var downloadUrl = "http://10.220.250.50/0.htm";

            WebClient webClient = new WebClient
            {
                Headers = new WebHeaderCollection
                {
                    {"Host", "10.220.250.50"},
                    {
                        "User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36"
                    },
                    {"Referer", "http://10.220.250.50/0.htm"}
                }
            };

            int tryTimes = 0;
            bool sucess = false;
            while (tryTimes < 5)
            {
                try
                {
                    byte[] responseData = webClient.UploadData(downloadUrl, "POST", postData);
                    //GB2312编码
                    string srcString = Encoding.GetEncoding("GB2312").GetString(responseData);
                    if (srcString.Contains("登录成功") || PingBaidu())
                    {
                        LogHelper.WriteInfo("自动登录成功！");
                        sucess = true;
                        break;
                    }
                    else
                    {
                        tryTimes++;
                    }
                }
                catch (Exception e)
                {
                    if (tryTimes == 4)
                    {
                        LogHelper.WriteError($"自动登录重试5次后失败，" + e.Message);
                    }
                    tryTimes++;
                }

            }
            webClient.Dispose();
            if (!sucess)
            {
                LogHelper.WriteInfo("自动登录失败");
            }
        }

        private bool PingBaidu()
        {
            try
            {
                return PingIpOrDomainName("www.baidu.com");
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// ping 百度，判断网络是否联通
        /// </summary>
        /// <param name="strIpOrDName"></param>
        /// <returns></returns>
        private bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void OnStop()
        {
            _timer.Change(0, -1);
            _timer.Dispose();
        }

    }
}
