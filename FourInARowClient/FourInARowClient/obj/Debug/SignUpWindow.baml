   M S B A M L   `   `   `  ����K  GFourInARowClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:FourInARowClientFourInARowClient  5   	   #    FourInARowClient.SignUpWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:FourInARowClient     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$ Sign up��5	   	   $	��300��6   $	��400��6&   .��5      � -   5        $��mainGrid��5      
������� � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��lbWelcom��5      $��
4 in a row��6    $��30(�65   $
��Bold"�6C   $��Italic$�6U   $��3��6h   $��Center=�6|   $��Center=�6�   �� 5   
   -   5        $��lbUser��5      $��1��6   $��0��6+   $��
User name:��6;   $��Center=�6P   $��Right=�6k   �� 5   
   -   5        $��tbUser��5      $��1��6    $��1��6-   $��2��6=   $��10,17,50,17q�6Q   �� 5   
   -   5        $��lbPass��5      $��2��6   $��0��6+   $��	Password:��6;   $��Center=�5      $��Right=�6+   G� 5   
   -   5        $��
tbPassword��5      $��2��6(   $��1��65   $��2��6E   $��10,17,50,17q�6Y   �� 5   
   -   5        $��lbRePass��5      $��3��6    $��0��6-   $��Re-Password:��6=   $��Center=�5      $��Right=�6+   G� 5    
   -   5        $��tbRePassword��5       $��3��6*   $��1��67   $��2��6G   $��10,17,50,17q�6[   �� 5!   
   -	   5        $��btnSigUp��5!      $��0��6!   $��3��61   $��4��6E   $��100,10q�6R   .��5#      �� $
��Wrap=�6   $��16(�60   $
��Bold"�6>   	Sign Up5%      5&      5(      5)      