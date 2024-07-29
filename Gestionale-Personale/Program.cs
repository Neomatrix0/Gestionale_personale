﻿  using Newtonsoft.Json;


class Program
{

    // crea cartella dipendenti dove verranno messi i file json per ogni dipendente
    static string directoryPath = @"dipendenti/";

    static void Main(string[] args)

    {
        // se la cartella non esiste la crea

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        Console.WriteLine("Benvenuto nel programma di gestione del personale.");
        int opzione;

        do
        {
            Console.Clear();
            Console.WriteLine("1. Inserisci dipendente");
            Console.WriteLine("2. Visualizza dipendenti");
            Console.WriteLine("3. Cerca dipendente");
            Console.WriteLine("4. Modifica dipendente");
            Console.WriteLine("5. Rimuovi dipendente");
           //  Console.WriteLine("6. Tasso di assenteismo");
           // Console.WriteLine("7. Indicatore di performance");
          //  Console.WriteLine("8. Ordina per stipendio");
            Console.WriteLine("9. Esci\n");

            // scelta del tipo di azione da svolgere

            opzione = Convert.ToInt32(Console.ReadLine());
            switch (opzione)
            {
                case 1:
                    InserisciDipendente();
                    break;
                case 2:
                    VisualizzaDipendenti();
                    break;
                case 3:
                    CercaDipendente();
                    break;
                case 4:
                    ModificaDipendente();
                    break;
                case 5:
                    RimuoviDipendente();
                    break;
                case 6:
                  //  TassoDiAssenteismo();
                    break;
                case 7:

                   // ValutazionePerformance();
                    break;
                case 8:

                  //  SortStipendio();
                    break;
                case 9:
                    Console.WriteLine("Il programma verrà chiuso. Attendere prego.");
                    break;
                default:
                    Console.WriteLine("Errore di scelta: Prego riprovare");
                    break;
            }

            // se viene scelta l'opzione 9 il programma si chiude altrimenti prosegue

            if (opzione != 9)
            {
                Console.WriteLine("\nPremere un tasto per proseguire");
                Console.ReadKey();
            }

        } while (opzione != 9);
    }

    static void InserisciDipendente()
    {
        do
        {
            try
            {
                Console.WriteLine("Inserisci nome, cognome, data di nascita DD/MM/YYYY,mansione, stipendio,voto performance da 1 a 100 ,giorni di assenze,mail,separate da virgola");

                // accetta l'input dei dati da console
                string? inserimento = Console.ReadLine();

                // permette l'inserimento di più valori divisi dalla virgola

                string?[] dati = inserimento.Split(',')!;

                // formattazione data di nascita
                //ParseExact permette di specificare esattamente come vogliamo il formato  della data converte da stringa a oggetto Datetime

                DateTime dataDiNascita = DateTime.ParseExact(dati[2].Trim(), "dd/MM/yyyy", null);

                //viene riconvertito in stringa
                string dataDiNascitaFormatted = dataDiNascita.ToString("dd/MM/yyyy");

                //  creazione di un oggetto dipendente contenente i dati richiesti dall'applicazione
                var dipendente = new
                {
                    Nome = dati[0].Trim(),
                    Cognome = dati[1].Trim(),
                    DataDiNascita = dataDiNascitaFormatted, //DateTime.Parse(dati[2].Trim()),
                    Mansione = dati[3].Trim(),
                    Stipendio = Convert.ToDecimal(dati[4].Trim()),
                    Performance = Convert.ToInt32(dati[5].Trim()),
                    Assenze = Convert.ToInt32(dati[6].Trim()),
                    Mail = dati[7].Trim()
                };

                // serializza l'oggetto in una stringa Json e lo indenta per renderlo più leggibile

                string jsonString = JsonConvert.SerializeObject(dipendente, Formatting.Indented);

                // Path.Combine concatena il path della cartella dipendenti al path dei file json di ogni dipendente
                string filePath = Path.Combine(directoryPath, $"{dipendente.Nome}_{dipendente.Cognome}.json");

                //scrive il file

                File.WriteAllText(filePath, jsonString);

            }
            catch (Exception e)
            {
                Console.WriteLine($"ERRORE INSERIMENTO DATI: {e.Message}");     // messaggio eccezione
                Console.WriteLine($"CODICE ERRORE:{e.HResult}");                //codice numerico eccezione
                return;
            }

            // se si svuole terminare l'immissione della registrazione del dipendente basta digitare n

            Console.WriteLine("Vuoi inserire un altro dipendente? (s/n)");
            string? risposta = Console.ReadLine().Trim().ToLower();
            if (risposta == "n")
            {
                break;
            }
        } while (true);
    }

    // creazione dei metodi per ogni singola funzionalità
    static void VisualizzaDipendenti()
    {
        // analizza tutti i file con estensione .json dentro la directoryPath(cartella dipendenti)
        var files = Directory.GetFiles(directoryPath, "*.json");  //

        // verifica se c'è almeno un file per eseguire il codice
        if (files.Length > 0)
        {
            Console.WriteLine("Lista dipendenti completa con tutti i dati:\n");

            // stampa i dati di tutti i dipendenti presi dai json

            foreach (var file in files)
            {

                StampaDati(file);

            }
        }
        else
        {
            Console.WriteLine("Nessun dipendente nel database.");
        }
    }

    // metodo per cercare il dipendente
    static void CercaDipendente()
    {
        try
        {
            Console.WriteLine("\nInserisci nome e cognome del dipendente che vuoi cercare separati da virgola");
            var inserisciNome = Console.ReadLine();

            // permette l'inserimento di molteplici input separati dalla virgola quindi permette un array di stringhe con nome cognome
            var nomi = inserisciNome.Split(',');

            // il dipendente deve essere cercato inserendo nome,cognome se vengono inseriti più valori viene gestito l'errore


            if (nomi.Length != 2)
            {
                throw new FormatException("L'input deve contenere esattamente due valori separati da virgola: nome e cognome.");
              
            }

            // creato variabili per associarvi il valore di nome e cognome relativi all'array nomi

            string nome = nomi[0].Trim();
            string cognome = nomi[1].Trim();

            // nome e cognome di ogni dipendente diventerenno il rispettivo nome dei file json 
            string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

            // verifica se un file json esiste

            if (File.Exists(filePath))
            {

                StampaDati(filePath);
            }
            else
            {
                Console.WriteLine("Dipendente non trovato");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Errore non trattato: {e.Message}");
            Console.WriteLine($"CODICE ERRORE: {e.HResult}");
        }
    }
    //cerca dipendente per nome e cognome e poi modifica le caratteristiche del dipendente a scelta

    static void ModificaDipendente()
    {
        Console.WriteLine("\nInserisci nome e cognome del dipendente che vuoi modificare separati da virgola");
        var inserisciNome = Console.ReadLine();
        var nomi = inserisciNome.Split(',');

        // il dipendente va cercato solo inserendo nome,cognome

        if (nomi.Length != 2)
        {
            Console.WriteLine("Nomi non validi");
            return;
        }

        // Trim() rimuove gli spazi vuoti dal nome e cognome

        string nome = nomi[0].Trim();
        string cognome = nomi[1].Trim();
        string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

        if (File.Exists(filePath))
        {
            string jsonRead = File.ReadAllText(filePath);
            var lavoratore = JsonConvert.DeserializeObject<dynamic>(jsonRead);

            // sottomenu per scegliere il valore da modificare

            Console.WriteLine("1. Cambia nome");
            Console.WriteLine("2. Cambia cognome");
            Console.WriteLine("3. Cambia data di nascita sempre nel formato DD/MM/YY");
            Console.WriteLine("4. Cambia mansione");
            Console.WriteLine("5. Cambia stipendio");
            Console.WriteLine("6. Cambia punteggio performance");
            Console.WriteLine("7. Cambia giorni di assenze");
            Console.WriteLine("8. Cambia mail");
            Console.WriteLine("9. Esci");

            // scelta del valore da cambiare

            int inserimento = Convert.ToInt32(Console.ReadLine());



            switch (inserimento)
            {


                case 1:

                    Console.WriteLine("Inserici il nuovo nome");
                    lavoratore.Nome = Console.ReadLine().Trim();



                    break;

                case 2:
                    Console.WriteLine("Inserici il nuovo cognome");
                    lavoratore.Cognome = Console.ReadLine().Trim();


                    break;

                case 3:
                    Console.WriteLine("Inserisci nuova data di nascita");
                    lavoratore.DataDiNascita = DateTime.ParseExact(Console.ReadLine().Trim(), "dd/MM/yyyy", null).ToString("dd/MM/yyyy");

                    break;

                case 4:
                    Console.WriteLine("Inserisci nuova mansione");
                    lavoratore.Mansione = Console.ReadLine().Trim();


                    break;

                case 5:
                    Console.WriteLine("Inserisci nuovo stipendio");
                    lavoratore.Stipendio = Convert.ToDecimal(Console.ReadLine());

                    break;

                case 6:
                    Console.WriteLine("Inserisci nuovo punteggio performance");
                    lavoratore.Performance = Convert.ToInt32(Console.ReadLine());

                    break;

                case 7:
                    Console.WriteLine("Modifica giorni di assenze");
                    lavoratore.Assenze = Convert.ToInt32(Console.ReadLine());

                    break;

                 case 8:
                    Console.WriteLine("Modifica giorni di assenze");
                    lavoratore.Mail = Console.ReadLine();

                    break;

                case 9:
                    Console.WriteLine("\nL'applicazione si sta per chiudere\n");

                    break;

                default:
                    Console.WriteLine("\nScelta errata.Prego scegliere tra le opzioni disponibili 1-8\n");
                    break;
            }

            // se nome,cognome vengono modificati cambia anche il nome del json corrispondente

            string newFilePath = Path.Combine(directoryPath, $"{lavoratore.Nome}_{lavoratore.Cognome}.json");

            // Serializza i dati aggiornati del dipendente 
            string jsonString = JsonConvert.SerializeObject(lavoratore, Formatting.Indented);

            //Cancella il vecchio json

            File.Delete(filePath);

            //scrive il nuovo file json con i dati aggiornati serializzati

            File.WriteAllText(newFilePath, jsonString);


            Console.WriteLine("Dipendente aggiornato con successo.");
        }
        else
        {
            Console.WriteLine("Dipendente non trovato");
        }
    }

      static void StampaDati(string filePath)
    {
        // legge il contenuto del file json
        string jsonRead = File.ReadAllText(filePath);
        // deserializza la stringa JSON in un oggetto di tipo dynamic
        var dipendente = JsonConvert.DeserializeObject<dynamic>(jsonRead);
        Console.WriteLine($"\nNome: {dipendente.Nome}");
        Console.WriteLine($"Cognome: {dipendente.Cognome}");
        Console.WriteLine($"Data di nascita: {dipendente.DataDiNascita}");
        Console.WriteLine($"Mansione: {dipendente.Mansione}");
        Console.WriteLine($"Stipendio: {dipendente.Stipendio}");
        Console.WriteLine($"Performance: {dipendente.Performance}");
        Console.WriteLine($"Giorni di assenza: {dipendente.Assenze}");
        Console.WriteLine($"Giorni di assenza: {dipendente.Mail}");

    }

    static void RimuoviDipendente()
    {
        Console.WriteLine("Inserisci nome e cognome del dipendente che vuoi rimuovere separati da virgola");
        var inserisciNome = Console.ReadLine();
        var nomi = inserisciNome.Split(',');

        if (nomi.Length != 2)
        {
            Console.WriteLine("Nomi non validi");
            return;
        }

        string nome = nomi[0].Trim();
        string cognome = nomi[1].Trim();
        string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);    // rimuove file json
            Console.WriteLine("Dipendente rimosso con successo.");
        }
        else
        {
            Console.WriteLine("Dipendente non trovato");
        }
    }

   

}