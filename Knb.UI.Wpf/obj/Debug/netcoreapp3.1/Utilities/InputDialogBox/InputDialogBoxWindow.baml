   M S B A M L   `   `   `  ����E  AKnb.UI.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullY1clr-namespace:Knb.UI.Wpf.Utilities.InputDialogBox#Knb.UI.Wpf.Utilities.InputDialogBox  5   	   >    8Knb.UI.Wpf.Utilities.InputDialogBox.InputDialogBoxWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Clocal1clr-namespace:Knb.UI.Wpf.Utilities.InputDialogBox     5   	   -   5          �Name$  window��5   	    � Title$ InputDialogBox��5   	    � 
ResizeMode$ 	CanResize=�5	   	   $	��480��5
   	   $	��240��5   	   $	��240��5   	   $	��480��5   	    � WindowStyle$ 
ToolWindow=�6   .��5      � $
��Auto��6   $
��Auto��6   .+�5   
   ��  �� Orientation$ Vertical=�6   $��Stretch=�6,   $	��Top=�6J   .+�5      �� -   5         ��Name$ TxtB_message��5      $��30��6.   $��
Text block��6:   �� 5      -   5         ��Name$ 	Txt_input��5      $��Type here...��6)   $��30��6=   $��2,0,2,0q�6I   5      �� 5   
   -   5         ��Name$ Btn_ok��5      $
��Okay��6!   $
��Left=�6E   $��Bottom=�6`   $��10,0,0,5q�6{   $	��100��6�   $��20��6�   �� 5   
   -   5        $ 
Btn_cancel��5      $��Cancel��6%   $��Right=�6O   $��Bottom=�6k   $��0,0,10,5q�6�   $	��100��6�   $��20��6�   5      5      