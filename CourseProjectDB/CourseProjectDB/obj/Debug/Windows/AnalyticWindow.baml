   M S B A M L   `   `   `  ����J  FCourseProjectDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:CourseProjectDB.WindowsCourseProjectDB.Windows  5   	   ,    &CourseProjectDB.Windows.AnalyticWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:CourseProjectDB.Windows     5   	     � Title$  AnalyticWindow��5   	   $	��800��6    $
��1200��6-   .��5      �  � ColumnDefinitions 5   
   �� 5      $��20 �6   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      $��20 �6    � RowDefinitions 5   
   �� 5      $��10 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��10 �6   �� 5      �� 5      $��10 �6   .+�5"   
   �� -   5         ��Name$ QueryTextBox��5"   C   $��1��6   $��1��6"   $��7��6/   6C   �� 5$   
   -   5        $��1��5$      $��2��6!   $%��Выполнить запрос��6.   6J   �� 5%   
   -   5        $��2��5%      $��2��6!   $��Все заказы��6.   6D   �� 5&   
   -   5        $��3��5&      $��2��6!   $��Все поставки��6.   6F   �� 5'   
   -   5        $��4��5'      $��2��6!   $1��+Сумма денег за поставки��6.   6Q   �� 5(   
   -   5        $��5��5(      $��2��6!   $-��'Сумма денег за заказы��6.   6O   �� 5)   
   -   5        $��6��5)      $��2��6!   $��
Назад��6.   6>   �� 5*   
   -   5        $��7��5*      $��2��6!   $)��#Записать результат��6.   6K   &   System.Windows.Controls.DataGrid  5-   
   -	   5          Name$ ResultDataGrid��5-   D   $��1��6   $��7��6#   $��4��67   6D   5.      5/      