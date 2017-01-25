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
using Microsoft.Phone.Marketplace;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Markup;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Phone.Controls;

namespace SlickThought.Phone
{
    /// <summary>
    /// Trial Manager controls the policy management for trial software.
    /// </summary>
    public class TrialManager: IApplicationService 
    {
        const double TIMER_INTERVAL = 60;                                       // Default interval of the timer
        const string DATA_FILE = "apppolicy.dat";                               // name of data file to save state to on app exit

        LicenseInformation _license;                                            // WP7 LicenseInformation object to access IsTrial API
        ExpirationPolicy _applicationPolicy;                                    // Policy to control expiration of Trial software

        static DispatcherTimer _timer;                                          // timer - used by Time Expiration Policies to monitor time usage
        
        /// <summary>
        /// Signals expiration of trial software according to expiration policy
        /// </summary>
        public event EventHandler<TrialExpirationEventArgs> Expired;            

        /// <summary>
        /// Access to the currently instantiaited TrialManager for the application
        /// </summary>
        public static TrialManager Current { get; private set; }                

        /// <summary>
        /// Controls the Tick interval on the timer
        /// </summary>
        public TimeSpan TimerInterval { get; set; }                           


        /// <summary>
        /// Configures TrialManager to simulate IsTrial == true for testing purposes
        /// </summary>
        public bool RunAsTrial { get; set; }

        /// <summary>
        /// Prevents saving Application Policy state
        /// </summary>
        public bool DoNotPersist { get; set; }                

        /// <summary>
        /// Returns if the app is running in Trial mode
        /// </summary>
        public bool IsTrial()                                             
        {        
                if (this.RunAsTrial)                                // If manual set to run as trial software, return true
                    return true;
                else                                                // otherwise check LicenseInformation object and IsTrail API 
                {
                    if (_license == null)
                        _license = new LicenseInformation();
                    return _license.IsTrial();
                }      
        }

        /// <summary>
        /// Determines if the Application Expiration Policy is expired
        /// </summary>
        public bool IsExpired { get { return this.ApplicationPolicy.IsExpired; } }  

        /// <summary>
        /// Application Expiration Policy object - controls expiration "rules"
        /// </summary>
        public ExpirationPolicy ApplicationPolicy {                                 
            get { return _applicationPolicy; }
            set {
                if (value == null)                      // always have to have a policy object
                    throw new InvalidOperationException("Cannot set Application Expiration Policy to null.");

                _applicationPolicy = value;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TrialManager() {
            TrialManager.Current = this;                                    // provide access to this instance
            this.TimerInterval = TimeSpan.FromSeconds(TIMER_INTERVAL);      // configure timer interval
        }

        // Checks isolated storage and restores past expiration policy data when the app has been run more than once
        private  void Load()
        {
            if (!IsolatedStorageFile.GetUserStoreForApplication().FileExists(DATA_FILE))    // check for data file - if it doesnt exist, use settings from App.xaml
                return;                                                                         

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                using (var fileStream = new IsolatedStorageFileStream(DATA_FILE, FileMode.Open, isf))
                {
                    DataContractSerializer dcs = new DataContractSerializer(ApplicationPolicy.GetType());
                    var newPolicy = dcs.ReadObject(fileStream);

                    if (newPolicy != null)
                        this.ApplicationPolicy = newPolicy as ExpirationPolicy;               
                }
        }

        /// <summary>
        /// Save application policy state to isolated storage
        /// </summary>
 
        public void Save()
        {
            if (!this.DoNotPersist)           // only need to save the data if we are still trial software
            {
                if (this.ApplicationPolicy.GetType() == typeof(TimeExpirationPolicy))
                    (this.ApplicationPolicy as TimeExpirationPolicy).UpdateElapsedTime();

                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fileStream = new IsolatedStorageFileStream(DATA_FILE, FileMode.Create, isf))
                    {
                        DataContractSerializer dcs = new DataContractSerializer(ApplicationPolicy.GetType());
                        dcs.WriteObject(fileStream,this.ApplicationPolicy);
                    }
                }
            }
        }

        // Set up policy 
        private  void Initialize()
        {
            if (this.ApplicationPolicy as TimeExpirationPolicy != null)     // If time based expiration, wire policy to timer tick event and start timer
            {
                _timer = new DispatcherTimer();
                _timer.Tick += (this.ApplicationPolicy as TimeExpirationPolicy).TimerTick;
                _timer.Interval = this.TimerInterval;
                _timer.Start();
            }

            this.ApplicationPolicy.Expired += new EventHandler<TrialExpirationEventArgs>(ApplicationPolicy_Expired);    // hook up Expired event

            if (this.ApplicationPolicy.IsExpired)       // We may have already expired the app from previous usage
                ProcessExpiration();
        }

        // if policy fires expired event, fire Expired event for Trial manager.  Doing this to support multiple policies in the future...
        void ApplicationPolicy_Expired(object sender, TrialExpirationEventArgs e)
        {
            ProcessExpiration();
        }

        // Stop the timer if necessary, and fire the expired event
        private void ProcessExpiration()
        {
            if (_timer != null)
                _timer.Stop();

            var handler = this.Expired;

            if (handler != null)
                this.Expired(this, new TrialExpirationEventArgs());
        }

        /// <summary>
        /// IApplication interface - called by the app when it starts 
        /// </summary>
        /// <param name="context"></param>
        public void StartService(ApplicationServiceContext context)
        {
            if (this.IsTrial()) { 
                Load();
                Initialize();
            }
        }

        /// <summary>
        /// IApplication inteface - called when the app exits
        /// </summary>
        public void StopService()
        {
            if (this.IsTrial())
            {
                Shutdown();
            }
        }

        // clean up the timer and save the application policy state
        private void Shutdown()
        {
            if (this.ApplicationPolicy as TimeExpirationPolicy != null )
            {            
                _timer.Tick -= (this.ApplicationPolicy as TimeExpirationPolicy).TimerTick;
            }
           Save();
        }


    }
}
