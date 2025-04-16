using System;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Zad_2
{
    public partial class Form1 : Form
    {
        private int[] reactionTimeOptyczny = new int[20]; // Tablica do przechowywania czasów reakcji
        private int[] reactionTimeDzwiekowy = new int[20]; // Tablica do przechowywania czasów reakcji
        private int currentTrialOptyczny = 0; // Numer bieżącej próby
        private int currentTrialDzwiekowy = 0; // Numer bieżącej próby
        private Random random = new Random(); // Generator liczb losowych
        private const int FormWidth = 994;
        private const int FormHeight = 645;

        public Form1()
        {
            InitializeComponent();
        }

        
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
     
        private async void TestOptyczny_Click(object sender, EventArgs e)
        {
            Controls.Clear();



            // Tworzymy label
            Label instructionLabel = new Label
            {
                Text = "Kliknij kwadrat,\ngdy ten zmieni kolor",
                Size = new Size(400, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = false
            };

            // Ustawiamy pozycję label na środku formularza, ale przesuwamy w górę
            instructionLabel.Location = new Point(
                (this.ClientSize.Width - instructionLabel.Width) / 2,
                (this.ClientSize.Height - instructionLabel.Height) / 2 - 150  // Przesunięcie w górę o 50px
            );

            // Dodajemy label do formularza
            Controls.Add(instructionLabel);

            // Tworzymy przycisk
            Button startButton = new Button
            {
                Text = "Start",
                Size = new Size(100, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            // Ustawiamy pozycję przycisku tuż pod etykietą
            startButton.Location = new Point(
                (this.ClientSize.Width - startButton.Width) / 2,
                instructionLabel.Bottom + 20 // Przyciski umieszczone poniżej labela z marginesem 20
            );

            // Dodajemy przycisk do formularza
            startButton.Click += StartButton_Click;
            Controls.Add(startButton);

        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            // Ukrywamy przycisk start po jego kliknięciu
            Button startButton = (Button)sender;
            startButton.Visible = false;

            // Tworzymy przycisk kwadratu, który będzie zmieniał kolor
            Button kwadrat = new Button
            {
                Size = new Size(200, 200),  // Ustawiamy rozmiar przycisku na 100x100 (kwadrat)
                Location = new Point(150, 150),  // Ustawiamy lokalizację na środku
                BackColor = Color.Green,  // Kolor początkowy - zielony
                AutoSize = true
            };

            // Wyśrodkowanie przycisku na formularzu
            kwadrat.Location = new Point(
                (FormWidth - kwadrat.Width) / 2,
                (FormHeight - kwadrat.Height) / 2
            );

            // Dodajemy go do formularza
            Controls.Add(kwadrat);

            // Losujemy czas, po którym zmieni kolor (od 1 do 6 sekund)
            int delay = random.Next(1000, 6001);  // Czas w milisekundach
            await Task.Delay(delay);  // Czekamy przez losowy czas

            // Zmieniamy kolor kwadratu na czerwony
            kwadrat.BackColor = Color.Red;

            // Uruchamiamy stoper do mierzenia czasu reakcji
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            // Czekamy na kliknięcie czerwonego kwadratu
            bool clicked = false;
            kwadrat.Click += (s, args) =>
            {
                if (kwadrat.BackColor == Color.Red)
                {
                    // Zatrzymujemy timer po kliknięciu
                    stopwatch.Stop();

                    // Zapisujemy czas reakcji w zmiennej
                    long reactionTime = stopwatch.ElapsedMilliseconds;

                    //wyswietlamy wynik czasowy
                    MessageBox.Show($"Czas reakcji: {reactionTime} ms");

                    // I zapisujemy do tablicy o długości 20
                    reactionTimeOptyczny[currentTrialOptyczny] = (int)reactionTime;

                    // Zwiększamy licznik prób optycznych
                    currentTrialOptyczny++;

                    clicked = true;

                    // Pokazujemy przyciski do powrotu do menu głównego lub powtórzenia testu po jednej próbie
                    ShowEndButtonsOptyczny();
                }
            };

            // Czekamy, aż użytkownik kliknie czerwony kwadrat
            while (!clicked)
            {
                await Task.Delay(10);  // Czekamy 10ms zanim sprawdzimy ponownie
            }
        }

        private void ShowEndButtonsOptyczny()
        {
            // Usuwamy wszystkie kontrolki z formularza
            Controls.Clear();

            // Tworzymy przycisk do powrotu do menu głównego
            Button mainMenuButton = new Button
            {
                Text = "Menu główne",
                Size = new Size(150, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            mainMenuButton.Location = new Point(
                (FormWidth - mainMenuButton.Width) / 2 - 160,  // Przesuwamy w lewo, aby zrobić miejsce na drugi przycisk
                (FormHeight - mainMenuButton.Height) / 2
            );

            mainMenuButton.Click += MainMenuButtonOptyczny_Click;
            Controls.Add(mainMenuButton);

            // Tworzymy przycisk do powtórzenia testu
            Button retryButton = new Button
            {
                Text = "Powtórz test",
                Size = new Size(150, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            // Ustawiamy lokalizację retryButton obok mainMenuButton
            retryButton.Location = new Point(
                (FormWidth - retryButton.Width) / 2 + 160,  // Przesuwamy w prawo, aby były obok siebie
                (FormHeight - retryButton.Height) / 2
            );

            retryButton.Click += RetryButtonOptyczny_Click;
            Controls.Add(retryButton);
        }


        private void RetryButtonOptyczny_Click(object sender, EventArgs e)
        {
            // Wywołanie metody button2_Click, żeby ponownie uruchomić test
            TestOptyczny_Click(sender, e);
        }

        private void MainMenuButtonOptyczny_Click(object sender, EventArgs e)
        {
            // Powrót do menu głównego po teście optycznym
            Controls.Clear();
            InitializeComponent();
        }
       
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private async void TestDzwieku_Click(object sender, EventArgs e)
        {
            Controls.Clear();

            // Etykieta instrukcji
            Label instructionLabel = new Label
            {
                Text = "Kliknij przycisk,\ngdy usłyszysz dźwięk",
                Size = new Size(400, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = false
            };

            instructionLabel.Location = new Point(
            (FormWidth - instructionLabel.Width) / 2,
            (FormHeight - instructionLabel.Height) / 4  // Możesz dostosować wartość 1/4, aby nie była zbyt blisko góry
            );

            Controls.Add(instructionLabel);

            // Przycisk startowy
            Button startButton = new Button
            {
                Text = "Start",
                Size = new Size(100, 50),
                Location = new Point(150, 200),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            startButton.Location = new Point(
            (FormWidth - startButton.Width) / 2,
            (FormHeight - startButton.Height) / 2
            );

            startButton.Click += StartButtonDzwiekowy_Click;
            Controls.Add(startButton);
        }

        private async void StartButtonDzwiekowy_Click(object sender, EventArgs e)
        {
            Button startButton = (Button)sender;
            startButton.Visible = false;

            Button dzwiekowyPrzycisk = new Button
            {
                Text = "Czekaj na dźwięk",
                Size = new Size(150, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            dzwiekowyPrzycisk.Location = new Point(
            (FormWidth - dzwiekowyPrzycisk.Width) / 2,
            (FormHeight - dzwiekowyPrzycisk.Height) / 2
            );

            Controls.Add(dzwiekowyPrzycisk);

            int delay = random.Next(1000, 6001);
            await Task.Delay(delay);

            SystemSounds.Beep.Play(); // Odgrywamy dźwięk

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            bool clicked = false;
            dzwiekowyPrzycisk.Click += (s, args) =>
            {
                stopwatch.Stop();
                long reactionTime = stopwatch.ElapsedMilliseconds;
                MessageBox.Show($"Czas reakcji: {reactionTime} ms");

                reactionTimeDzwiekowy[currentTrialDzwiekowy] = (int)reactionTime;

                // Zwiększ licznik prób dźwiękowych
                currentTrialDzwiekowy++;

                clicked = true;

                // Po każdej próbie wyświetlamy opcję powrotu do menu lub powtórzenia testu
                ShowEndButtonsDzwiekowy();
            };

            while (!clicked)
            {
                await Task.Delay(10);  // Czekamy aż użytkownik kliknie przycisk
            }
        }

        private void ShowEndButtonsDzwiekowy()
        {
            // Usuwamy poprzednie kontrolki
            Controls.Clear();

            // Tworzymy przycisk do powrotu do menu głównego
            Button mainMenuButton = new Button
            {
                Text = "Menu główne",
                Size = new Size(150, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            mainMenuButton.Location = new Point(
            (FormWidth - mainMenuButton.Width) / 2 - 160,  // Przesuwamy w lewo, aby zrobić miejsce na drugi przycisk
            (FormHeight - mainMenuButton.Height) / 2
            );

            mainMenuButton.Click += MainMenuButtonDzwiekowy_Click;
            Controls.Add(mainMenuButton);

            // Tworzymy przycisk do powtórzenia testu dźwiękowego
            Button retryButton = new Button
            {
                Text = "Powtórz test",
                Size = new Size(150, 50),
                Location = new Point(300, 200),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            retryButton.Location = new Point(
            (FormWidth - retryButton.Width) / 2 + 160,  // Przesuwamy w prawo, aby były obok siebie
            (FormHeight - retryButton.Height) / 2
            );

            retryButton.Click += RetryButtonDzwiekowy_Click;
            Controls.Add(retryButton);
        }

        private void RetryButtonDzwiekowy_Click(object sender, EventArgs e)
        {
            // Ponowne wywołanie testu dźwiękowego (przywracamy stan początkowy)
            TestDzwieku_Click(sender, e);
        }

        private void MainMenuButtonDzwiekowy_Click(object sender, EventArgs e)
        {
            // Powrót do menu głównego po teście dźwiękowym
            Controls.Clear();
            InitializeComponent();
        }
    
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Statystyki_Click(object sender, EventArgs e)
        {
            // Usuwamy wszystkie kontrolki z formularza
            Controls.Clear();

            // Tworzymy kontrolkę wykresu (Chart) z dopasowanym rozmiarem
            Chart chart = new Chart
            {
                Size = new Size(600, 350),   // Rozmiar wykresu
            };

            // Wyśrodkowanie wykresu na formularzu
            chart.Location = new Point(
                (FormWidth - chart.Width) / 2,   // Wyśrodkowanie w poziomie
                (FormHeight - chart.Height) / 2 - 50   // Wyśrodkowanie w pionie z uwzględnieniem przerwy na przycisk
            );

            // Dodajemy wykres do formularza
            Controls.Add(chart);

            // Tworzymy obszar wykresu (ChartArea)
            ChartArea chartArea = new ChartArea
            {
                Name = "ChartArea1"
            };
            chart.ChartAreas.Add(chartArea);

            // Tworzymy serię danych dla testu optycznego
            Series seriesOptyczny = new Series
            {
                Name = "Test Optyczny",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column  // Typ wykresu - słupkowy
            };

            // Dodajemy dane do serii optycznej
            for (int i = 0; i < currentTrialOptyczny; i++)
            {
                seriesOptyczny.Points.AddXY(i + 1, reactionTimeOptyczny[i]);
            }

            // Dodajemy serię do wykresu
            chart.Series.Add(seriesOptyczny);

            // Tworzymy serię danych dla testu dźwiękowego
            Series seriesDzwiekowy = new Series
            {
                Name = "Test Dźwiękowy",
                Color = Color.Red,
                ChartType = SeriesChartType.Column  // Typ wykresu - słupkowy
            };

            // Dodajemy dane do serii dźwiękowej
            for (int i = 0; i < currentTrialDzwiekowy; i++)
            {
                seriesDzwiekowy.Points.AddXY(i + 1, reactionTimeDzwiekowy[i]);
            }

            // Dodajemy serię do wykresu
            chart.Series.Add(seriesDzwiekowy);

            // Dodajemy legendę wykresu
            Legend legend = new Legend
            {
                Name = "Legend1"
            };
            chart.Legends.Add(legend);

            // Dodatkowe opcje wykresu (np. tytuły osi, formatowanie)
            chartArea.AxisX.Title = "Numer próby";
            chartArea.AxisY.Title = "Czas reakcji (ms)";

            // Tworzymy przycisk do powrotu do menu głównego
            Button mainMenuButton = new Button
            {
                Text = "Menu główne",
                Size = new Size(150, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                AutoSize = true
            };

            // Wyśrodkowanie przycisku pod wykresem
            mainMenuButton.Location = new Point(
                (FormWidth - mainMenuButton.Width) / 2,  // Wyśrodkowanie przycisku w poziomie
                chart.Bottom + 20  // Pozycja przycisku 20px poniżej wykresu
            );

            mainMenuButton.Click += MainMenuButton_Click;
            Controls.Add(mainMenuButton);
        }


        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            // Powrót do menu głównego po kliknięciu przycisku
            Controls.Clear();
            InitializeComponent();
        }
    
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
       
        private async void szkolenie_click(object sender, EventArgs e)
        {
            Controls.Clear();

            // --- LABEL I PRZYCISK START DO TESTU WZROKOWEGO ---
            Label labelWzrok = new Label
            {
                Text = "Kliknij przycisk Start",
                Font = new Font("Arial", 16),
                AutoSize = true,
                Location = new Point((FormWidth - 200) / 2, (FormHeight / 2) - 80)
            };
            Controls.Add(labelWzrok);

            Button startWzrok = new Button
            {
                Text = "Start",
                Size = new Size(100, 50),
                Font = new Font("Arial", 14),
                Location = new Point((FormWidth - 100) / 2, (FormHeight / 2) - 20)
            };
            Controls.Add(startWzrok);

            var startWzrokClicked = new TaskCompletionSource<bool>();
            startWzrok.Click += (s, e2) => startWzrokClicked.SetResult(true);
            await startWzrokClicked.Task;

            Controls.Clear();

            // --- TEST WZROKOWY ---

            // Tworzymy label
            Label instructionLabel = new Label
            {
                Text = "Kliknij kwadrat,\ngdy ten zmieni kolor",
                Size = new Size(400, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = false
            };

            // Ustawiamy pozycję label na środku formularza, ale przesuwamy w górę
            instructionLabel.Location = new Point(
                (this.ClientSize.Width - instructionLabel.Width) / 2,
                (this.ClientSize.Height - instructionLabel.Height) / 2 - 150  // Przesunięcie w górę o 50px
            );
            Controls.Add(instructionLabel);

            Button kwadrat = new Button
            {
                Size = new Size(200, 200),
                BackColor = Color.Green,
                Location = new Point((FormWidth - 200) / 2, (FormHeight - 200) / 2)
            };
            Controls.Add(kwadrat);

            int delayWzrok = random.Next(1000, 6001);
            await Task.Delay(delayWzrok);
            kwadrat.BackColor = Color.Red;

            var stopwatch1 = new System.Diagnostics.Stopwatch();
            stopwatch1.Start();

            var clicked1 = new TaskCompletionSource<bool>();
            kwadrat.Click += (s, e2) =>
            {
                if (kwadrat.BackColor == Color.Red)
                {
                    stopwatch1.Stop();
                    MessageBox.Show($"Czas reakcji: {stopwatch1.ElapsedMilliseconds} ms");
                    clicked1.SetResult(true);
                }
            };

            await clicked1.Task;

        
            Controls.Clear();

            // --- LABEL I PRZYCISK START DO TESTU DŹWIĘKOWEGO ---
            Label labelDzwiek = new Label
            {
                Text = "Kliknij przycisk, gdy usłyszysz dźwięk",
                Font = new Font("Arial", 16),
                AutoSize = true,
                Location = new Point((FormWidth - 300) / 2, (FormHeight / 2) - 80)
            };
            Controls.Add(labelDzwiek);

            Button startDzwiek = new Button
            {
                Text = "Start",
                Size = new Size(100, 50),
                Font = new Font("Arial", 14),
                Location = new Point((FormWidth - 100) / 2, (FormHeight / 2) - 20)
            };
            Controls.Add(startDzwiek);

            var startDzwiekClicked = new TaskCompletionSource<bool>();
            startDzwiek.Click += (s, e2) => startDzwiekClicked.SetResult(true);
            await startDzwiekClicked.Task;

            Controls.Clear();

            // --- TEST DŹWIĘKOWY ---
            Button dzwiekPrzycisk = new Button
            {
                Text = "Kliknij po dźwięku",
                Size = new Size(200, 80),
                Font = new Font("Arial", 14),
                Location = new Point((FormWidth - 200) / 2, (FormHeight - 80) / 2)
            };
            Controls.Add(dzwiekPrzycisk);

            int delayDzwiek = random.Next(1000, 6001);
            await Task.Delay(delayDzwiek);
            SystemSounds.Beep.Play();

            var stopwatch2 = new System.Diagnostics.Stopwatch();
            stopwatch2.Start();

            var clicked2 = new TaskCompletionSource<bool>();
            dzwiekPrzycisk.Click += (s, e2) =>
            {
                stopwatch2.Stop();
                MessageBox.Show($"Czas reakcji: {stopwatch2.ElapsedMilliseconds} ms");
                clicked2.SetResult(true);
            };
            await clicked2.Task;

            ShowEndButtonsSzkolenie();
        }


        private void ShowEndButtonsSzkolenie()
        {
            Controls.Clear();

            Button mainMenuButton = new Button
            {
                Text = "Menu główne",
                Size = new Size(150, 50),
                Font = new Font("Arial", 16)
            };
            mainMenuButton.Location = new Point(FormWidth / 2 - 160, FormHeight / 2);
            mainMenuButton.Click += MainMenuButtonSzkolenie_Click;
            Controls.Add(mainMenuButton);

            Button retryButton = new Button
            {
                Text = "Powtórz test",
                Size = new Size(150, 50),
                Font = new Font("Arial", 16)
            };

            retryButton.Location = new Point(FormWidth / 2 + 10, FormHeight / 2);
            retryButton.Click += RetryButtonSzkolenie_Click;
            Controls.Add(retryButton);
        }

        private void RetryButtonSzkolenie_Click(object sender, EventArgs e)
        {
            szkolenie_click(sender, e);
        }

        private void MainMenuButtonSzkolenie_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            InitializeComponent();
        }

    }
}
