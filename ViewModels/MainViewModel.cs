using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LinqToTwitter;

namespace Twitigo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private PinAuthorizer pinAuthorizer;
        private TwitterContext twitter;

        public MainViewModel()
        {
            this.reloadCommand = new DelegateCommand(ReloadCommandExecute);
            this.pinRequestCommand = new DelegateCommand(PinRequestCommandExecute);
            this.pinEnterCommand = new DelegateCommand(PinEnterCommandExecute);

            var auth = new SingleUserAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = Properties.Settings.Default.ConsumerKey,
                    ConsumerSecret = Properties.Settings.Default.ConsumerSecret,
                    OAuthToken = Properties.Settings.Default.AccessTokenKey,
                    AccessToken = Properties.Settings.Default.AccessTokenSecret,
                }
            };

            this.twitter = new TwitterContext(auth);

            this.Tabs = new TabViewModel[]{
                new TabViewModel
                {
                    Header = "Home",
                    Query = () => from tweet in twitter.Status
                                  where tweet.Type == StatusType.Home
                                  select tweet,
                },
                new TabViewModel
                {
                    Header = "Mentions",
                    Query = () => from tweet in twitter.Status
                                  where tweet.Type == StatusType.Mentions
                                  select tweet,
                },
            };

            this.SelectedTab = this.Tabs.First();
        }

        private void ReloadCommandExecute(object parameter)
        {
            this.SelectedTab.Reload();
        }

        private void PinRequestCommandExecute(object parameter)
        {
            Properties.Settings.Default.Save();

            this.pinAuthorizer = new PinAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = Properties.Settings.Default.ConsumerKey,
                    ConsumerSecret = Properties.Settings.Default.ConsumerSecret,
                },
                UseCompression = true,
                GoToTwitterAuthorization = (pageLink) => Process.Start(pageLink),
            };

            this.pinAuthorizer.BeginAuthorize(resp => {});
        }

        private async void PinEnterCommandExecute(object parameter)
        {
            pinAuthorizer.CompleteAuthorize(
                this.PinCode,
                completeResp =>
                {
                    this.twitter = new TwitterContext(this.pinAuthorizer);
                }
            );
        }

        #region string ConsumerKey
        public string ConsumerKey
        {
            get
            {
                return Properties.Settings.Default.ConsumerKey;
            }
            set
            {
                Properties.Settings.Default.ConsumerKey = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region string ConsumerSecret
        public string ConsumerSecret
        {
            get
            {
                return Properties.Settings.Default.ConsumerSecret;
            }
            set
            {
                Properties.Settings.Default.ConsumerSecret = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region string PinCode
        private string _PinCode;
        public string PinCode
        {
            get
            {
                return _PinCode;
            }
            set
            {
                if (_PinCode != value)
                {
                    _PinCode = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
        
        #region IEnumerable<TabViewModel> Tabs
        private IEnumerable<TabViewModel> _Tabs;
        public IEnumerable<TabViewModel> Tabs
        {
            get
            {
                return _Tabs;
            }
            set
            {
                if (_Tabs != value)
                {
                    _Tabs = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region TabViewModel SelectedTab
        private TabViewModel _SelectedTab;
        public TabViewModel SelectedTab
        {
            get
            {
                return _SelectedTab;
            }
            set
            {
                if (_SelectedTab != value)
                {
                    _SelectedTab = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        private readonly ICommand reloadCommand;
        public ICommand ReloadCommand
        {
            get
            {
                return this.reloadCommand;
            }
        }

        private readonly ICommand pinRequestCommand;
        public ICommand PinRequestCommand
        {
            get
            {
                return this.pinRequestCommand;
            }
        }

        private readonly ICommand pinEnterCommand;
        public ICommand PinEnterCommand
        {
            get
            {
                return this.pinEnterCommand;
            }
        }
        #endregion
    }
}
