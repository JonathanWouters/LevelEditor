   M S B A M L   `   `   `  ����F  BDaeSharpWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null>.clr-namespace:DaeSharpWpf;assembly=DaeSharpWpfDaeSharpWpf  5   	   E ADirectxWpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:DirectxWpf.MVVM_ModelDirectxWpf.MVVM_Model 5   	   E'clr-namespace:DirectxWpf.MVVM_ViewModelDirectxWpf.MVVM_ViewModel 5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5   	   9!clr-namespace:DirectxWpf.BehaviorDirectxWpf.Behavior 5
   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      %   DirectxWpf.MVVM_View.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   FDaeSharpWpf.clr-namespace:DaeSharpWpf;assembly=DaeSharpWpf     5   	   5Model#clr-namespace:DirectxWpf.MVVM_Model     5   	   6VM'clr-namespace:DirectxWpf.MVVM_ViewModel     5   	   _wiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   f bGalaSoft.MvvmLight.Platform, Version=5.2.0.37226, Culture=neutral, PublicKeyToken=5f873c45e98af8a1*cmd http://www.galasoft.ch/mvvmlight  5	   	   )b!clr-namespace:DirectxWpf.Behavior  5
   	   0sys&clr-namespace:System;assembly=mscorlib  5        � Title$  Level Editor��5   	   $	��720��6   $
��1280��6+   $��0q�5      ���	#FF7A7A7A � DataContext 5   	   ��    mainViewModel �� Source# [   � 	Resources 5        ModelTemplate  ItemTemplate��%�   &	       &	 ?     �� 5   
   .R�5      �� ��6   �� *Name+5      �� 5   
   .R�5      �� ��6   �� *Title+5      5      .��5      � -   5        $��MainGrid��5      ���	#FF505050 � RowDefinitions 5   
   �� 5      $��24 �6   �� 5      $
��486* �6   �� 5      $
��180* �6   $	��200��6*    � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5       �� 5!      $	��200 �6   .+�5$   
   ]� $��3��6   .+�5%      o� $��0q�6   $	��Top=�6   $
��10,0q�63   ���	#FF4B4B4B$��0,0,0,1q�6Y   ���	#FF323232���	#FFE0E0E0.@�5&      m� $��_File��6   $��24��6*   $��40��66   $��0q�6A   ���	#FF2E2E2E���	#FFDADADA���	#FF4B4B4B.@�5'      m� $��_Open Project��6   ���	#FF444444 m� Command 6M   �� *OpenProjectCommand+m� 5(      $��_Load��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6k   �� *LoadXmlCommand+m� 5)      $��_Save��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6k   �� *SaveXmlCommand+m� 5*      $��
_Close Map��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6p   �� *CloseMapCommand+m� 5+      $��_Close Project��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6t   �� *CloseProjectCommand+5,      m� 5-      $��_GameObject��6A   $��24��6V   $��80��6b   $��0q�6m   ���	#FF2E2E2E���	#FFDADADA���	#FF4B4B4B{�6   �� *IsProjectLoaded+.@�5.      m� $��
_Add empty��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6p   �� *AddEmptyCommand+m� 5/      $��_Save as prefab��6E   ���	#FF444444{�6   �� *IsProjectLoaded+ 6u   �� *SavePrefabCommand+ m� CommandParameter 6�   �� *GoManager.SelectedObject+50      51      52      �� 53   
   $��1��6   $��1��6   $��1��6/   $	��Top=�6@   $��0,5,0,0q�6X    �� Padding$	 0,2q�6i   ���	#FF707070$��32��6�   $��1q�6�   ���	#FF444444._�54      � -   5        $��ViewportMenuGrid��54       55      �� 56      $��* �6'   �� 57      $��* �6'   .+�59      �� 	 �� Orientation$	 
Horizontal=�6   .+�5:      �� $��26��6   $	��2,0q�6*   ���	#FF636363���	#FF9E9E9E
�������
 � ToolTip$

 Undo��6�   ��67   �� *UndoCommand+��5;      �� 5<      $8��2/DirectxWpf;component/Resources/Icons/UndoIcon.png��6$   5>      �� 5?      $��26��6   $	��2,0q�6)   ���	#FF636363���	#FF9E9E9E
���������66   �� *RedoCommand+��5@      �� 5A      $8��2/DirectxWpf;component/Resources/Icons/RedoIcon.png��6$   $

 Redo��6`   5C      5D      �� 5F      $	 
Horizontal=�6   $��0,0,0,0q�67   $��RightToLeft=�6H   $��1��6d   $��Right=�6t   .+�5G      	� -   5        $��rbRotate��5G   "   
 � Tag$ Rotate��62   $��26��6?   $	��2,0q�6J    � Cursor$
 Hand��6W   $
 Rotate��6e   ���	#FF636363���	#FF9E9E9E��5H      �� 5I      .R�5J   "   d� ���	#FF636363���	#FF9E9E9E d� 	IsChecked 6/   �� *	IsChecked+ �� RelativeSource �� *TemplatedParent+ �� Mode$ TwoWay=���5K   /   �� *	Content+ �� *TemplatedParent+$ TwoWay=���5L   /   �� *SetGizmoCommand+��6S   ��  �� *TemplatedParent+ �� Path$ .�5M      ��5O      �� 5P      $:��4/DirectxWpf;component/Resources/Icons/RotateIcon.png��6$    J� OpacityMask �	#FF6363636b   5R      	� 5T      -   5        $��rbScale��5T   "   $ Scale��61   $��26��6=   $	��2,0q�6H   $
 Hand��6U   $
 Scale��6c   ���	#FF636363���	#FF9E9E9E��5U      �� 5V      .R�5W   "   d� ���	#FF636363���	#FF9E9E9E 6/   �� *	IsChecked+ �� *TemplatedParent+$ TwoWay=���5X   /   �� *	Content+ �� *TemplatedParent+$ TwoWay=���5Y   /   �� *SetGizmoCommand+��6S   ��  �� *TemplatedParent+$ .�5Z      ��5\      �� 5]      $9��3/DirectxWpf;component/Resources/Icons/ScaleIcon.png��6$    �	#FF6363636a   5_      	� 5a      -   5        $��rbTranslate��5a   "   $ 	Translate��65   $��26��6E   $��2,0,4,0q�6P   $
 Hand��6a   $
 	Translate��6o   ���	#FF636363���	#FF9E9E9E��5b      �� 5c      .R�5d   "   d� ���	#FF636363���	#FF9E9E9E 6/   �� *	IsChecked+ �� *TemplatedParent+$ TwoWay=���5e   /   �� *	Content+ �� *TemplatedParent+$ TwoWay=���5f   /   �� *SetGizmoCommand+��6S   ��  �� *TemplatedParent+$ .�5g      ��5i      �� 5j      $8��2/DirectxWpf;component/Resources/Icons/MoveIcon.png��6$    �	#FF6363636`   5l      �� 5m      $��1,0,0,0q�6   ���	#FFE8E8E8._�5n      �� $	 
Horizontal=�6!   $��3��6:   $��LeftToRight=�6J   $	��2,0q�6f   .+�5o      �� $��26��6!   $	��2,0q�6-   ���	#FF636363���	#FF9E9E9E��6:   �� *OpenSnapWindowCommand+��5p      �� 5q   "   $8��2/DirectxWpf;component/Resources/Icons/SnapIcon.png��6(    �	#FF6363636d   5s      5t      5u      5v      5x      5y      �� 5{   
   ���	#FF707070$��1,0,1,1q�6)   $��1��6C   $��0,36,0,0q�6S   $��1��6e   ._�5|      "   DaeSharpWpf.DX10RenderCanvas  -   5          Name$ dX10RenderCanvas��5|   ]   |�. $
��Fill=�6x   $��1��6�   $��0q�6�      Viewport 6<   �� *
Viewport10+.  (System.Windows.Interactivity.Interaction   Triggers 5}      /  )System.Windows.Interactivity.EventTrigger  5~         	EventName$ GotFocus��6&   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      /  )GalaSoft.MvvmLight.Command.EventToCommand     Command 6-   �� *ViewportGotFocusCommand+5�        5�      $ GotKeyboardFocus��6&   . 5�         6-   �� *ViewportGotFocusCommand+5�        5�      $ LostKeyboardFocus��6&   . 5�         6-   �� *ViewportLostFocusCommand+5�        5�      $ 	LostFocus��6&   . 5�         6-   �� *ViewportLostFocusCommand+5�      5�      5�      �� 5�   
   ���	#FF343434$��1q�6)   $��2��6=   $��5q�6M   $��2��6X   ���	#FF444444$��1��6�   ._�5�      �� $��2��6   $	 Vertical=�6)   $	��0,5q�6@   .+�5�      �� $��22��6   $	��5,0q�6&    �� TextWrapping$
 Wrap=�63   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �� SelectionBrush �	#FFA6A6A6$��2��6�   $��14(�6  ��6G   �� *GoManager.SelectedObject.Name+$ TwoWay=�6  �� 5�      $��Position��6   $��2��6+   $
��Left=�6;   $��0q�6V   $	��Top=�6a   
�������$��26��6�   $��55��6�   � 5�      -   5        $��PositionGrid��5�      $��0q�6+   $	��Top=�66    5�      �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� $��0��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� */-GoManager.SelectedObject.Transform.Position.X+$ TwoWay=� �� StringFormat$ N2��6  �� 5�      $��1��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� */-GoManager.SelectedObject.Transform.Position.Y+$ TwoWay=�$ N2��6  �� 5�      $��2��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� */-GoManager.SelectedObject.Transform.Position.Z+$ TwoWay=�$ N2��6  5�      �� 5�      $��Rotation��6   $
��Left=�6+   $��0,0,0,0q�6F   $	��Top=�6W   
�������$��26��6�   $��55��6�   � 5�      -   5        $��RotationGrid��5�      $��0q�6+   $	��Top=�66    5�      �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� $��0��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *42GoManager.SelectedObject.Transform.RotationEuler.X+$ TwoWay=�$ N0��6  �� 5�      $��1��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *42GoManager.SelectedObject.Transform.RotationEuler.Y+$ TwoWay=�$ N0��6  �� 5�      $��2��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *42GoManager.SelectedObject.Transform.RotationEuler.Z+$ TwoWay=�$ N0��6  5�      �� 5�      $��Scale��6   $��2��6(   $
��Left=�68   $��0q�6S   $	��Top=�6^   
�������$��26��6�   $��37��6�   � 5�      -	   5        $��	ScaleGrid��5�      $��0q�6(   $	��Top=�63    5�      �� 5�      $��* �6+   �� 5�      $��* �6+   �� 5�      $��* �6+   .+�5�      �� $��0��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *,*GoManager.SelectedObject.Transform.Scale.X+$ TwoWay=�$ N2��6  �� 5�      $��1��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *,*GoManager.SelectedObject.Transform.Scale.Y+$ TwoWay=�$ N2��6  �� 5�      $��2��6   $��22��6.   $	��5,0q�6:   $
 Wrap=�6G   $	��Top=�6�   ���	#FF505050���	#FF343434
������� �	#FFA6A6A6��6[   �� *,*GoManager.SelectedObject.Transform.Scale.Z+$ TwoWay=�$ N2��6  5�      5�      5�      ]� 5�   
   $��1��6   $	��0,5q�6$   $��2��61   .+�5�      � $	��Top=�6   $��32��6(   ���	#FF323232$	��Top=�6M   .+�5�      �� $	 
Horizontal=�6   .+�5�      �� $��23��6   $
��10,0q�6(   $��Filters��66   
�������6I   �� 5�      $��23��6^   $	��100��6j   $
��Left=�6v   ���	#FF505050���	#FF343434
���   � �� SelectedIndex 6   �� *&$LBFilesViewModel.SelectedFilterIndex+.@�5�      �� $��Models��6'   $
��Left=�68   $��98��6S   ���	#FF272727���	#FFF0F0F06u   �� 5�      $��Collision Meshes��6'   $
��Left=�6B   $��98��6]   ���	#FF272727���	#FFF0F0F06   �� 5�      $
��Maps��6'   $
��Left=�66   $��98��6Q   ���	#FF272727���	#FFF0F0F06s   �� 5�      $	��All��6'   $
��Left=�65   $��98��6P   ���	#FF272727���	#FFF0F0F06r   5�      �� 5�      $
��10,0q�6G   $��23��6U   $	��100��6a   ���	#FF505050���	#FF232323
������� �	#FFB8B8B8��6   �� *LBFilesViewModel.Filter+6�   5�      5�      �� 5�      $��Bottom=�6   $��Disabled=�6b   $��1��6�   $��2��6�   ���	#FF444444���	#FF343434
���������6/   �� *LBFilesViewModel.FilesList+��5�      �� 5�      .R�5�      �  � Orientation$ 
Horizontal=�6%   5�      ��5�      �� 5�      .R�5�      �� $��5q�6%   .+�5�      �� $��80��6%   $��80��61   ���	#FF343434$��1q�6T    �� CornerRadius$ 2��6h   ���	#FF404040._�5�   "   �� $��Center=�6L   $��Center=�6g   $��Impact)�6�   $��20(�6�   ���	#FFD4D4D4���	#FF0A0A0A��6)   �� *NoDotExtension+6�   5�      �� 5�      $��16��6(   $��80��64   ��6?   �� *
FileName+ J� InputBindings 5�      d� 5�   "    d� MouseAction$ LeftDoubleClicke�6/     �� Command  5�   #   ��  �� ! �� AncestorType$! WindowL�$( "DataContext.ItemDoubleClickCommand�" �� CommandParameter" 5�   %   �� *.+5�      5�      5�      5�      R� 5�   
   -
   5        # R�Name$# TrvDirectories��5�      $��5q�6g   $��2��6r   ���	#FF444444���	#FF343434���	#FF636363��6+   �� *$"TRVDirectoriesViewModel.Root.Items+��5�      �� 5�      $ �� DataType$  DirectxWpf.MVVM_Model.TreeNode"$  6+   % �� ItemsSource% 6N   �� *Items+.R�5�      �� 
���������6    �� *Title+67   5�       5�        5�      $ SelectedItemChanged��6"   . 5�         6)   �� *)'LBFilesViewModel.ChangeDirectoryCommand+&   CommandParameter& 6e   �� ' �� ElementName$' TrvDirectories��$ SelectedItem�5�      5�      R� 5�   
   -   5        $# TrvObjectList��5�      ( J� 	AllowDrop( . $��5,5,5,0q�6i   $��1��6z   ���	#FF444444���	#FF343434���	#FF636363��6*   �� *GoTreeViewModel.Root+��5�      �� 5�      =  7DirectxWpf.MVVM_ViewModel.TRVGameObjectElementViewModel"$  6+   % 6`   �� *
Children+.R�5�      �� .+�5�      �� 
���������6    �� *GameObject.Name+6A   )   	Behaviors) 5�      6  0DirectxWpf.Behavior.FrameworkElementDragBehavior  5�      6	  0DirectxWpf.Behavior.FrameworkElementDropBehavior	  5�      5�      5�      ��5�      �� 5�      * �� 
TargetType"* Q�6   .�5�      �� + �� Property+ �@Q�
IsExpanded, �� Value, . 63   �� 5�      + �@Q�
IsSelected, 63   �� *GameObject.IsSelected+5�       5        5     $ SelectedItemChanged��6"   . 5       -   PassEventArgsToCommand- .  6)   �� *,*GoTreeViewModel.ChangeSelecedObjectCommand+6h   5     5     5     5     