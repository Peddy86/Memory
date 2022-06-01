using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;



namespace Memory
{
    class MemeoryFeld
    {
        //Array für die Karten
        Memory_Karten[] karten;

        bool schummeln;

        //Array für die Namen der Karten
        string[] bilder = { "grafiken/apfel.bmp", "grafiken/birne.bmp", "grafiken/blume.bmp",
                            "grafiken/blume2.bmp", "grafiken/ente.bmp", "grafiken/fisch.bmp",
                            "grafiken/fuchs.bmp", "grafiken/igel.bmp", "grafiken/kaenguruh.bmp",
                            "grafiken/katze.bmp", "grafiken/kuh.bmp", "grafiken/maus1.bmp",
                            "grafiken/maus2.bmp", "grafiken/maus3.bmp", "grafiken/melone.bmp",
                            "grafiken/pilz.bmp", "grafiken/ronny.bmp", "grafiken/schmetterling.bmp",
                            "grafiken/sonne.bmp", "grafiken/wolke.bmp", "grafiken/maus4.bmp"};

        //für die Punkte
        int menschPunkte, computerPunkte;
        Label menschPunkteLabel, computerPunkteLabel;

        int spielstaerke;

        //wie viele Karten sind umgedreht
        int umgedrehteKarten;

        //für das aktuell umgedrehte Paar
        Memory_Karten[] paar;

        //für den aktuellen Spieler
        int spieler;

        //das "Gedächtnis" des Computers, er speichert hier wo das Gegenstück liegt.
        int[,] gemerkteKarten;

        //für das eigentliche Spielfeld
        UniformGrid feld;

        System.Windows.Threading.DispatcherTimer Zeit = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer schummelZeit = new System.Windows.Threading.DispatcherTimer();

        Slider intelligenz = new Slider();

        Button schummelButton = new Button();

        //der Konstructor
        public MemeoryFeld(UniformGrid feld)
        {
            Random zufallsZahl = new Random();

            schummeln = false;

            //zum zählen für die Bilder
            int count = 0;

            //das Array für die Karten erstellen, insgesamt 42 stück
            karten = new Memory_Karten[42];

            spielstaerke = 10;

            //für das Paar
            paar = new Memory_Karten[2];

            //für das Gedächtnis es Speichert jede Karte paarweise die Position im Spielfeld
            gemerkteKarten = new int[2, 21];

            //keiner hat zu Beginn einen Punkt
            menschPunkte = 0;
            computerPunkte = 0;

            //es ist keine Karte umgedreht
            umgedrehteKarten = 0;

            //der Mensch fängt an
            spieler = 0;

            //das Spielfeld setzen
            this.feld = feld;
                        
            //es gibt keine gemerkte Karten
            for (int aussen = 0; aussen < 2; aussen++)
                for (int innen = 0; innen < 21; innen++)
                    gemerkteKarten[aussen, innen] = - 1;
            

            //das eigentliche Spielfeld erstellen
            for(int i = 0; i <= 41; i++)
            {
                //eine neue Karte erzeugen
                karten[i] = new Memory_Karten(bilder[count], count, this);
                                
                //bei jeder 2. Karte kommt auch ein neues Bild
                if ((i + 1) % 2 == 0)
                    count++;
            }

            //Karten Mischen
            for (int i = 0; i <= 41; i++)
            {
                int temp1;
                Memory_Karten temp2;

                //eine Zufällige zahl im Vereich 0 bis 41 erzeugen
                temp1 = zufallsZahl.Next(42);

                //den alten Wert in Sicherheit bringen
                temp2 = karten[temp1];

                //die Werte tauschen
                karten[temp1] = karten[i];
                karten[i] = temp2;
            }

            //die Karten ins Spielfeld bringen
            for(int i = 0; i <= 41; i++)
            {
                //die Position der Karten setzen
                karten[i].setBildPos(i);
                //die Karte hinzufügen
                feld.Children.Add(karten[i]);
            }

            //die Labels für die Punkte
            Label mensch = new Label();
            mensch.Content = "Mensch";
            feld.Children.Add(mensch);
            menschPunkteLabel = new Label();
            menschPunkteLabel.Content = 0;
            feld.Children.Add(menschPunkteLabel);

            Label computer = new Label();
            computer.Content = "Computer";
            feld.Children.Add(computer);
            computerPunkteLabel = new Label();
            computerPunkteLabel.Content = 0;            
            feld.Children.Add(computerPunkteLabel);

            Label labelStaerke = new Label();            
            labelStaerke.Content = "Intelligenz";
            feld.Children.Add(labelStaerke);
            
            intelligenz.Value = 5;
            feld.Children.Add(intelligenz);

            //CSHP Aufgabe 2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<            
            schummelButton.Content = "Cheat";
            schummelButton.Height = 30;
            schummelButton.VerticalAlignment = VerticalAlignment.Top;
            feld.Children.Add(schummelButton);
            if (spieler == 1)
                schummelButton.IsEnabled = false;
            schummelButton.Click += schummeln_Click;

            // das intervall setzen
            Zeit.Interval = TimeSpan.FromMilliseconds(2000);
            //die Methode für das Ereignis zuweisen
            Zeit.Tick += new EventHandler(zeit_tick);

            schummelZeit.Interval = TimeSpan.FromMilliseconds(2000);
            schummelZeit.Tick += new EventHandler(schummelZeit_Tick);

        }

        //CSHP Aufgabe 2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        private void schummeln_Click(object sender, EventArgs e)
        {            
            //gehen wir hier das erste mal durch setzen wir den bool auf false beim 2. mal wieder auf true
            if (schummeln == true)
                schummeln = false;
            else
            {
                schummeln = true;                
            } 

            getSchummeln();
            //wir übergeben alle 42 felder der Methode Schummelkarte in der Klasse Mermory_Karten
            for (int i = 0; i <= 41; i++)
                karten[i].schummelKarte();

            //wir Aktivieren einen 2 Sekunden Timer 
            schummelZeit.Start();
        }

        void schummelZeit_Tick(object sender, EventArgs e)
        {
            //wir Simulieren den KLick auf dem SchummelButton
            schummelButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            schummelZeit.Stop();
        }

        public bool getSchummeln()
        {
            return schummeln;
        }

        //die Methode für den Timer
        private void zeit_tick(object sender, EventArgs e)
        {
            //den Timer anhalten
            Zeit.Stop();
            
            //die karten zurückdrehen
            karteSchliessen();            
        }

        //die Methode übernimmt die wesentliche Steuerung des Spiels, Sie wird beim anklicken einer Karte ausgeführt
        public void karteOeffnen(Memory_Karten karte)
        {
            //zum speichern der ID und Position
            int kartenID, kartenPos;

            
            //die Karten zwischenspeichern
            paar[umgedrehteKarten] = karte;
            

            //die ID und die Position beschaffen
            kartenID = karte.getBildID();
            kartenPos = karte.getBildPos();

            //wenn kein paar, die Karte ins gedächtnis des Computer eintragen
            if ((gemerkteKarten[0, kartenID] == -1))
                gemerkteKarten[0, kartenID] = kartenPos;
            else
                //wenn es einen Eintrag schon gibt und der nicht mit der Aktuellen Position nicht übereinstimmt, 
                //dann haben wir die 2. Karte gefunden
                //Sie wird in der 2. Dimension eingetragen
                if (gemerkteKarten[0, kartenID] != kartenPos)
                    gemerkteKarten[1, kartenID] = kartenPos;

            //umgedrehte karten erhöhen
            umgedrehteKarten++;

            //sind 2 karten umgedreht worden
            if (umgedrehteKarten == 2)
            {
                //den Timer starten
                Zeit.Start();

                //dann prüfen wir ob es ein paar ist
                paarPruefen(kartenID);                
            }

            //haben wir zusammen 21 Paare dann ist das Spiel vorbei<<<<<<<<<<<<<<<<< CSHP Aufgabe 1
            if(computerPunkte + menschPunkte == 21)
            {
                if (computerPunkte < menschPunkte)
                    MessageBox.Show("Das Spiel ist vorbei, der Gewinner ist der Mensch mit einer Punktzahl von " + menschPunkte + "!");
                else
                    MessageBox.Show("Das Spiel ist vorbei, der Gewinner ist der Computer mit einer Punktzahl von " + computerPunkte + "!");

                Application.Current.Shutdown();
                Zeit.Stop();
            }
        }

        //Prüft ob ein paar gefunden wurde
        private void paarPruefen(int kartenID)
        {
            if(paar[0].getBildID() == paar[1].getBildID())
            {
                //die Punkte setzen
                paarGefunden();
                //die Karte aus dem Gedächtnis Löschen
                gemerkteKarten[0, kartenID] = -2;
                gemerkteKarten[1, kartenID] = -2;
            }
        }

        //die Methode setzt die Punkte
        private void paarGefunden()
        {
            //spielt gerade der Mensch
            if(spieler == 0)
            {
                menschPunkte++;
                menschPunkteLabel.Content = menschPunkte.ToString();
            }
            else
            {
                computerPunkte++;
                computerPunkteLabel.Content = computerPunkte.ToString();
            }
        }

        //die Methode nimmt die karte aus dem Spiel oder dreht sie um
        private void karteSchliessen()
        {
            bool raus = false;

            //ist es ein Paar
            if (paar[0].getBildID() == paar[1].getBildID())
                raus = true;

            //wenn es ein paar war nehmen wir die Karte aus dem Spiel, sonst drehen wir sie wieder um
            paar[0].RueckseiteZeigen(raus);
            paar[1].RueckseiteZeigen(raus);

            //es ist keine Karte mehr geöffnet
            umgedrehteKarten = 0;

            //hat der Spieler kein Paar gefunden
            if (raus == false)
                //dann wird der Spieler gewechselt
                spielerWechseln();
            else
                //hat der Computer ein Paar gefunden, dann darf er nochmal
                if (spieler == 1)
                    computerZug();
        }
        
        private void spielerWechseln()
        {
            //wennn der Mensch an der Reihe war kommt jetzt der Computer
            //CSHP der Button wird ausgeblendet und deaktiviert solange der Computer an der Reihe ist<<<<<<<<<<<<<<<<<<<
            if (spieler == 0)
            {
                spieler = 1;
                computerZug();
                schummelButton.IsEnabled = false;
                schummelButton.Opacity = 0;
            }
            else
            {
                spieler = 0;
                schummelButton.IsEnabled = true;
                schummelButton.Opacity = 1;
            }
                            
        }

        // die Methode setzt die Computerzüge um
        private void computerZug()
        {            
            int kartenZaehler = 0;
            int zufall = 0;
            bool treffer = false;
            spielstaerke = 10 - Convert.ToInt32(intelligenz.Value);

            Random zufallsZahl = new Random();

            if(zufallsZahl.Next(spielstaerke) == 0)
            {
                //erst einmal nach einen Paar suchen dazu durchsuchen wir das Array gemrkteKarten, bis iwr in beiden Versionen einen Wert für eine Karte finden
                while ((kartenZaehler < 21) && (treffer == false))
                {
                    //gibt es in beiden einen Wert größer oder gleich 0?
                    if ((gemerkteKarten[0, kartenZaehler] >= 0) && (gemerkteKarten[1, kartenZaehler] >= 0))
                    {
                        //dann haben wir ein Paar
                        treffer = true;
                        //die erste Karte umdrehen durch einen Simulierten klick auf die Karte
                        //karten[gemerkteKarten[0, kartenZaehler]].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

                        //Vorderseite Anzeigen
                        karten[gemerkteKarten[0, kartenZaehler]].vorderseiteAnzeigen();
                        //und die Karte öffnen
                        karteOeffnen(karten[gemerkteKarten[0, kartenZaehler]]);

                        //für die 2. Karte
                        //Vorderseite Anzeigen
                        karten[gemerkteKarten[1, kartenZaehler]].vorderseiteAnzeigen();
                        //und die Karte öffnen
                        karteOeffnen(karten[gemerkteKarten[1, kartenZaehler]]);
                    }
                    kartenZaehler++;
                }
            }

            //wenn wir kein Paar gefunden haben drehen wir 2 zufällige Karten um
            if (treffer == false)
            {
                //solange eine Zufallszahl suchen bis eine Karte gefunden wurd die noch im Spiel ist
                do
                {
                    zufall = zufallsZahl.Next(42);
                }
                while (karten[zufall].getNochImSpiel() == false);

                //die erste Karte Umdrehen, wir Simulieren ein Klick auf einen Button
                //karten[zufall].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                //Vorderseite Anzeigen
                karten[zufall].vorderseiteAnzeigen();
                //und die Karte öffnen
                karteOeffnen(karten[zufall]);

                //für die 2. Karte müssen wir überprüfen ob Sie nicht gerade angezeigt wird
                do
                {
                    zufall = zufallsZahl.Next(42);
                }
                while ((karten[zufall].getNochImSpiel() == false) || (karten[zufall].getUmgedreht() == true));

                //und die 2. Karte umdrehen
                //karten[zufall].RaiseEvent(new RoutedEventArgs (ButtonBase.ClickEvent));
                karten[zufall].vorderseiteAnzeigen();
                //und die Karte öffnen
                karteOeffnen(karten[zufall]);
                
            } 
        }

        //Die methdoe liefert ob die Züge des Menschen erlaubt sind, falls 2 karten umgedreht sind 
        //oder der Computer ist am Zug Liefert die Methode false
        public bool zugErlaubt()
        {
            bool erlaubt = true;

            //zieht der Computer?
            if (spieler == 1)
            {
                erlaubt = false;
            }
                

            //sind 2 Karten umgedreht
            if (umgedrehteKarten == 2)
            {
                erlaubt = false;
            }               

            return erlaubt;
        }
            

    }
}
