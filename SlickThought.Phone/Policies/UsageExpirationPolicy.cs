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

namespace SlickThought.Phone
{
    [DataContract]
    public class UsageExpirationPolicy : ExpirationPolicy
    {
        private int _usageCount = 0;

        [DataMember]
        public int MaxUsage { get; set; }

        [DataMember]
        public int UsageCount {
            get { return _usageCount; }
            set
            {
                _usageCount = value;
                if (_usageCount > this.MaxUsage)
                    this.IsExpired = true;
            }
        }

        public UsageExpirationPolicy() { }
    }
}
