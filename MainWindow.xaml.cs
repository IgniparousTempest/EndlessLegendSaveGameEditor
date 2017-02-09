using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.Compression;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace EndlessLegenedSaveEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SaveGame saveGame;
        private List<EmpirePage> empirePages = new List<EmpirePage>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonSelectSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = openFileDialog();

            if (dlg.ShowDialog() == true)
            {
                try {
                    saveGame = new SaveGame(dlg.FileName);
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("The file is not an Endless Legends save file :(", "Gebruiker is 'n idioot", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                removeEmpirePages();

                empirePages = createEmpirePages(saveGame);

                showGameStats(saveGame, empirePages);

                buttonSelectSaveEdits.IsEnabled = true;
            }
        }

        private void buttonSelectSaveEdits_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < saveGame.MajorEmpires.Count; i++)
            {
                updateXmlWithEmpireStats(empirePages[i], saveGame.MajorEmpires[i]);
            }

            saveGame.UpdateSaveFile();

            MessageBox.Show("Save was super successful", "Great success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Removes empire pages from their parent container.
        /// Side effects: deletes list elements in place.
        /// </summary>
        private void removeEmpirePages()
        {
            empiresPanel.Children.Clear();
            empirePages.Clear();
        }

        private List<EmpirePage> createEmpirePages(SaveGame game)
        {
            List<EmpirePage> pages = new List<EmpirePage>();
            EmpirePage newPage;
            Frame newFrame;
            for (int i = 0; i < game.MajorEmpires.Count; i++)
            {
                newPage = new EmpirePage();
                newFrame = new Frame();
                newFrame.Width = 310;
                newFrame.Height = 150;
                newFrame.Margin = new Thickness(4);
                newFrame.Navigate(newPage);
                empiresPanel.Children.Add(newFrame);
                pages.Add(newPage);
            }

            return pages;
        }

        private void setEmpireStats(EmpirePage empirePage, EmpireStats empireStats)
        {
            empirePage.labelFactionName.Content = empireStats.EmpireName;
            empirePage.imageFaction.Source = new BitmapImage(empireStats.EmpirePortraitPath);
            empirePage.textBoxScience.Text = empireStats.EmpireResearchStock;
            empirePage.textBoxDust.Text = empireStats.BankAccount;
            empirePage.textBoxInfluence.Text = empireStats.EmpirePointStock;
            empirePage.textBoxGlassteel.Text = empireStats.Strategic1Stock;
            empirePage.textBoxTitanium.Text = empireStats.Strategic2Stock;
            empirePage.textBoxAdamantium.Text = empireStats.Strategic3Stock;
            empirePage.textBoxPalladiun.Text = empireStats.Strategic4Stock;
            empirePage.textBoxHyperium.Text = empireStats.Strategic5Stock;
            empirePage.textBoxMithrite.Text = empireStats.Strategic6Stock;
        }

        private void updateXmlWithEmpireStats(EmpirePage empirePage, EmpireStats empireStats)
        {
            empireStats.EmpireResearchStock = empirePage.textBoxScience.Text;
            empireStats.BankAccount = empirePage.textBoxDust.Text;
            empireStats.EmpirePointStock = empirePage.textBoxInfluence.Text;
            empireStats.Strategic1Stock = empirePage.textBoxGlassteel.Text;
            empireStats.Strategic2Stock = empirePage.textBoxTitanium.Text;
            empireStats.Strategic3Stock = empirePage.textBoxAdamantium.Text;
            empireStats.Strategic4Stock = empirePage.textBoxPalladiun.Text;
            empireStats.Strategic5Stock = empirePage.textBoxHyperium.Text;
            empireStats.Strategic6Stock = empirePage.textBoxMithrite.Text;
        }

        private void showGameStats(SaveGame game, List<EmpirePage> empirePages)
        {
            for (int i = 0; i < game.MajorEmpires.Count; i++)
            {
                setEmpireStats(empirePages[i], game.MajorEmpires[i]);
            }
        }

        private OpenFileDialog openFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"%USERNAME%\Documents\Endless Legend\Save Files";
            ofd.DefaultExt = "*.zip|zip save game (*.zip)";
            return ofd;
        }

        private void buttonWebsite_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://igniparoustempest.github.io");
        }
    }
}
