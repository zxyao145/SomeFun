   M S B A M L   `   `   `  ���� H  DMusicDownload, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:MusicDownloadMusicDownload  ;"clr-namespace:MusicDownload.ModelsMusicDownload.Models      MusicDownload.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:MusicDownload     5models"clr-namespace:MusicDownload.Models     -     � Title$  音乐下载助手��$	��440��$	��700��$	��700��$	��440�� � 	Resources    
ZxyTextBox �� 
TargetType �� Property �� Value �� 
TargetType ��Name �Name ��Name  ContentCenterStyle  CenterGridHeaderStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%e  &	        &	 �     &	     �� " ��.���  � �� �� " ��.R��� $ border��$
��Auto��$
��Auto��$��1q����	#FF7F9DB9._�� $
 grid�����#FFFFFF.+��� $ PART_ContentHost��$��Center=�$
��Left=��� " ��.���  � ��$ Center=���  � ��$ Center=��� "  .���  � ��$ Center=���  � ��$ Center=�.��� .+�� $��2,10,2,0q�$��40��$	��Top=�.+��� -   $��TbxSongName��#��[  $��
10,0,111,0q�	 �� TextWrapping$
	 Wrap=�$��	周杰伦��$��16(�$��34��$	��Top=��� -   $��搜索��
 �� 	IsDefault
 . $��0,0,10,0q�$	��Top=�$��34��$��16(�$��Right=�$��79��� $��	2,50,2,45q�.+�&   System.Windows.Controls.DataGrid  -   $��SongDataGrid��$
��Auto��$
��10,0q�   AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly . $
��Auto=�$��Hidden=�   HeadersVisibility$	 All=�   ColumnHeaderStyle# [ ����    Columns 0  *System.Windows.Controls.DataGridTextColumn  1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ ,  &System.Windows.Controls.DataGridColumn   Header$ 歌曲��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2*    Binding �� *
SongName+  # [ $ 歌手��$ 1*  �� *
SingerName+  # [ $ 专辑��$ 3*  �� *Album+  # [ $ 时长��$ 1*  �� *Time+4  .System.Windows.Controls.DataGridTemplateColumn  $ 下载��$ 1*    CellTemplate ��  �� DataType$  models:BasicMusicInfoModel��.R��� -   $��8q�
�������
����� �����  �� Path$ SongId�.��下载� $��	2,0,1.6,6q�$��40��$��Bottom=�.+��� -   $��BtnNext��{�.  $��	下一页��$��0,8,10,0q�$	��Top=�$��14(�$��Right=�$��60��$��22���� -   $��BtnPre��{�.  $��	上一页��$��0,8,79,0q�$	��Top=�$��14(�$��Right=�$��60��$��22���� -   $��BtnGo��$��跳转��{�.  $��	0,8,165,0q�$	��Top=�$��14(�$��Right=�$��60��$��22���� -	   $��PageNum��#��[  $��	0,8,271,0q�$
	 Wrap=�$��1��$��14(�$��Right=�$��41��$��22��$	��Top=��� -
   $��LabelStatus��$�� ��$��10,8,338.6,0q�$��10(�$��22��$	��Top=��� -   $��LabelMaxPageNum��$��/1��$��14(�$��	0,6,226,0q�$	��Top=�$��30��$��Right=�$��44��