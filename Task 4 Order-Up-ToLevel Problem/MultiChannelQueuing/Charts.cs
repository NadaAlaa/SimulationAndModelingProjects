﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using System.Windows.Forms.DataVisualization.Charting;

namespace orderUpToLevelProblem
{
    public partial class Charts : Form
    {
        public Charts() { InitializeComponent(); }

        private void Charts_Load(object sender, EventArgs e)
        {

            loadChart();
        }
        void LoadWithRanges()
        {

            List<Tuple<double, int>> points = new List<Tuple<double, int>>();
            List<Tuple<double, double, int>> ranges = new List<Tuple<double, double, int>>();
            // <1 Refrigerator,5>
            // <4 Refrigerators,10>
            // <7 Refrigerators,20>
            // <xValue, yValue>
            int numOfRanges = Convert.ToInt32(numericUpDown1.Text);
            for (int i = 0; i < Program.mainForm.simulation.daysList.Count; i++)
            {
                // Fill the list with points
                bool found = false;
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[j].Item1 == Program.mainForm.endingInventoryValues[i])
                    {
                        Tuple<double, int> T = new Tuple<double, int>(points[j].Item1, points[j].Item2 + 1);
                        points[j] = T;
                        found = true;
                    }
                }
                if (found == false)
                {
                    points.Add(new Tuple<double, int>(Program.mainForm.endingInventoryValues[i], 1));
                }
            }
            points.Sort();

            double stepSize = (Math.Abs(points[0].Item1) + Math.Abs(points[points.Count - 1].Item1)) / numOfRanges;
            for (int i = 0; i < numOfRanges; i++)
            {
                if (i == 0)
                {
                    ranges.Add(new Tuple<double, double, int>(points[0].Item1, points[0].Item1 + stepSize, 0));
                }
                else
                {
                    ranges.Add(new Tuple<double, double, int>(ranges[i - 1].Item2, ranges[i - 1].Item2 + stepSize, 0));
                }
            }
            int k = 0;
            for (int i = 0; i < points.Count; )
            {
                if (Math.Round(points[i].Item1) >= Math.Round(ranges[k].Item1) && Math.Round(points[i].Item1) < Math.Round(ranges[k].Item2))
                {
                    Tuple<double, double, int> T = new Tuple<double, double, int>(ranges[k].Item1, ranges[k].Item2, ranges[k].Item3 + points[i].Item2);
                    ranges[k] = T;
                    i++;
                }
                else if (i == points.Count - 1)
                {
                    Tuple<double, double, int> T = new Tuple<double, double, int>(ranges[ranges.Count - 1].Item1, ranges[ranges.Count - 1].Item2, ranges[ranges.Count - 1].Item3 + points[i].Item2);
                    ranges[ranges.Count - 1] = T;
                    break;
                }
                else
                {
                    k++;
                }
            }

            for (int i = 0, j = 0; i < ranges.Count; i++)
            {
                // Add point value.
                chart1.Series["ColumnChart"].Points.AddY(ranges[i].Item3);
                // Add a label to the column.
                chart1.Series["ColumnChart"].Points[j++].AxisLabel = ranges[i].Item1.ToString("0 Units") + " to " + (ranges[i].Item2).ToString("0 Units");
            }
            // Add axis titles.
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Ending Inventory Value (Units)";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count of Repetition (Days)";
        }
        void loadChart()
        {
            
            List<Tuple<double, int>> points = new List<Tuple<double, int>>();
            
            // <1 Refrigerator,5>
            // <4 Refrigerators,10>
            // <7 Refrigerators,20>
            // <xValue, yValue>
            int numOfRanges = 10;
            for (int i = 0; i < Program.mainForm.simulation.daysList.Count; i++)
            {
                // Fill the list with points
                bool found = false;
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[j].Item1 == Program.mainForm.endingInventoryValues[i]) 
                    {
                        Tuple<double, int> T = new Tuple<double, int>(points[j].Item1, points[j].Item2 + 1);
                        points[j] = T;
                        found = true;
                    }
                }
                if (found == false)
                {
                    points.Add(new Tuple<double, int>(Program.mainForm.endingInventoryValues[i], 1));
                }
            }
            points.Sort();

            for (int i = 0, j = 0; i < points.Count; i++)
            {
                // Add point value.
                chart1.Series["ColumnChart"].Points.AddY(points[i].Item2);
                // Add a label to the column.
                chart1.Series["ColumnChart"].Points[j++].AxisLabel = points[i].Item1.ToString("0 Units");
            }
            // Add axis titles.
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Ending Inventory Value (Units)";
            chart1.ChartAreas["ChartArea1"].AxisY.Title = "Count of Repetition (Days)";

            
             
        }
        private void pie_CheckedChanged(object sender, EventArgs e)
        {
            if (chart1.Series["ColumnChart"].ChartType == SeriesChartType.Pie)
                chart1.Series["ColumnChart"].ChartType = SeriesChartType.Column;
            else
                chart1.Series["ColumnChart"].ChartType = SeriesChartType.Pie;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series["ColumnChart"].Points.Clear();
            loadChart();
        }

        private void numericUpDown1_Click(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 1)
            {
                numericUpDown1.Value = 1;
                MessageBox.Show("Please choose a number greater than 0");
                return;
            }
            numericUpDown1.Enabled = false;
            chart1.Series["ColumnChart"].Points.Clear();
            loadChart();
            numericUpDown1.Enabled = true;
        }
    }
}