using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;
using ReactiveUI.Routing;
using TweetRex.Desktop.ViewModels;

namespace TweetRex.Desktop.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl, IViewFor<IHomeViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(IHomeViewModel), typeof(HomeView), new PropertyMetadata(null));


        public HomeView()
        {
            InitializeComponent();
            this.WhenNavigatedTo(ViewModel, () =>
            {
                /* COOLSTUFF: Setting up the View
                 * 
                 * Whenever we're Navigated to, we want to set up some bindings.
                 * In particular, we want to Subscribe to the HelloWorld command
                 * and whenever the ViewModel invokes it, we will pop up a 
                 * Message Box.
                 */


                // Make XAML Bindings be relative to our ViewModel
                DataContext = ViewModel;


                return ViewModel.RefreshCommand.Subscribe(param =>
                    MessageBox.Show("It worked!"));
            });

        }

        public IHomeViewModel ViewModel
        {
            get { return (IHomeViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }        

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IHomeViewModel)value; }
        }
    }

    // XXX: Ignore the man behind this curtain. This will soon be in ReactiveUI itself
    public static class ViewForMixins
    {
        public static IDisposable WhenNavigatedTo<TView, TViewModel>(this TView This, TViewModel viewModel, Func<IDisposable> onNavigatedTo)
            where TView : IViewFor<TViewModel>
            where TViewModel : class, IRoutableViewModel
        {
            var disp = Disposable.Empty;
            var inner = This.WhenAny(x => x.ViewModel, x => x.Value)
                .Where(x => x != null && x.HostScreen.Router.GetCurrentViewModel() == x)
                .Subscribe(x =>
                {
                    if (disp != null) disp.Dispose();
                    disp = onNavigatedTo();
                });


            return Disposable.Create(() =>
            {
                inner.Dispose();
                disp.Dispose();
            });
        }
    }

}
