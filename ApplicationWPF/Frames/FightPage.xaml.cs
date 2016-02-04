﻿using System;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApplicationWPF.Frames
{
    /// <summary>
    /// Logique d'interaction pour FightPage.xaml
    /// </summary>
    public partial class FightPage : Page, IFrameNavigator
    {

        public event EventHandler<FrameChangedEventArgs> m_changeFrame;
        public string m_nextFrame;

        public FightPage()
        {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, EventArgs args)
        {
            BusinessLayer.JediTournamentManager jtm = new BusinessLayer.JediTournamentManager();

            // Gestion de la Partie
            EntitiesLayer.Partie game = BusinessLayer.PartieManager.getCurrentGame();
            ViewModel.Partie.PartieViewModel gvm = new ViewModel.Partie.PartieViewModel(game);
            Concurent1.DataContext = gvm.Current_match.Jedi1;
            Concurent2.DataContext = gvm.Current_match.Jedi2;

            game.Choice_j1 = EntitiesLayer.EShifumi.Aucun;
            game.Choice_j2 = EntitiesLayer.EShifumi.Aucun;

            game.Pari_j1 = 0;
            game.Pari_j2 = 0;

            PhaseTournoi.Text = gvm.Current_match.PhaseTournoi.ToString();

            Affiche1.Visibility = Visibility.Hidden;
            Affiche2.Visibility = Visibility.Hidden;
        }

        public string NextFrame
        {
            get { return m_nextFrame; }
            set { m_nextFrame = value; }
        }

        public EventHandler<FrameChangedEventArgs> OnFrameChanged
        {
            get { return m_changeFrame; }
            set { m_changeFrame = value; }
        }



        private void ButtonBack_Event(object sender, EventArgs e)
        {
            string nextFrame = "Frames/MainMenu.xaml";
            OnFrameChanged(this, new FrameChangedEventArgs(nextFrame));
        }



        private void ButtonStart_Event(object sender, EventArgs e)
        {
            if(BusinessLayer.PartieManager.getCurrentGame().Current_match.JediVainqueur == null)
            {           
                if (((BusinessLayer.PartieManager.getCurrentGame().J1 != null && BusinessLayer.PartieManager.getCurrentGame().Current_match.Jedi1.Nom == BusinessLayer.PartieManager.getCurrentGame().Jedi_j1.Nom) 
                    || (BusinessLayer.PartieManager.getCurrentGame().J2 != null && BusinessLayer.PartieManager.getCurrentGame().Current_match.Jedi1.Nom == BusinessLayer.PartieManager.getCurrentGame().Jedi_j2.Nom) )
                    && (BusinessLayer.PartieManager.getCurrentGame().Mode.Equals(EntitiesLayer.Mode.Solo) || BusinessLayer.PartieManager.getCurrentGame().Mode.Equals(EntitiesLayer.Mode.Multi)))
                {
                    Affiche1.Visibility = Visibility.Visible;
                }
                else
                {
                    BusinessLayer.PartieManager.getCurrentGame().Choice_j1 = BusinessLayer.PartieManager.getIAChoice();
                }

                if (((BusinessLayer.PartieManager.getCurrentGame().J1 != null && BusinessLayer.PartieManager.getCurrentGame().Current_match.Jedi2.Nom == BusinessLayer.PartieManager.getCurrentGame().Jedi_j1.Nom)
                   || (BusinessLayer.PartieManager.getCurrentGame().J2 != null && BusinessLayer.PartieManager.getCurrentGame().Current_match.Jedi2.Nom == BusinessLayer.PartieManager.getCurrentGame().Jedi_j2.Nom))
                   && (BusinessLayer.PartieManager.getCurrentGame().Mode.Equals(EntitiesLayer.Mode.Solo) || BusinessLayer.PartieManager.getCurrentGame().Mode.Equals(EntitiesLayer.Mode.Multi)))
                {
                    Affiche2.Visibility = Visibility.Visible;
                }
                else
                {
                    BusinessLayer.PartieManager.getCurrentGame().Choice_j2 = BusinessLayer.PartieManager.getIAChoice();
                }

                resolve();
            }
        }

        private void ButtonNext_Event(object sender, EventArgs e)
        {
            if(BusinessLayer.PartieManager.getCurrentGame().Current_match.JediVainqueur != null) {
                BusinessLayer.PartieManager.nextMatch();
                this.NavigationService.Refresh();
            }
        }

        private void ButtonPierre_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j1 = EntitiesLayer.EShifumi.Pierre;
            resolve();
        }

        private void ButtonPapier_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j1 = EntitiesLayer.EShifumi.Papier;
            resolve();
        }

        private void ButtonCiseau_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j1 = EntitiesLayer.EShifumi.Ciseau;
            resolve();
        }


        private void ButtonPierre2_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j2 = EntitiesLayer.EShifumi.Pierre;
            resolve();
        }

        private void ButtonPapier2_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j2 = EntitiesLayer.EShifumi.Papier;
            resolve();
        }

        private void ButtonCiseau2_Event(object sender, EventArgs e)
        {
            BusinessLayer.PartieManager.getCurrentGame().Choice_j2 = EntitiesLayer.EShifumi.Ciseau;
            resolve();
        }

        private void resolve()
        {
            if (BusinessLayer.PartieManager.getCurrentGame().Current_match.JediVainqueur == null)
            {
                if (BusinessLayer.PartieManager.resolve())
                {
                    DropShadowEffect o = new DropShadowEffect();
                    o.Direction = 0;
                    o.Color = Colors.Blue;
                    o.ShadowDepth = 0;
                    o.BlurRadius = 10;

                    if (BusinessLayer.PartieManager.getCurrentGame().Current_match.JediVainqueur == BusinessLayer.PartieManager.getCurrentGame().Current_match.Jedi1)
                    {
                        this.Concurent1Img.Effect = o;
                    }
                    else
                    {
                        this.Concurent2Img.Effect = o;
                    }
                }
            }
        }


        private void Page_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
