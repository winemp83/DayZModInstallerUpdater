using DayZModTool.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DayZModTool.Views.Dialogs
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class AddEditMod : Window
    {
        public AddEditMod(ModModel model = null)
        {
            if (model == null)
                model = new ModModel();
            Mod = model;
            InitializeComponent();
        }
        public ModModel Mod
        {
            get { return (ModModel)DataContext; }
            set { DataContext = value; }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box canceled
            DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            // Dialog box accepted
            DialogResult = true;
        }
    }
}
