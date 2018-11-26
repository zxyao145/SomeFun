using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Jil;
using MusicDownload.Business;
using MusicDownload.Logic;
using MusicDownload.Models;

namespace MusicDownload
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMusicSearcher _searcher;
        private IMusicParser _parser;
        private AbsDownloaderPool _downloaderPool;
        private int _maxPageNum = 1;
        private int _curPageNum = 1;
        private List<BasicMusicInfoModel> _songDataGridDbcontext;


        private readonly ConcurrentQueue<BasicMusicInfoModel> _downloadMusicQueue;

        /// <summary>
        /// 判断是否有网络
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="ReservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(int Description, int ReservedValue);
        public MainWindow()
        {
            int description = 0;
            if (!InternetGetConnectedState(description, 0))
            {
                MessageBox.Show("无网络，程序无法运行！");
                Environment.Exit(0);
            }

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _downloadMusicQueue = new ConcurrentQueue<BasicMusicInfoModel>();
            _downloaderPool = new QqDownloaderPool(_downloadMusicQueue);
            var req1 = new Requests();
            req1.OnError += RequestErrorCallBack;
            var downloader1 = new QqDownloader(req1);
            downloader1.OnBeforeDownload += BeforeDownload;
            downloader1.OnAfterDownload += AfterDownload;
            downloader1.OnDownloadError += DownloadError;

            var req2 = new Requests();
            req2.OnError += RequestErrorCallBack;
            var downloader2 = new QqDownloader(req2);
            downloader2.OnBeforeDownload += BeforeDownload;
            downloader2.OnAfterDownload += AfterDownload;
            downloader2.OnDownloadError += DownloadError;

            _downloaderPool.AddDownloader(downloader1);
            _downloaderPool.AddDownloader(downloader2);

            _downloaderPool.StartListen();

            InitializeComponent();
        }

        private void BtnDownload_OnClick(object sender, RoutedEventArgs e)
        {
            ((FrameworkElement) sender).IsEnabled = false;
            if (((FrameworkElement)sender).DataContext is BasicMusicInfoModel obj)
            {
                _downloadMusicQueue.Enqueue(obj);
            }
        }
        #region 触发搜索

        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var songName = TbxSongName.Text;
            _curPageNum = 1;
            RealSearch(songName, 1);
            BtnGo.IsEnabled = true;
            BtnPre.IsEnabled = true;
            BtnNext.IsEnabled = true;

        }

        /// <summary>
        /// 跳转按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGo_OnClick(object sender, RoutedEventArgs e)
        {
            var songName = TbxSongName.Text;
            _curPageNum = Convert.ToInt32(PageNum.Text);
            RealSearch(songName, _curPageNum);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            var songName = TbxSongName.Text;
            _curPageNum += 1;
            PageNum.Text = _curPageNum.ToString();
            RealSearch(songName, _curPageNum);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPre_OnClick(object sender, RoutedEventArgs e)
        {
            var songName = TbxSongName.Text;
            _curPageNum -= 1;
            PageNum.Text = _curPageNum.ToString();
            RealSearch(songName, _curPageNum);
        }

        #endregion


        #region 依赖对象的事件
        /// <summary>
        /// IRequests请求出错时的事件
        /// </summary>
        /// <param name="args"></param>
        private void RequestErrorCallBack(MusicDownload.Models.EventArgs.ErrorEventArgs args)
        {
            UpdateLabelStatus($"第{args.RetryIndex}次请求出错：{args.Exception.Message}");
        }

        #region downloader的事件
        private void BeforeDownload(string songInfo)
        {
            UpdateLabelStatus($"【{songInfo}】正在下载中...");
        }
        private void AfterDownload(string songInfo)
        {
            UpdateLabelStatus($"【{songInfo}】下载完毕！");
            var splitIndex = songInfo.LastIndexOf("_", StringComparison.Ordinal);
            var songName = songInfo.Substring(0, splitIndex);
            var index = _songDataGridDbcontext.FindIndex(a => a.SongName == songName);
            EnableBtn(index);
        }
        private void DownloadError(Exception e, string songInfo)
        {
            UpdateLabelStatus($"【{songInfo}】下载失败：{e.Message}");
            var splitIndex = songInfo.LastIndexOf("_", StringComparison.Ordinal);
            var songName = songInfo.Substring(0, splitIndex);
            var index = _songDataGridDbcontext.FindIndex(a => a.SongName == songName);
            EnableBtn(index);
        }
        #endregion

        #endregion

        #region 从其他线程回到主线程执行

        /// <summary>
        /// 在ui线程中更新ui
        /// </summary>
        /// <param name="text"></param>
        private void UpdateLabelStatus(string text)
        {
            void Action(string t)
            {
                this.LabelStatus.Content = t;
            }

            this.Dispatcher.BeginInvoke((Action<string>)Action, DispatcherPriority.Send, text);
        }

        /// <summary>
        /// 启用button
        /// </summary>
        /// <param name="rowIndex"></param>
        private void EnableBtn(int rowIndex)
        {
            void Action(int index)
            {
                var colNum = SongDataGrid.Columns.Count;
                var row = SongDataGrid.Items[index];
                var col = SongDataGrid.Columns[colNum - 1];
                var content = (ContentPresenter)col.GetCellContent(row);
                if (content != null)
                {
                    DataTemplate contentTemplate = content.ContentTemplate;
                    var findRes = contentTemplate.FindName("BtnDownload", content);
                    if (findRes is Button btn)
                    {
                        btn.IsEnabled = true;
                    }
                }
            }
            this.Dispatcher.BeginInvoke((Action<int>)Action, DispatcherPriority.Send, rowIndex);

        }

        #endregion


        #region 跳转页码输入框相关事件
        private void PageNum_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) 
                || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) 
                || e.Key == Key.Back || e.Key == Key.Delete
                || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;//可输入
            }
            else 
            {
                e.Handled = true;//不可输入
            }
        }

        private void PageNum_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PageNum.Text))
            {
                var pageNum = Convert.ToInt32(PageNum.Text);
                if (pageNum > _maxPageNum)
                {
                    PageNum.Text = _maxPageNum.ToString();
                }
            }
            else
            {
                PageNum.Text = "1";
            }
        }
        private void PageNum_OnGotFocus(object sender, RoutedEventArgs e)
        {
            PageNum.SelectionStart = PageNum.Text.Length;
        }
        #endregion



        /// <summary>
        /// 真正执行搜索的方法
        /// </summary>
        /// <param name="songName"></param>
        /// <param name="pageNum"></param>
        private async void RealSearch(string songName, int pageNum)
        {
            if (_searcher == null)
            {
                IRequests req = new Requests();
                req.OnError += RequestErrorCallBack;
                _searcher = new QqMusicMusicSearcher(req);
                _searcher.OnBeforeSearch += () => { UpdateLabelStatus("正在搜素中..."); };
                _searcher.OnAfterSearch += () => { UpdateLabelStatus("搜素完毕！"); };
                _searcher.OnSearchError += (exp) => { UpdateLabelStatus($"搜素出错：{exp.Message}"); };

                _parser = new QqMusicParser();
                _parser.OnBeforeParse += () => { UpdateLabelStatus("正在搜素中..."); };
                _parser.OnAfterParse += () => { UpdateLabelStatus("解析完毕！"); };
                _parser.OnParseError += (exp) => { UpdateLabelStatus($"解析出错：{exp.Message}"); };
            }

            var searchInfo = await _searcher.SearchAsync(songName, pageNum);
            var musicInfos = await _parser.ParseAsync(searchInfo);
            _maxPageNum = musicInfos.Item1 % 20 == 0 ? musicInfos.Item1 / 20 : (musicInfos.Item1 / 20 + 1);
            LabelMaxPageNum.Content = "/" + _maxPageNum.ToString();
            _songDataGridDbcontext = musicInfos.Item2;
            SongDataGrid.DataContext = _songDataGridDbcontext;
            BtnNext.IsEnabled = _curPageNum != _maxPageNum;
            BtnPre.IsEnabled = _curPageNum != 1;
        }


        private void TbxSongName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            TbxSongName.SelectionStart = TbxSongName.Text.Length;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_downloaderPool!=null)
            {
                try
                {
                    _downloaderPool.StopPool();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }
}
