   M S B A M L   `   `   `  ���� G  CAppsExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:AppsExplorerAppsExplorer  #    AppsExplorer.AppsGroupPackage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  R NMahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  &localclr-namespace:AppsExplorer  -     � WindowStartupLocation$  CenterScreen=� � Title$ AppsGroup Manage Tool��$	��250��$	��500��(  "MahApps.Metro.Controls.MetroWindow   TitleCharacterCasing$ Normal=�.��� .+��� .+�,  &MahApps.Metro.Controls.MetroTabControl  -   $��tabmain��-  'MahApps.Metro.Controls.TabControlHelper   
Underlined$ SelectedTabItem=�.@�)  #MahApps.Metro.Controls.MetroTabItem  -   $��saveTab��$��
Save      �� � ToolTip$2 ,Save current AppsGroup layout as new package��$��2q�.���� $��3��$��1��$��	10,20,0,0q�.+���  �� Orientation$ 
Horizontal=�$��0q�.+��� $��Save as *.appPckZip:��$��Center=�$��0q��� -   $��saveAsAppZipPackagePath��$��20,0,0,0q�
������ $��0q�$��30��$5 /Select the path for target *.appPckZip package.����/  )MahApps.Metro.IconPacks.MaterialExtension  �  �MahApps.Metro.IconPacks.PackIconExtension`2[[MahApps.Metro.IconPacks.PackIconMaterial, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null],[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null]]   Kind$  ArrowLeftBoldCircleOutline=�   Flip$
 BothX��� $��0,10,0,0q�$��5q�   AccentColorBrush#���    �� IsDocumentEnabled . 	 �� 
IsReadOnly	 . . �/� .T�J� .*�.*��� $�Selected .appPckZip Name:���� �� -   
 �Name$
 saveZipName��$��Oblique$��� .+��� $ 
Horizontal=�$��Right=�.+�.  (MahApps.Metro.IconPacks.PackIconMaterial  E AControlzEx, Version=3.0.2.4, Culture=neutral, PublicKeyToken=null�  �ControlzEx.PackIconBase`1[[MahApps.Metro.IconPacks.PackIconMaterialKind, MahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null]]   Kind$ HandPointingRight=�$��Center=�$��0 2 0 20q��� -   $��GO��$��40��$��Right=�$��Center=�$��	10 2 0 20q�$ saveAppsGroup���� -   $��Right=�$��Center=�$
 Quit��$��	10 2 0 20q���  $ CloseBox=�$
 BothX�  -   $��loadTab��$��
Load      ��$K ELoad from existing package. Current AppsGroup layout will be removed.��$��20 2 2 2q�.���� $��3��$��1��$��	10,20,0,0q�.+��� $ 
Horizontal=�$��0q�.+��� $��Load from *.appPckZip:��$��Center=�$��0q��� -	   $��loadAppZipPackagePath��$��20,0,0,0q�
������ $��0q�$��30��$5 /Select the path for target *.appPckZip package.����  $  ArrowLeftBoldCircleOutline=�$
 BothX��� $��0,10,0,0q�$��5q�#���    . 	 . . �/� .T�J� .*�.*��� $�Selected .appPckZip Name:���� �� -
   $
 loadZipName��$��Oblique$��� .+��� $ 
Horizontal=�$��Right=�.+�  $ HandPointingRight=�$��Center=�$��0 2 0 20q��� -   $��GO��$��40��$��Right=�$��Center=�$��	10 2 0 20q�$ loadAppsGroup���� -   $��Right=�$��Center=�$
 Quit��$��	10 2 0 20q���  $ CloseBox=�$
 BothX��� $3��-Be sure of that no APP is running before use!��
���  ��