   M S B A M L   `   `   `  ����  CustomersTestApp-    'CustomersTestApp.Views.CustomerListView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �    BackgroundColor#���   6     � RowDefinitions  5   
   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��10q�6"   .+�5      �� $��Filter Customers��6     GroupBoxBackgroundColor#���  61   .��5      �� .+�5      �� $&�� Type to filter by name or email:��6    �� 5      ��6   �� *
FilterText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      $��5q�6s   ��6   �� *FilterOptions+ �� SelectedItem 6E   �� *SelectedFilterOption+6s   5      5      5      �� 5   
   $��1��6   $��10q�6"   .+�5      �� $��Add New Customer��6   #���  61   .��5      �� .+�5      �� $
��Name��6    �� 5       � ToolTip$ Enter customer name��6d   ��6   �� *NewCustomerName+$ PropertyChanged=�6d   �� 5      
���  ����6    �� *	NameError+6;   �� 5      $��Email��6    �� 5       $ Enter customer email��6e   ��6   �� *NewCustomerEmail+$ PropertyChanged=�6e   �� 5!      
���  ����6    �� *
EmailError+6<   �� 5#      $��Discount��6    �� 5$      $ Enter discount value��6h   ��6   �� *NewCustomerDiscount+$ PropertyChanged=�6h   �� 5%      
���  ����6    �� *DiscountError+6?   �� 5'      $��Add Customer��6   ��64   �� *AddCustomerCommand+{�6[   �� *CanAddCustomer+�� 5(      $��Test Streaming��6   $	��130��6_   $��10q�6k   ��66   �� *TestStreamingCommand+6k   5)      5*      5+      �� 5-   
   $��2��6   $��0,10,0,0q�6k   ��6   �� *	Customers+ 6A   �� *SelectedCustomer+��5.      �� 5/      .R�50      ��   ListBoxItemBackgroundColor#���  6     ListBoxItemBorderColor#���  6W   $��1q�6�    �� Padding$ 5q�6�   $��5q�6�   ._�51      �� .+�52      ��   	TextColor#���  6>   ��6(   �� *Name+6>   �� 53      #���  6?   ��6(   �� *Email+6?   �� 54      #���  6W   ��6(   �� *
Discount+ �� StringFormat$
 {0}%��6W   �� 55      $��Remove��6%   $
��Left=�6�     ButtonBackgroundColor#���  6�     ButtonForegroundColor#���  64  ��66   �� *#!DataContext.RemoveCustomerCommand+ �� RelativeSource ��  �� AncestorType UserControl��6�   �� {�6�   �� *
Can_Remove+64  56      57      58      5:      5<      5=      