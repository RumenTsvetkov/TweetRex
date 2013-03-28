using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Routing;
using TweetRex.Desktop.ViewModels;

namespace TweetRex.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //RxApp.Register(typeof(AppBootstrapper), typeof(IScreen)); 
            base.OnStartup(e);            
        }
    }
}
