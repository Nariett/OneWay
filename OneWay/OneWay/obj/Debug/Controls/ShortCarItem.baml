   M S B A M L   `   `   `  ����A  =OneWay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OneWay.ControlsOneWay.Controls  5      "    OneWay.Controls.ShortCarItem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      g cMahApps.Metro.IconPacks.Fontisto, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137af bMahApps.Metro.IconPacks.Unicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137aj fMahApps.Metro.IconPacks.FontAwesome, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Octicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ae	 aMahApps.Metro.IconPacks.Modern, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ai
 eMahApps.Metro.IconPacks.PicolIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ai eMahApps.Metro.IconPacks.RPGAwesome, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137al hMahApps.Metro.IconPacks.PixelartIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ai eMahApps.Metro.IconPacks.RadixIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.EvaIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Typicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a^ ZMahApps.Metro.IconPacks, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ah dMahApps.Metro.IconPacks.Zondicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ak gMahApps.Metro.IconPacks.FeatherIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Ionicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Codicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.JamIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ak gMahApps.Metro.IconPacks.WeatherIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137am iMahApps.Metro.IconPacks.MaterialDesign, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137aj fMahApps.Metro.IconPacks.VaadinIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137al hMahApps.Metro.IconPacks.MaterialLight, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.BoxIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ah dMahApps.Metro.IconPacks.RemixIcon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ah dMahApps.Metro.IconPacks.FileIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137af bMahApps.Metro.IconPacks.Microns, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137am iMahApps.Metro.IconPacks.BootstrapIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ah dMahApps.Metro.IconPacks.Fontaudio, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137aj  fMahApps.Metro.IconPacks.SimpleIcons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag! cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ac" _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137aj# fMahApps.Metro.IconPacks.ForkAwesome, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ah$ dMahApps.Metro.IconPacks.Coolicons, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ae% aMahApps.Metro.IconPacks.Entypo, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a}	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks!     	 
                        ! " # $ % 5      ilocalclr-namespace:OneWay.Controls!     	 
                        ! " # $ % 5      -   5        $
��this��5      .��5	      �� $
��auto��6   $
��auto��6   
������ $��2q�6A      TextSecundaryColor#���   6U   $��10q�6�     �� CornerRadius$  10��6�   ._�5
   
   �  � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6    .+�5      ��  �� Orientation$ 
Horizontal=�6   $��0��62   $
��Left=�6?   $��10 0 0 0q�6Z   .+�5      �� $��23(�6\   #���   6j   $��10q�6�   ��6   �� *	CarName+ �� ElementName$
 this�� �� FallbackValue$	 BMW��6�   5      � 5      $��1��6   $��0 0 0 10q�6     � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $��* �6'   .+�5      � $��0��6    5      �� 5      $��60 �6+   �� 5      $��* �6+    5      �� 5      $��* �6(   �� 5      $��* �6(   .+�5!      . ! (MahApps.Metro.IconPacks.PackIconMaterial  -   5          Name$ Engine��5!   1   $��0��6B   $��0��6O      Kind$ Engine=�6_   $��20��6m   $��20��6y   #���   6�   $��Center=�6�   $��Center=�6�   �� 5"      $��0��6    $��1��6-   $��15(�6~    �� TextAlignment$ Center=�6�   #���   6�   $��Center=�6�   $��Center=�6�   ��6=   �� *Volume+$
 this��$ 2.4 л��6�   �� 5$      $��0��6   $��0��6+   $
��auto��6;   $
��auto��6H   
������ 	 � ToolTip	 5%      _� 5&      
������ $��0q�6?   
 _� 	Placement$
 Bottom=�6S    _� HorizontalOffset$ 90��6f   .��5'   "   �� $��30��6)   $
��auto��66   $  5��6C     PrimaryBlueColor#���  6U   ._�5(   &   �� $<��6Объем двигателя и тип топлива��60   
�������$��14(�6h   $��Regular"�6v   $��Center=�6�   $��Center=�6�   $��	10 5 10 5q�6�   5)   #   5*      5,        5.      $��1��61   $��0��6>   $ CarShiftPattern=�6N   $��20��6e   $��20��6q   #���   6|   $��Center=�6�   $��Center=�6�   �� 5/      $��1��6    $��1��6-   $��15(�6�   $ Center=�6�   #���   6�   $��Center=�6�   $��Center=�6�   ��6=   �� *Transmission+$
 this��$ Механика��6�   �� 51      $��1��6   $��0��6+   $
��auto��6;   $
��auto��6H   
������ 	 52      _� 53      
������ $��0q�6?   $
 Bottom=�6S   $ 40��6f   .��54   "   �� $��30��6)   $
��auto��66   $  5��6C   #���  6U   ._�55   &   �� $#��Коробка передач��60   
�������$��14(�6Z   $��Regular"�6h   $��Center=�6}   $��Center=�6�   $��	10 5 10 5q�6�   56   #   57      59      5:      � 5<      $��1��6    5=      �� 5>      $��60 �6+   �� 5?      $��* �6+    5A      �� 5B      $��* �6(   �� 5C      $��* �6(   .+�5F        $��0��61   $��0��6>   $	 Car=�6N   $��20��6Y   $��20��6e   #���   6p   $��Center=�6�   $��Center=�6�   �� 5G      $��0��6   $��0��6+   $
��auto��6;   $
��auto��6H   
������ 	 5H      _� 5I      
������ $��0q�6?   $
 Bottom=�6S   $ 20��6f   .��5J   "   �� $��30��6)   $
��auto��66   $  5��6C   #���  6U   ._�5K   &   �� $��Тип кузова��60   
�������$��14(�6U   $��Regular"�6c   $��Center=�6x   $��Center=�6�   $��	10 5 10 5q�6�   5L   #   5M      5O      �� 5P      $��0��6    $��1��6-   $��15(�6|   $ Center=�6�   #���   6�   $��Center=�6�   $��Center=�6�   ��6=   �� *Body+$
 this��$ 
Седан��6�     5R      $��1��61   $��0��6>   $ CarSeat=�6N   $��20��6]   $��20��6i   #���   6t   $��Center=�6�   $��Center=�6�   �� 5S      $��1��6   $��0��6+   $
��auto��6;   $
��auto��6H   
������ 	 5T      _� 5U      
������ $��0q�6?   $
 Bottom=�6S   $ 70��6f   .��5V   "   �� $��30��6)   $
��auto��66   $  5��6C   #���  6U   ._�5W   &   �� $1��+Число мест в автомобиле��60   
�������$��14(�6b   $��Regular"�6p   $��Center=�6�   $��Center=�6�   $��	10 5 10 5q�6�   5X   #   5Y      5[      �� 5\      $��1��6    $��1��6-   $��15(�6x   $ Center=�6�   #���   6�   $��Center=�6�   $��Center=�6�   ��6=   �� *Seat+$
 this��$ 6��6�   5]      5^      5_      5`      5a      