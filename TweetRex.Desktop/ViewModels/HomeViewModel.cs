using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using ReactiveUI.Routing;
using ReactiveUI.Xaml;

namespace TweetRex.Desktop.ViewModels
{
    public interface IHomeViewModel : IRoutableViewModel
    {
        ReactiveCommand RefreshCommand { get; }
    }

    class HomeViewModel: ReactiveObject, IHomeViewModel
    {
        public HomeViewModel(IScreen screen)
        {
            this.HostScreen = screen;
            this.RefreshCommand = new ReactiveCommand();
        }

        public string UrlPathSegment { get { return "Home"; } }
        public IScreen HostScreen { get; private set; }
        public ReactiveCommand RefreshCommand { get; private set; }
    }
}
