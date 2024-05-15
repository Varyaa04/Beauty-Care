﻿using Beauty_Care.goods;
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

namespace Beauty_Care
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppConnect.modeldb = new Entities();
            AppFrame.frameMain = FrmMain;

            FrmMain.Navigate(new goods.beautyGoodsPages());
        }

        private void FrmMain_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
