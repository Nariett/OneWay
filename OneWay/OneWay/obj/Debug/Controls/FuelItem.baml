   M S B A M L   `   `   `  ����A  =OneWay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:OneWay.ControlsOneWay.Controls  5      U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5          OneWay.Controls.FuelItem   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:OneWay.Controls     5      Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      -   5        $
��this��5      $
��auto��6+   .��5	      �� -   5        $	��180��5	      $	��280��6   $��2q�6&      TextSecundaryColor#���   6:   $��10q�6m     �� CornerRadius$  10��6y   
������ ._�5
   
   �  � ColumnDefinitions 5      �� 5      $��* �6#    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6    .+�5      �� $��0��6^   $��25(�6k   #���   6z   $��15,15,105,27q�6�   ��6   �� *OctanNumber+ �� ElementName$
 this�� �� FallbackValue$ АИ-95��6�   �� 5      $��0��6\   $��20(�6i   #���   6x   $��
15,38,10,5q�6�   ��6   �� *
FuelType+$
 this��$ Бензин��6�   �� 5       �� TextAlignment$ Right=�6]   $��0��6s   $��25(�6�   #���   6�   $��140,15,10,27q�6�   ��6   �� *
PriceRub+$
 this��$ 
3.25руб��6�   #  LiveCharts.Wpf.CartesianChart  5      -   5        $��	liveChart��5   !   $��10,10,10,20q�62   $��1��6G   &   LiveCharts.Wpf.Charts.Base.Chart   Zoom$
 None=�6T      AxisX 5        LiveCharts.Wpf.Axis     
ShowLabels .  	   Labels	 5      �� 5      
 �� Path$
 	TripDates�6&      	Separator 5        LiveCharts.Wpf.Separator  5         StrokeThickness$ 0Q�6,   6B   5         AxisY 5!         .     MinValue$ 0Q�62    5"        5#      $ 0Q�6,   6B   5%      5'      5(      5)      5*      