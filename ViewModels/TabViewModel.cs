using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using System.Windows.Input;

namespace Twitigo.ViewModels
{
    public class TabViewModel : ViewModelBase
    {
        public TabViewModel()
        {
            this.replyCommand = new DelegateCommand(ReplyCommandExecute);
            this.retweetCommand = new DelegateCommand(RetweetCommandExecute);
            this.favoriteCommand = new DelegateCommand(FavoriteCommandExecute);
        }

        private void ReplyCommandExecute(object parameter)
        {
            var status = parameter as Status;

            // throw new NotImplementedException("Reply Action");
        }
        
        private void RetweetCommandExecute(object parameter)
        {
            var status = parameter as Status;

            throw new NotImplementedException("Retweet Action");
        }

        private void FavoriteCommandExecute(object parameter)
        {
            var status = parameter as Status;

            throw new NotImplementedException("Favorite Action");
        }


        public void Reload()
        {
            _Tweets = null;
            RaisePropertyChanged(() => Tweets);
        }

        #region string Header
        private string _Header;
        public string Header
        {
            get
            {
                return _Header;
            }
            set
            {
                if (_Header != value)
                {
                    _Header = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region IQueryable<Status> Query
        private Func<IQueryable<Status>> _Query;
        public Func<IQueryable<Status>> Query
        {
            get
            {
                return _Query;
            }
            set
            {
                if (_Query != value)
                {
                    _Query = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region IEnumerable<Status> Tweets
        private IEnumerable<Status> _Tweets;
        public IEnumerable<Status> Tweets
        {
            get
            {
                if (_Query == null)
                    return null;

                if (_Tweets == null)
                    _Tweets = _Query();

                return _Tweets;
            }
        }
        #endregion

        #region Commands
        private readonly ICommand replyCommand;
        public ICommand ReplyCommand
        {
            get
            {
                return replyCommand;
            }
        }

        private readonly ICommand retweetCommand;
        public ICommand RetweetCommand
        {
            get
            {
                return this.retweetCommand;
            }
        }

        private readonly ICommand favoriteCommand;
        public ICommand FavoriteCommand
        {
            get
            {
                return this.favoriteCommand;
            }
        }
        #endregion
    }
}
