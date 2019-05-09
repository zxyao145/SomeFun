# SchoolNetAutoLogin
ccnu 华中师范大学校园网自动登录服务，基于.NET Framework 4.6.1。

# 下载
编译后的文件存放在为Release文件夹中：
+ CCNUAutoLoginRelease：源程序编译文件，请配合InstallUtil.exe使用
+ CCNUAutoLogin.msi：程序安装包
+ CCNUAutoLogin.Conf：程序配置文件模板

# 使用教程
1. 安装
+ 使用“安装工具”下的InstallUtil.exe
将installutil.exe复制到CCNUAutoLogin.exe所在的文件夹中，以管理员身份启动cmd，定位到CCNUAutoLogin.exe所在的文件夹，输入以下命令

`cmd
installutil.exe CCNUAutoLogin.exe
sc config CCNUAutoLogin start= auto
`
+ 使用安装包
使用msi安装包“CCNUAutoLogin.msi”。

2. 配置
使用记事本编辑配置文件“CCNUAutoLogin.Conf”，其中type为校园网类型，包括值为校园网、电信、移动、联通；UserName为用户名；Password为密码。保存编辑后复制到安装目录下，注意不要改变文件名。

3. 重启
按住“windows+r”，打开运行，输入“services.msc”，打开服务窗口，找到“CCNUAutoLogin”服务，右键启动/重启即可。

# 注意
如果安装目录下不存在配置文件“CCNUAutoLogin.Conf”，将会创建一个配置文件模板。


