# Getting Started



1. Create a new maui project

   

2. Install nuget package    [![package](https://img.shields.io/nuget/vpre/Material.Components.Maui?style=for-the-badge)](https://www.nuget.org/packages/Material.Components.Maui) 

   

3. Add `UseMaterialComponents` in `MauiProgram.cs`

   ```c#
   using Material.Components.Maui.Extensions;
   
   ...
   
       public static MauiApp CreateMauiApp()
       {
           var builder = MauiApp.CreateBuilder();
       
       	...
               
           builder
               .UseMaterialComponents(new List<string>
               {
                   //generally, we needs add 6 types of font families
                   "xxx-Regular.ttf",
                   "xxx-Italic.ttf",
                   "xxx-Medium.ttf",
                   "xxx-MediumItalic.ttf",
                   "xxx-Bold.ttf",
                   "xxx-BoldItalic.ttf",
               });
           return builder.Build();
       }
   
   ...
       
   ```

   

4. Add Material colors & styles in `App.xaml`
   ```xml
   <?xml version="1.0" encoding="UTF-8" ?>
   <Application
                
      ...
                
       xmlns:mds="clr-namespace:Material.Components.Maui.Styles;assembly=Material.Components.Maui">
       <Application.Resources>
           <ResourceDictionary>
               <ResourceDictionary.MergedDictionaries>
                   
                   ...
                   
                   <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                   <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
                   <mds:MaterialStyles />
                   <!--or use seedColor
                   <mds:MaterialStyles Dark="DarkBlue" Light="Green" />-->
               </ResourceDictionary.MergedDictionaries>
           </ResourceDictionary>
       </Application.Resources>
   </Application>
   ```

   

5. Using components in `.xaml`

   ```xml
   <?xml version="1.0" encoding="utf-8" ?>
   <ContentPage
       
       ...
                
       xmlns:mdc="clr-namespace:Material.Components.Maui;assembly=Material.Components.Maui">
       
       <mdc:Button Text="button" Icon="Add" />
       
   </ContentPage>
   ```
