   M S B A M L   `   `   `  ����E  AKnb.UI.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Knb.UI.Wpf.PagesKnb.UI.Wpf.Pages  5      $    Knb.UI.Wpf.Pages.SimulatorPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Knb.UI.Wpf.Pages     5      -   5          N� Title$  SimulatorPage��5	      .��5      � $��Stretch=�6   $��Stretch=�6)   .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      �� $ 
Horizontal=�6   .+�5      �� $ Vertical=�6   .+�5      �� $
��10 5q�6!   $
��Left=�6/   $ Vertical=�6J   $	��Top=�6a   .+�5      �� -   5         ��Name$ lbl_noOfTimes��5       $��Number of times:��5      �� 5      -   5         ��Name$ cmb_noOfTimes��5   #   $
��auto��5      $	��255��5      5      �� 5      $
��10,5q�6!   $
��Left=�6/   $ Vertical=�6J   $	��Top=�6a   .+�5      �� -   5        $ lbl_noOfPlayers��5       $��Number of players:��5      �� 5      -   5        $ cmb_noOfPlayers��5   #   $
��auto��5      $	��255��5       5"      �� 5#      $
��10,5q�6!   $
��Left=�6/   $ Vertical=�6J   $	��Top=�6a   .+�5$      �� -   5        $ lbl_noOfCardPacks��5$       $��Number of CardPacks:��5%      �� 5'      -   5        $ cmb_noOfCardPacks��5'   #   $
��auto��5(      $	��255��5)      5+      �� 5,      $
��10,5q�6!   $
��Left=�6/   $ Vertical=�6J   $	��Top=�6a   .+�5-      �� -   5        $ lbl_noOfTimesPerPeriod��5-       $!��Number of times per period:��5.      �� 50      -	   5        $ cmb_noOfTimesPerPeriod��50   #   $
��auto��51      $	��255��52      54      55      �� 56      $ Vertical=�6   $
{�True��64   .+�57      �� $��	100,5,0,0q�6!   $
��Left=�64   $ Vertical=�6O   $	��Top=�6f   .+�58      �� -
   5        $ lbl_dataFileName��58       $��Data file name:��59       $	��200��5:       $
��Left=�6,   �� 5<      $ 
Horizontal=�6%   .+�5=      �� -   5        $ cmb_dataFileName��5=   '   $
��auto��5>   '   $	��200��5?   '   �� 5A      -   5         ��Name$ Btn_AddDataFile��5A   %   $��Add..��6>   $
��auto��6N   $��Stretch=�6\   $��Stretch=�6z   $��5,0,0,0q�6�   $��50��6�   $��Center=�6�   $��Stretch=�6�   6�   5B      5D      �� 5E      $��	100,5,0,0q�6!   $
��Left=�64   $ Vertical=�6O   $	��Top=�6f   .+�5F      �� -   5        $ lbl_modelFileName��5F       $��Ml model file name:��5G       $
��Left=�5H       �� 5I      -   5        $ cmb_modelFileName��5I   #   $
��auto��5J   '   $	��255��5K   #   $��Stretch=�5L   '   $��Stretch=�6E   5M      5N      5O      �� 5P      $��10,30q�6   $ Vertical=�6(   .+�5Q      �� -   5        $ Lbl_gameDataPb��5Q      $�� ��60   � 5R      -   5         �Name$ Pb_gameData��5R       � Value$ 25Q�63   $��20��6>   5S      5T      �� 5U   
   -   5        $ Btn_startSimulator��5U      $��Start��5V      $��Right=�5W      $��Bottom=�5X      $��16(�5Y      $��30��5Z      $	��120��5[      $��	0,0,10,10q�5\      6$   5]      5^      