   M S B A M L   `   `   `  ����K  GFourInARowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:FourInARowClientFourInARowClient  5   	   !    FourInARowClient.GameWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:FourInARowClient     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � Title$ Connect Four��5
   	   $	��700��6   $	��625��6+   .��5      � 
������� � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��10* �6   �� 5      $
��0.5* �6   .+�5   
   �� .+�5      �� -   5        $��lbGameTitle��5      $��20(�6'   $	��Vs.��65   $��Center=�6C   $��Center=�6`   �� 5      -   5        $
��lbID��5      $��20(�6    $�� ��6.   $��Center=�69   $��Center=�6V   5      �� 5   
   -   5        $��myCanvas��5      $��0,77,0.4,0.2q�6@   $��2��6V   ��5      �� 5       �� ImageSource$ board_image.jpg��6   5      � 5   
   $��2��6   .+�5      �� $��5,0,0,0q�6   .��6%   Number of Discs:67   �� 5      -   5        $��
tbNumDiscs��5   )   $��5,5,0,0q�6   6)   5      5      5      