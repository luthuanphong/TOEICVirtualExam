namespace TOEICVirtualExam
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for ExamView.xaml
    /// </summary>
    public partial class ExamView : Window
    {
        /// <summary>
        /// Test time countdown
        /// </summary>
        private CountdownTimer _timer;

        /// <summary>
        /// Test resource
        /// </summary>
        private ToeicTest _test;

        /// <summary>
        /// Time of the test
        /// </summary>
        private TimeSpan _defaultTimeSpan = new TimeSpan(2, 0, 0);

        /// <summary>
        /// Audio runner
        /// </summary>
        private MediaPlayer audioPlayer = new MediaPlayer();

        /// <summary>
        /// Answer key mapped with A-0,B-1,C-2,D-3
        /// </summary>
        private string[] answerKey = new string[] {"A","B","C","D"};

        /// <summary>
        /// Examination view constructor
        /// </summary>
        /// <param name="test">Test basic resource</param>
        public ExamView(ToeicTest test = null)
        {
            InitializeComponent();
            this._timer = new CountdownTimer(
                this._defaultTimeSpan,
                this.Dispatcher,
                this.CountIntervalCallBack, this.TimeUp);

            if(test != null)
            {
                this._test = test;
                this.Initialize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,string> GetListenningAnswers()
        {
            Dictionary<int, string> answerWithAlphabet = new Dictionary<int, string>();
            Dictionary<int,int> answers = this.ListeningTestAnswers.GetAnswers();

            foreach(KeyValuePair<int,int> answer in answers)
            {
                answerWithAlphabet.Add(
                    answer.Key, 
                    answer.Value == -1 ? string.Empty : answerKey[answer.Value]
                    );
            }

            return answerWithAlphabet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetReadingAnswers()
        {
            Dictionary<int, string> answerWithAlphabet = new Dictionary<int, string>();
            Dictionary<int, int> answers = this.ReadingTestAnswers.GetAnswers();

            foreach(KeyValuePair<int,int> answer in answers)
            {
                answerWithAlphabet.Add(
                    answer.Key,
                    answer.Value == -1 ? string.Empty : answerKey[answer.Value]
                    );
            }

            return answerWithAlphabet;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            this.audioPlayer.Open(new Uri(this._test.Audio));
            this.ListeningTest.Navigate(this._test.Listening);
            this.ListeningTest.LoadCompleted += ListenningTestLoad;
            this.ReadingTest.Navigate(this._test.Reading);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        private void CountIntervalCallBack(string time)
        {
            this.Timer.Text = time;
        }

        /// <summary>
        /// 
        /// </summary>
        private void TimeUp()
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void ListenningTestLoad(object sender, EventArgs ev)
        {
            //this.audioPlayer.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.audioPlayer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this._timer.Pause();
            this.audioPlayer.Pause();
            MessageBoxResult result = 
                MessageBox.Show(
                    "Are you sure to Submit your test?",
                    "Submit your test?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.TimeUp();
            }
            else
            {
                this._timer.Resume();
                this.audioPlayer.Play();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.audioPlayer.Play();
        }
    }
}
