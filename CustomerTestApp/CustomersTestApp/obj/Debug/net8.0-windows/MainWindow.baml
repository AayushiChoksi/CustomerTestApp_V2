   M S B A M L   `   `   `  ����K  GCustomersTestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:CustomersTestApp.ViewModelsCustomersTestApp.ViewModels  5   	   ?$clr-namespace:CustomersTestApp.ViewsCustomersTestApp.Views  5   	   !    CustomersTestApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   @
viewModels)clr-namespace:CustomersTestApp.ViewModels     5   	   6views$clr-namespace:CustomersTestApp.Views     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)    � DataContext 5      /   )CustomersTestApp.ViewModels.MainViewModel  5   
   .��5      �    BackgroundColor#���   6    � ColumnDefinitions 5   
   �� 5      $��3* �6   $	��200��6*   �� 5      $
��Auto �6   �� 5      $��5* �6   $	��400��6*   .+�5   
   -   'CustomersTestApp.Views.CustomerListView  $��0��6!   $��10q�61   �� 5   
   $��1��6   $��5��6'   
�������$��Stretch=�6C   /   )CustomersTestApp.Views.CustomerEditorView  5   
   $��2��6#   $��10q�63   �� 5   
    �� Orientation$ 
Horizontal=�6   $	��Top=�6.   $��Right=�6F   $��10q�6b   .+�5       �� $��Export��6   $	��100��6H   $��5q�6T   ��6&   �� *ExportCommand+6T   5"      5#      5$      