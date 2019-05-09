using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CCNUAutoLogin;
using Jil;

namespace CCNUAutoLoginTest
{
    public class Services
    {
        private Timer _timer;

        public void OnStart()
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
            var curDir = AppDomain.CurrentDomain.BaseDirectory;
            var config = Path.Combine(curDir, "./CCNUAutoLogin.Conf");
            //var config = @"C:/CCNUAutoLogin.Conf";
            if (!File.Exists(config))
            {
                using (StreamWriter sw = new StreamWriter(config))
                {
                    sw.Write(JSON.Serialize(new LoginInfo
                    {
                        UserName = "用户名",
                        Password = "密码",
                        Type = "校园网"
                    }, new Options(true)));
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
                    {"Connection", "keep-alive"},
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
                    byte[] responseData = webClient.UploadData(downloadUrl, "POST", postData);//得到返回字符流
                    string srcString = Encoding.GetEncoding("GB2312").GetString(responseData);//解码
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
                    if (tryTimes == 5)
                    {
                        LogHelper.WriteError($"自动登录5次出错，" + e.Message);
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

    }
}
