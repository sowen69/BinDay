using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace SlickThought.Phone
{
    [DataContract]
    public abstract class ExpirationPolicy 
    {
        protected bool _isExpired;

        public event EventHandler<TrialExpirationEventArgs> Expired;

        [IgnoreDataMember]
        public virtual bool IsExpired {
            get { return _isExpired; }
            set
            {
                _isExpired = value;
                if (_isExpired)
                    OnExpired();
            }
        }

        public ExpirationPolicy() { }

        protected virtual void OnExpired()
        {
            var handler = this.Expired;
            if (handler != null)
                Expired(this, new TrialExpirationEventArgs());
        }


    }
}
