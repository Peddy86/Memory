using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Memory
{
    //die KLasse für eine Karte des Spiels Sie erbt von Button
    class Memory_Karten : Button
    {
        Image bildRausgenommen = new Image();

        //für das Spielfeld für die Karte
        MemeoryFeld spiel;

        //die Felder. Eine eindeutige ID zur Identifizierung des Bildes
        int bildID;
        //Bild für Vorder- und Rückseite
        Image vorderseite, rueckseite;

        //wo liegt die Karte im Spielfeld?
        int bildPos;

        //ist die Karte umgedreht?
        bool umgedreht;

        //ist die Karte noch im Spiel?
        bool nochImSpiel;

        //der Konstructor er setzt die Größe, die Bilder und die Position
        public Memory_Karten(string vorne, int bildID, MemeoryFeld spiel)
        {
            //mit dem Spielfeld verbinden
            this.spiel = spiel;

            //die Vorderseite, der Dateiname des Bildes wird an den Konstructor übergeben
            vorderseite = new Image();
            vorderseite.Source = new BitmapImage(new Uri(vorne, UriKind.Relative));
            //die Rückseite wird festegesetzt
            rueckseite = new Image();
            rueckseite.Source = new BitmapImage(new Uri("grafiken/verdeckt.bmp", UriKind.Relative));

            //die Eigenschaften setzen
            Content = rueckseite;
            // die Bild ID
            this.bildID = bildID;
            //die Karte ist erstmal noch umgedreht und noch auf dem Feld
            umgedreht = false;
            nochImSpiel = true;
            //die Methode mit dem Ereignis verbinden
            Click += new RoutedEventHandler(ButtonClick);    
        }

        //die Methode für das Anklicken
        private void ButtonClick(object sender,RoutedEventArgs e)
        {
            //ist die Karte überhaupt noch im Spiel
            if (nochImSpiel == false || spiel.zugErlaubt() == false)
                return;

            //wenn die Rückseite zu sehen ist, die Vorderseite anzeigen lassen
            if (umgedreht == false)
            {
                vorderseiteAnzeigen();

                //die Methode KarteOeffnen() im Spielfeld aufrufen, die Karte übergeben
                spiel.karteOeffnen(this);
            }                  
        }

        //die Methode zeigt die Vorderseite der Karte an
        public void vorderseiteAnzeigen()
        {
            Content = vorderseite;
            umgedreht = true;
        }

        //die Methode zeigt die Rückseite der Karte an
        public void RueckseiteZeigen(bool rausnehmen)
        {
            //soll die Karte komplett aus dem Spiel genommen werden
            if (rausnehmen == true)
            {
                //das Bild aufgedeckt zeigen und die Karte aus dem Spiel nehmen                
                bildRausgenommen.Source = new BitmapImage(new Uri("grafiken/aufgedeckt.bmp", UriKind.Relative));

                Content = bildRausgenommen;
                nochImSpiel = false;
            }
            else
            {
                //sonst nur die Rückseite anzeigen
                Content = rueckseite;
                umgedreht = false;
            }
        }

        //die Methode liefert die Bild-ID einer Karte
        public void setBildPos(int bildPos)
        {
            this.bildPos = bildPos;
        }

        public int getBildID()
        {
            return bildID;
        }

        public int getBildPos()
        {
            return bildPos;
        }

        //die Methoden Liefern den Wert der Felder umgedreht und nochImSpiel
        public bool getUmgedreht()
        {
            return umgedreht;
        }

        public bool getNochImSpiel()
        {
            return nochImSpiel;
        }

        public void schummelKarte()
        {
            //ist die Karte bereits rausgenommen verlassen wir die Methode sofort wieder
            if (Content == bildRausgenommen)
                    return;
            if (spiel.getSchummeln() == true)
                Content = vorderseite;            
            else
            {
                Content = rueckseite;
            }                
        }
    }













    
}
