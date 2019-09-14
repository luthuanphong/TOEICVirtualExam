using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TOEICVirtualExam
{
    public class CountdownTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CountdownTimer));

        private DispatcherTimer _timer;

        private TimeSpan _countdownTime;

        private Action<string> _callbackAction;

        private Action _callTimeUp;

        private object _object = new object();

        private bool _isPaused = false;

        public CountdownTimer (TimeSpan countdownTime, Dispatcher dispathcher, Action<string> callBack, Action timeUp)
        {
            this._countdownTime = countdownTime;

            this._timer = new DispatcherTimer(TimeSpan.FromSeconds(1),
                DispatcherPriority.Normal,
                HandleTick, dispathcher);

            this._callbackAction = callBack;
            this._callTimeUp = timeUp;
        }

        public void Start()
        {
            try
            {
                this._timer.Start();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Unable to start timer due to unexpected exception - {0}", ex);
            }
        }

        public void Pause()
        {
            lock (this._object)
            {
                this._isPaused = true;
            }
        }

        public void Resume()
        {
            lock (this._object)
            {
                this._isPaused = false;
            }
        }

        public void Stop()
        {
            try
            {
                this._timer.Stop();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Unable to stop timer due to unexpected exception - {0}", ex);
            }
        }

        private void HandleTick(object sender, EventArgs ev)
        {
            lock (this._object)
            {
                if (this._isPaused)
                {
                    return;
                }

                if (this._callbackAction != null)
                {
                    this._callbackAction.Invoke(this._countdownTime.ToString("c"));
                }
                else
                {
                    Logger.Debug("Callback Action is null.");
                }

                if (this._countdownTime == TimeSpan.Zero)
                {
                    this.Stop();

                    if(this._callTimeUp != null)
                    {
                        this._callTimeUp.Invoke();
                    }
                    else
                    {
                        Logger.Debug("Timeup callback is null.");
                    }
                }

                this._countdownTime = this._countdownTime.Add(TimeSpan.FromSeconds(-1));
            }
        }
    }
}
