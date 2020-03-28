﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Bar
    {
        [Test]
        public void Test_Bar_Series()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(xs, ys);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithError()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };
            double[] yErr = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(xs, ys, yErr);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_MutiSeries()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            plt.PlotBar(xs, ys1, xOffset: -.20, barWidth: 0.3, label: "Group A");
            plt.PlotBar(xs, ys2, xOffset: +.20, barWidth: 0.3, label: "Group B");

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot, enableVertical: false);
            plt.Legend(location: ScottPlot.legendLocation.upperRight);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithErrorHorizontal()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };
            double[] yErr = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(xs, ys, yErr, horizontal: true);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_Stacked()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] valuesA = { 1, 2, 3, 2, 1, };
            double[] valuesB = { 3, 3, 2, 1, 3 };

            // to simulate stacking, shift B up by A
            double[] valuesB2 = new double[valuesB.Length];
            for (int i = 0; i < valuesB.Length; i++)
                valuesB2[i] = valuesA[i] + valuesB[i];

            var plt = new ScottPlot.Plot(400, 300);
            plt.PlotBar(xs, valuesB2, label: "Series B"); // plot the uppermost bar first
            plt.PlotBar(xs, valuesA, label: "Series A"); // plot lower bars last (in front)
            plt.Legend(location: ScottPlot.legendLocation.upperRight);
            plt.Axis(y2: 7);
            plt.Title("Stacked Bar Charts");
            TestTools.SaveFig(plt, "", true);
        }
    }
}
