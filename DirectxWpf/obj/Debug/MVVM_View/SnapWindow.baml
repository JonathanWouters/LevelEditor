   M S B A M L   `   `   `  ����  
DirectxWpf%    DirectxWpf.MVVM_View.SnapWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$
  Snap��5   	   $	��181��6   $	��190��6#   ���	#FF444444 � 
ResizeMode$ NoResize=�5   G    � ShowInTaskbar .   � DataContext 6	   ��    snapViewModel �� Source# [  .��5      � .+�5	   
   �� $��10q�6   .+�5
      ]� $��2q�6   .+�5      �� $��	Translate��6   $
��Left=�6,   $	��Top=�6G   $��80��6_   
�������6j   �� 5      $��Right=�6   $��50��61   $��23��6h   $��Center=�6t   ���	#FF505050���	#FF343434
������� �� SelectionBrush �	#FFA6A6A6$��Right=�6�   ��6<   �� *Gizmo.TranslationSnapValue+6�   5      ]� 5      $��2q�6   .+�5      �� $��Scale��6   $
��Left=�6(   $	��Top=�6C   $��80��6[   
�������6f   �� 5      $��Right=�6   $��50��61   $��23��6b   $��Center=�6n   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6$��Right=�6�   ��6<   �� *Gizmo.ScaleSnapValue+6�   5      ]� 5      $��2q�6   .+�5      �� $��Rotation��6   $
��Left=�6+   $	��Top=�6F   $��80��6^   
�������6j   �� 5      $��Right=�6   $��50��61   $��23��6e   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6$��Right=�6�   ��6<   �� *Gizmo.RotationSnapValue+6�   5      �� 5      $��50��6   $��24��6    $��Close��6,   $��Right=�6<   $��2q�6X   ���	#FF636363���	#FF9E9E9E
���������6c   �� *CloseCommand+��6�   ��  �� RelativeSource ��  �� AncestorType$ WindowL�6  5      5      5      