//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("MeetAndGo.Views.SearchPage.xaml", "Views/SearchPage.xaml", typeof(global::MeetAndGo.Views.SearchPage))]

namespace MeetAndGo.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views\\SearchPage.xaml")]
    public partial class SearchPage : global::Xamarin.Forms.ContentPage {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::MeetAndGo.Controls.UserMapControl map;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::DurianCode.PlacesSearchBar.PlacesBar StartLocationBar;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::DurianCode.PlacesSearchBar.PlacesBar EndLocationBar;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(SearchPage));
            map = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::MeetAndGo.Controls.UserMapControl>(this, "map");
            StartLocationBar = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::DurianCode.PlacesSearchBar.PlacesBar>(this, "StartLocationBar");
            EndLocationBar = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::DurianCode.PlacesSearchBar.PlacesBar>(this, "EndLocationBar");
        }
    }
}
