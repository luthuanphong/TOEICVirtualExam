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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TOEICVirtualExam
{
    /// <summary>
    /// Interaction logic for Answer.xaml
    /// </summary>
    public partial class Answer : UserControl
    {
        private Dictionary<int, RadioButton> choices = new Dictionary<int, RadioButton>();

        private int _questionNumber;

        public int QuestionNumber
        {
            get
            {
                return this._questionNumber;
            }
        }

        public Answer(int questionNumber = -1)
        {
            InitializeComponent();
            this._questionNumber = questionNumber;
            this.QuestionNumberTextBlock.Text = this._questionNumber.ToString() + ".";

            this.A.GroupName = this._questionNumber.ToString();
            this.B.GroupName = this._questionNumber.ToString();
            this.C.GroupName = this._questionNumber.ToString();
            this.D.GroupName = this._questionNumber.ToString();

            this.choices.Add(0, this.A);
            this.choices.Add(1, this.B);
            this.choices.Add(2, this.C);
            this.choices.Add(3, this.D);
        }

        public int GetChoice()
        {
            for(int i = 0; i < 4; i++)
            {
                if(this.choices[i].IsChecked == true)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
