using bntu.vsrpp.DSleptsov.lab2.api.exceptions;
using bntu.vsrpp.DSleptsov.lab2.api.loaders;
using bntu.vsrpp.DSleptsov.lab2.converter.exceptions;
using bntu.vsrpp.DSleptsov.lab2.converter;
using bntu.vsrpp.DSleptsov.lab2.chart;
using System.Windows.Forms;
using System.Data;

namespace bntu.vsrpp.DSleptsov.lab2
{
    public partial class MainForm : Form
    {
        private DateTime endPeriod = DateTime.Now;
        private DateTime startPeriod;
        public MainForm()
        {
            InitializeComponent();
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            //await FillWithCurrency();
            await LoadRates();
        }

        private async Task LoadRates()
        {
            this.comboBox1.Items.Clear();
            this.comboBox2.Items.Clear();
            this.comboBox3.Items.Clear();
            this.comboBox4.Items.Clear();
            this.comboBox5.Items.Clear();

            try
            {
                await RatesLoader.loadRate();
                foreach (var rate in RatesLoader.RATES)
                {
                    if (!this.comboBox1.Items.Contains(rate.Cur_Abbreviation))
                    {
                        this.comboBox1.Items.Add(rate.Cur_Abbreviation);
                        this.comboBox2.Items.Add(rate.Cur_Abbreviation);
                        this.comboBox3.Items.Add(rate.Cur_Abbreviation);
                        this.comboBox4.Items.Add(rate.Cur_Abbreviation);
                        this.comboBox5.Items.Add(rate.Cur_Abbreviation);
                    }
                }
            }
            catch (RatesLoadException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void DrawGraphic(DateTime start, DateTime end)
        {
            if (ChartHandler.CheckDates(start, end))
            {
                ChartHandler.fromDate = start;
                ChartHandler.toDate = end;
                ChartHandler.rate = RatesLoader.RATES.Where(r => r.Cur_Abbreviation == comboBox5.SelectedItem.ToString()).FirstOrDefault();
                textBox7.Text = start.ToString("dd/MM/yyyy");
                textBox8.Text = end.ToString("dd/MM/yyyy");
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                await ChartHandler.DrawChart(pictureBox1);
                label20.Text = ChartHandler.FindDateWithMin().ToString("dd/MM/yyyy");
                label21.Text = ChartHandler.FindAverageOfThePeriod().ToString() + " " + ChartHandler.rate.Cur_Abbreviation;
                label22.Text = ChartHandler.FindDateWithMax().ToString("dd/MM/yyyy");
            }
            else
            {
                MessageBox.Show("From date cannot be more than to date.", "Warning");
            }
        }
        private async void ChartButton_Click(object sender, EventArgs e)
        {
            DrawGraphic(monthCalendar1.SelectionRange.Start, monthCalendar2.SelectionRange.Start);
        }

        private void BtoCConvert_Click(object sender, EventArgs e)
        {
            try
            {
                textBox3.Text = CurrencyConverter.ConvertFromBYN(comboBox1.SelectedItem.ToString(), textBox4.Text);
            }
            catch (CurrencyConvertException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CtoBConvert_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = CurrencyConverter.ConvertToBYN(comboBox2.SelectedItem.ToString(), textBox1.Text);
            }
            catch (CurrencyConvertException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CtoCConvert_Click(object sender, EventArgs e)
        {
            try
            {
                textBox5.Text = CurrencyConverter.ConvertCurrency(
                    comboBox3.SelectedItem.ToString(),
                    comboBox4.SelectedItem.ToString(),
                    textBox6.Text);
            }
            catch (CurrencyConvertException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtoCClear_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "";
        }

        private void CtoBClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox2.Text = "";
        }

        private void CtoCClear_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox6.Text = "";
            comboBox3.Text = "";
            comboBox4.Text = "";
        }

        private void ClearChartSpaceButton_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox8.Text = "";
            comboBox5.Text = "";
            pictureBox1.Image = null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            startPeriod = endPeriod.AddDays(-30);
            DrawGraphic(startPeriod, endPeriod);
            startPeriod = endPeriod;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            startPeriod = endPeriod.AddDays(-60);
            DrawGraphic(startPeriod, endPeriod);
            startPeriod = endPeriod;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            startPeriod = endPeriod.AddDays(-182);
            DrawGraphic(startPeriod, endPeriod);
            startPeriod = endPeriod;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            startPeriod = endPeriod.AddDays(-365);
            DrawGraphic(startPeriod, endPeriod);
            startPeriod = endPeriod;
        }
        private void ShowExchangeRate()
        {
            int count = 0;
            if (RatesLoader.RATES != null)
            {
                foreach (var rate in RatesLoader.RATES)
                {
                    count++;
                    dataGridView1.Rows.Add(count, rate.Cur_Abbreviation, rate.Cur_Name, rate.Cur_Scale, rate.Cur_OfficialRate);
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            ShowExchangeRate();
        }
    }
}