using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace Twitigo.ViewModels
{
    public class TabViewModel : ViewModelBase
    {
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

        #region IEnumerable<Status> Tweets
        private IEnumerable<Status> _Tweets;
        public IEnumerable<Status> Tweets
        {
            get
            {
                return _Tweets;
            }
            set
            {
                if (_Tweets != value)
                {
                    _Tweets = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
    }
}
