   M S B A M L   `   `   `  ����  CustomersTestApp/    )CustomersTestApp.Views.CustomerEditorView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � RowDefinitions  5	   
   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0,0,0,10q�6"   .+�5      �� $	��Id:��6   �� 5       �� 
IsReadOnly$
 True��6H   ��6   �� *EditableCustomer.Id+ �� Mode$ OneWay=�6H   5      �� 5   
   $��1��6   $��0,0,0,10q�6"   .+�5      �� $��Name:��6   �� 5      ��6   �� *EditableCustomer.Name+ �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   $��2��6   $��0,0,0,10q�6"   .+�5      �� $��Email:��6   �� 5      ��6   �� *EditableCustomer.Email+$ PropertyChanged=�5      �� 5    
   $��3��6   $��0,0,0,10q�6"   .+�5!      �� $��	Discount:��6   �� 5"      ��6   �� *EditableCustomer.Discount+$ PropertyChanged=�5#      �� 5%   
   $��4��6   $��0,10,0,0q�6"   $��Center=�64   .+�5&      �� $
��Save��6   ��6$   �� *SaveCommand+{�6D   �� *EditableCustomer.CanSave+5'      5(      5)      