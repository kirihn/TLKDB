   M S B A M L   `   `   `  ����J  FCourseProjectDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CourseProjectDBCourseProjectDB  5   	   !    CourseProjectDB.AddEmployee   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:CourseProjectDB     5   	     � 
ResizeMode$  NoResize=�5       � Title$ AddEmployee��5
   	   $	��500��6   $	��400��6*   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      $	��220 �6   �� 5       � RowDefinitions 5   
   �� 5      �� 5      $��25 �6   �� 5      �� 5      $��25 �6   �� 5      �� 5      $��25 �6   �� 5      �� 5      $��20 �6   �� 5      �� 5      $��5 �6   �� 5      �� 5      ��5"   
   �� 5#       �� ImageSource$ ../Images/AdminPanel.jpg��6   .+�5%   
   �� $+��%Добавление рабочего��6   $��Center=�6.   $��1��6K   $��0��6[   $��14(�6h   $
��Bold"�6v   ���	#FFFDFDFD6�   �� 5&   
   $��Должность��6   $��Center=�6$   $��1��6A   $��1��6Q   $��12(�6^   $
��Bold"�6l   ���	#FFFDFDFD6~   �� 5'   
   $��Имя��6   $��Center=�6   $��1��6;   $��3��6K   $��12(�6X   $
��Bold"�6f   ���	#FFFDFDFD6x   �� 5(   
   $��Контакты��6   $��Center=�6#   $��1��6@   $��5��6P   $��12(�6]   $
��Bold"�6k   ���	#FFFDFDFD6}   �� 5+   
   -   5        $��Добавить��5+      $��1��6$   $��8��64   $��14(�6A   6O   �� 5,   
   -   5        $��
Назад��5,      $��1��6!   $��10��61   $��14(�6?   6M   �� 5.   
   -   5        $��	Dolzhnost��5.   h    �� 	MaxLength$ 50��6   $��1��6!   $��2��61   $��14(�6>    �� TextWrapping$
 Wrap=�6L   $�� ��6`   6h   �� 5/   
   -   5        $��EmployeeName��5/   h   $ 50��6   $��1��6!   $��4��61   $��14(�6>   $
 Wrap=�6L   $�� ��6`   6h   �� 50   
   -   5        $��Contact��50   h   $ 50��6   $��1��6!   $��6��61   $��14(�6>   $
 Wrap=�6L   $�� ��6`   6h   53      54      