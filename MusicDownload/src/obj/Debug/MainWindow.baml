   M S B A M L   `   `   `  ����H  DMusicDownload, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:MusicDownloadMusicDownload  5   	   ;"clr-namespace:MusicDownload.ModelsMusicDownload.Models  5   	       MusicDownload.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:MusicDownload     5   	   5models"clr-namespace:MusicDownload.Models     5   	   -   5          � Title$  音乐下载助手��5	   	   $	��440��6   $	��700��6%   $	��700��61   $	��440��6@    � 	Resources 5
         
ZxyTextBox �� 
TargetType �� Property �� Value �� 
TargetType ��Name �Name ��Name  ContentCenterStyle  CenterGridHeaderStyle=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%n  &	        &	 c    &	 �    �� 5   
   " ��6#   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� $ border��6!   $
��Auto��61   $
��Auto��6>   $��1q�6L   ���	#FF7F9DB9._�5      � $
 grid��6#   ���#FFFFFF.+�5   "   �� $ PART_ContentHost��6/   $��Center=�6I   $
��Left=�6d   5      5      5      5      5      �� 5   
   " ��6+   .�5      ��  � ��$ Center=�64   �� 5       � ��$ Center=�62   5      �� 5   
   "  6.   .�5      ��  � ��$ Center=�6;   �� 5       � ��$ Center=�69   6J   5      5       .��5!      � .+�5"   
   � $��2,10,2,0q�6   $��40��6!   $	��Top=�6-   .+�5#      �� -   5        $��TbxSongName��5#      #��[  6)   $��
10,0,111,0q�6o   	 �� TextWrapping$
	 Wrap=�6�   $��	周杰伦��6�   $��16(�6�   $��34��6�   $	��Top=�6�   �� 5$      -   5        $��搜索��5$      
 �� 	IsDefault
 . $��0,0,10,0q�63   $	��Top=�6E   $��34��6]   $��16(�6i   $��Right=�6�   $��79��6�   5%      � 5&   
   $��	2,50,2,45q�6   .+�5'      &   System.Windows.Controls.DataGrid  -   5        $��SongDataGrid��5'   $   $
��Auto��6   $
��10,0q�68      AutoGenerateColumns .     CanUserAddRows .     
IsReadOnly . $
��Auto=�5*   )   $��Hidden=�6Y      HeadersVisibility$	 All=�6�      ColumnHeaderStyle# [ 6�   ��5(      ��    Columns 5+      0  *System.Windows.Controls.DataGridTextColumn  5,      1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6)   ,  &System.Windows.Controls.DataGridColumn   Header$ 歌曲��6\      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6h      Binding 6s   �� *
SongName+6�     5-      # [ 6)   $ 歌手��6\   $ 1* 6h    6s   �� *
SingerName+6�     5.      # [ 6*   $ 专辑��6]   $ 3* 6i    6t   �� *Album+6�     5/      # [ 6*   $ 时长��6]   $ 1* 6i    6t   �� *Time+6�   4  .System.Windows.Controls.DataGridTemplateColumn  50      $ 下载��6-   $ 1* 69      CellTemplate 51      �� 52       �� DataType$  models:BasicMusicInfoModel��6+   .R�53   "   ��  ��Name$ BtnDownload��6)   -   5        $��8q�53   ?   
�������
����� ���6�   ��  �� Path$ SongId�.��6�   下载6�   54      56      58      59      � 5:   
   $��	2,0,1.6,6q�6   $��40��6"   $��Bottom=�6.   .+�5;      �� -   5        $��BtnNext��5;      {�.  $��	下一页��6N   $��0,8,10,0q�6\   $	��Top=�6n   $��14(�6�   $��Right=�6�   $��60��6�   $��22��6�   �� 5<      -   5        $��BtnPre��5<      {�.  $��	上一页��6L   $��0,8,79,0q�6Z   $	��Top=�6l   $��14(�6�   $��Right=�6�   $��60��6�   $��22��6�   �� 5=      -   5        $��BtnGo��5=      $��跳转��6#   {�.  $��	0,8,165,0q�6X   $	��Top=�6k   $��14(�6�   $��Right=�6�   $��60��6�   $��22��6�   �� 5>      -	   5        $��PageNum��5>      #��[  6C   $��	0,8,271,0q�6g   $
	 Wrap=�6z   $��1��6�   $��14(�6�   $��Right=�6�   $��41��6�   $��22��6	  $	��Top=�6  �� 5?      -
   5        $��LabelStatus��5?      $�� ��6'   $��10,8,338.6,0q�62   $��10(�6H   $��22��6V   $	��Top=�6b   �� 5@      -   5        $��LabelMaxPageNum��5@      $��/1��6+   $��14(�68   $��	0,6,226,0q�6F   $	��Top=�6Y   $��30��6q   $��Right=�6}   $��44��6�   5A      5C      5D      