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
    /// <summary>
    /// Time-based expiration policies can be set either for the total installed lifetime of the app (try for 30 days), or 
    /// can be set to run for a fixed time each time it is launched (5 minutes each use).
    /// </summary>
    [DataContract]
    public enum TimeExpirationMode
    {
        [EnumMember]
        Lifetime,
        [EnumMember]
        Session
    }

    /// <summary>
    /// Contains a time based application expiration policy that can be monitored for the lifetime usage of an app or on a per session basis
    /// </summary>
    [DataContract]
    public class TimeExpirationPolicy : ExpirationPolicy
    {
        TimeSpan _previousElapsedTime;                          // saves previous elapsed time when policy is deserialized and started again for lifetime policy
        bool _isRunning = false;                                // used to control setup of policy when first Tick event is received
        static DateTime _startTime;                             // set to static since all Time policies will have same start time
                                                                // also solves issue with policies being deserialized and not calling constructor

        /// <summary>
        /// The total elapsed time the application has been used based on lifetime or session
        /// </summary>
        [DataMember]
        public TimeSpan ElapsedTime { get; set; }               
        
        /// <summary>
        /// Time span that controls the total time of usage before trial application expires
        /// </summary>
       [DataMember]
        public TimeSpan TrialDuration { get;   set; }           

        /// <summary>
        /// controls lifetime or session monitoring
        /// </summary>
        [DataMember]       
        public TimeExpirationMode Mode { get;  set; }           

        /// <summary>
        /// Default constructor
        /// </summary>
        public TimeExpirationPolicy() {
            _startTime = DateTime.Now;                          // set start time for all time policy objects (right now there is only one)
        }

        /// <summary>
        /// Attached to the TrialManager Timer object to be notified of each Tick according to TrialManager TimerInterval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TimerTick(object sender, EventArgs e)
        {
            if (!_isRunning)                                            // are we startign for the first time?       
            {
                _previousElapsedTime = this.ElapsedTime;
                if (this.Mode == TimeExpirationMode.Session)            // if per session, we don't care what the past time usage was
                    this.ElapsedTime = TimeSpan.Zero;
                 _isRunning = true;                                     // we are running
            }

            UpdateElapsedTime();

            if (this.ElapsedTime >= this.TrialDuration)
            {
                this.IsExpired = true;
            }
        }

        public void UpdateElapsedTime()
        {
            TimeSpan sessionElapsedTime = DateTime.Now - _startTime;        // how much time has passed for the entire session
            this.ElapsedTime = sessionElapsedTime + _previousElapsedTime;   // update total elapsed time property
        }
    }
}
