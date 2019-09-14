using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.IO;
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
using Path = System.IO.Path;

namespace TOEICVirtualExam
{
    /// <summary>
    /// Interaction logic for InsertTest.xaml
    /// </summary>
    public partial class InsertTest : Window
    {
        public InsertTest()
        {
            InitializeComponent();
        }

        private void BrowseListeningFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pdf files (*.pdf)|*.pdf";
            if(dialog.ShowDialog() == true)
            {
                this.ListenFilePath.Text = dialog.FileName;
            }
        }

        private void BrowseReadingFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "pdf files (*.pdf)|*.pdf";
            if (dialog.ShowDialog() == true)
            {
                this.ReadingFilePath.Text = dialog.FileName;
            }
        }

        private void BrowseAudioFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "audio files (*.mp3)|*.mp3";
            if (dialog.ShowDialog() == true)
            {
                this.AudiogFilePath.Text = dialog.FileName;
            }
        }

        private void BrowseAnswerFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                this.AnswergFilePath.Text = dialog.FileName;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.InsertProgressBar.Value = 0;

            if (!File.Exists(this.ListenFilePath.Text))
            {
                MessageBox.Show(
                    "Listening file is missing",
                    "File missing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(this.ReadingFilePath.Text))
            {
                MessageBox.Show(
                    "Reading file is missing",
                    "File missing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(this.AudiogFilePath.Text))
            {
                MessageBox.Show(
                    "Audio file is missing",
                    "File missing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (!File.Exists(this.AnswergFilePath.Text))
            {
                MessageBox.Show(
                    "Answer file is missing",
                    "File missing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if(
                string.IsNullOrEmpty(this.TestName.Text) || 
                string.IsNullOrWhiteSpace(this.TestName.Name))
            {
                MessageBox.Show(
                    "Name is undefined",
                    "No name",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            this.AddTest(
                this.TestName.Text,
                this.ListenFilePath.Text,
                this.ReadingFilePath.Text,
                this.AudiogFilePath.Text,
                this.AnswergFilePath.Text
                );
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }

        private void AddTest(string name, string listenning, string reading, string audio, string answer)
        {
            string testPath;
            SQLiteTransaction transaction;
            IDataAccess dataAccess = LocalDataAccess.GetDBAccess();
            transaction = dataAccess.GetCurrentConnection().BeginTransaction();

            testPath = Path.Combine(
                ConfigurationManager.AppSettings[Common.TEST_PATH],
                DateTime.Now.ToString("yyyyMMddHHmmss"));

            try
            {
                //Get Answer
                string[] lines = File.ReadAllLines(answer);
                string listenning_answer = lines.Where(x => x.Contains(Common.LISTEN) && x.Contains("=")).FirstOrDefault();
                string reading_answer = lines.Where(x => x.Contains(Common.READ) && x.Contains("=")).FirstOrDefault();

                if (listenning_answer == null)
                {
                    MessageBox.Show(
                        "Listenning data is incorrect",
                        "Incorrect data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (reading_answer == null)
                {
                    MessageBox.Show(
                        "Reading data is incorrect",
                        "Incorrect data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                listenning_answer = listenning_answer.Split(new char[] { '=' })[1];
                reading_answer = reading_answer.Split(new char[] { '=' })[1];

                if (listenning_answer.Split(new char[] { ',' }).Length != 100)
                {
                    MessageBox.Show(
                        "Number of answer of listenning test is not equal to 100",
                        "Incorrect data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (reading_answer.Split(new char[] { ',' }).Length != 100)
                {
                    MessageBox.Show(
                        "Number of answer of reading test is not equal to 100",
                        "Incorrect data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                //Insert into DB
                dataAccess.InsertTestCorrectAnswer(name, listenning_answer, reading_answer, testPath, transaction);

                this.InsertProgressBar.Value = 25;

                string testFullPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    testPath);

                Directory.CreateDirectory(testFullPath);

                //Copy LC file

                string LC_path = Path.Combine(testFullPath, Common.LISTEN + @"\");

                Directory.CreateDirectory(LC_path);

                File.Copy(listenning, Path.Combine(LC_path, Common.LISTEN + ".pdf"));

                this.InsertProgressBar.Value = 50;

                //Copy RC file 

                string RC_path = Path.Combine(testFullPath, Common.READ + @"\");

                Directory.CreateDirectory(RC_path);

                File.Copy(reading, Path.Combine(RC_path, Common.READ + ".pdf"));

                this.InsertProgressBar.Value = 75;
                //Copy Audio file

                string AU_path = Path.Combine(testFullPath, Common.AUDIO + @"\");

                Directory.CreateDirectory(AU_path);

                File.Copy(audio, Path.Combine(AU_path, Common.AUDIO + ".mp3"));

                this.InsertProgressBar.Value = 100;

                transaction.Commit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.InsertProgressBar.Value = 0;

                transaction.Rollback();

                if (Directory.Exists(testPath))
                {
                    Directory.Delete(testPath, true);
                }
            }
        }
    }
}
