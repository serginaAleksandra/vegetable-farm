using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        private int money = 100;
        private int seconds, minutes, hours = 0;
        private int timeInterval = 100;
        private int plantCost = 2;
        private int harvestImmatureCost = 3; 
        private int harvestMatureCost = 5;
        private int harvestOvergrownCost = 1;

        public Form1() 
        {
            InitializeComponent();
            SetupInitialValues();
            foreach (CheckBox cb in panel1.Controls)
                field.Add(cb, new Cell());
        }

        private void SetupInitialValues()
        {
            timerLabel.Text = "0";
            moneyLabel.Text = money.ToString();
            timer1.Interval = timeInterval;
            timerIntervalTrackBar.Value = timeInterval;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer timer = (sender as Timer);
            seconds++;
            if (seconds == 59)
            {
                minutes++;
                seconds = 0;
            }
            if (minutes == 59)
            {
                hours++;
                minutes = 0;
            }

            timerLabel.Text = string.Format("{0:##}:{1:##}:{2:##}", hours, minutes, seconds);
            foreach (CheckBox cb in panel1.Controls)
                NextStep(cb);
        }

        private void Plant(CheckBox cb)
        {
            field[cb].Plant();
            UpdateMoney(cb);
            UpdateBox(cb);
        }

        private void Harvest(CheckBox cb)
        {
            UpdateMoney(cb);
            field[cb].Harvest();
            UpdateBox(cb);
        }

        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            TrackBar tb = (sender as TrackBar);
            timer1.Interval = tb.Value;
            timerIntervalLabel.Text = tb.Value.ToString();
        }

        private void UpdateMoney(CheckBox cb)
        {
            switch (field[cb].state)
            {
                case CellState.Planted:
                    money -= plantCost;
                    break;
                case CellState.Green:
                    break;
                case CellState.Immature:
                    money += harvestImmatureCost;
                    break;
                case CellState.Mature:
                    money += harvestMatureCost;
                    break;
                case CellState.Overgrown:
                    money -= harvestOvergrownCost;
                    break;
            }
            moneyLabel.Text = money.ToString();
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted: c = Color.Black;
                    break;
                case CellState.Green: c = Color.Green;
                    break;
                case CellState.Immature: c = Color.Yellow;
                    break;
                case CellState.Mature: c = Color.Red;
                    break;
                case CellState.Overgrown: c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
        }
    }

    enum CellState
    {
        Empty,
        Planted,
        Green,
        Immature,
        Mature,
        Overgrown
    }

    class Cell
    {
        public CellState state = CellState.Empty;
        public int progress = 0;

        private const int prPlanted = 20;
        private const int prGreen = 100;
        private const int prImmature = 120;
        private const int prMature = 140;

        public void Plant()
        {
            state = CellState.Planted; 
            progress = 1;
        }

        public void Harvest()
        {
            state = CellState.Empty;
            progress = 0;
        }

        public void NextStep()
        {
            if ((state != CellState.Empty) && (state != CellState.Overgrown))
            {
                progress++;
                if (progress < prPlanted) state = CellState.Planted;
                else if (progress < prGreen) state = CellState.Green;
                else if (progress < prImmature) state = CellState.Immature;
                else if (progress < prMature) state = CellState.Mature;
                else state = CellState.Overgrown;
            }
        }
    }
}
