   M S B A M L   `   `   `  ����J  FCourseProjectDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:CourseProjectDB.WindowsCourseProjectDB.Windows  5   	   &     CourseProjectDB.Windows.DelFacts   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:CourseProjectDB.Windows     5   	     � Title$  DelFacts��5   	   $	��500��6   $	��400��6'   .��5	      �  � ColumnDefinitions 5
   
   �� 5      �� 5      $	��220 �6   �� 5       � RowDefinitions 5   
   �� 5      $��25 �6   �� 5      $��45 �6   �� 5      $��45 �6   �� 5      $��25 �6   �� 5      $��45 �6   �� 5      $��25 �6   �� 5      $��45 �6   �� 5      $��45 �6   �� 5      $��45 �6   �� 5      $��5 �6   �� 5      $��45 �6   �� 5      ��5!   
   �� 5"       �� ImageSource$ ../Images/AdminPanel.jpg��6   .+�5%   
   �� $%��Удаление записей��6   $��Center=�6+   $��1��6H   $��1��6X   $��14(�6e   $
��Bold"�6s   ���	#FFFDFDFD6�   �� 5&   
   $��выберите id��6   $��Center=�6&   $��1��6C   $��4��6S   $��12(�6`   $
��Bold"�6n   ���	#FFFDFDFD6�   �� 5'   
   $>��8Выберите поставщика/заказчика��6   $��Center=�68   $��1��6U   $��2��6e   $��12(�6r   $
��Bold"�6�   ���	#FFFDFDFD6�   �� 5(   
   -   5        $��Combobox��5(   ]   $��1��6   $��3��6#   6]   �� 5)   
   -   5         ��Name$ shipmentComboBox��5)      $��1��6-   $��5��6=   �� 5+   
   -   5        $��Удалить��5+      $��1��6#   $��8��63   $��14(�6@   6N   �� 5,   
   -   5        $��
Назад��5,      $��1��6!   $��10��61   $��14(�6?   6M   5-      51      