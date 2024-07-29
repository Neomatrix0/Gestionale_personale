using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static string directoryPath = @"dipendenti/";

    static void Main(string[] args)
    {
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
            Console.WriteLine("6. Tasso di assenteismo");
            Console.WriteLine("7. Indicatore di performance");
            Console.WriteLine("8. Ordina per stipendio");
            Console.WriteLine("9. Incidenza percentuale");
            Console.WriteLine("10. Esci\n");

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
                    TassoDiAssenteismo();
                    break;
                case 7:
                    ValutazionePerformance();
                    break;
                case 8:
                    SortStipendio();
                    break;
                case 9:
                  

                    IncidenzaPercentuale();
                    break;
                case 10:
                    Console.WriteLine("Il programma verrà chiuso. Attendere prego.");
                    break;
                default:
                    Console.WriteLine("Errore di scelta: Prego riprovare");
                    break;
            }

            if (opzione != 10)
            {
                Console.WriteLine("\nPremere un tasto per proseguire");
                Console.ReadKey();
            }
        } while (opzione != 10);
    }

    static void InserisciDipendente()
    {
        do
        {
            try
            {
                Console.WriteLine("Inserisci nome, cognome, data di nascita DD/MM/YYYY, mansione, stipendio, voto performance da 1 a 100, giorni di assenze, email, separati da virgola");
                string? inserimento = Console.ReadLine();
                string[] dati = inserimento.Split(',');

                if (dati.Length != 8)
                {
                    throw new FormatException("L'input deve contenere esattamente otto valori separati da virgola");
                }

                DateTime dataDiNascita = DateTime.ParseExact(dati[2].Trim(), "dd/MM/yyyy", null);
                string dataDiNascitaFormatted = dataDiNascita.ToString("dd/MM/yyyy");

                var dipendente = new
                {
                    Nome = dati[0].Trim(),
                    Cognome = dati[1].Trim(),
                    DataDiNascita = dataDiNascitaFormatted,
                    Mansione = dati[3].Trim(),
                    Stipendio = Convert.ToDecimal(dati[4].Trim()),
                    Performance = Convert.ToInt32(dati[5].Trim()),
                    Assenze = Convert.ToInt32(dati[6].Trim()),
                    Mail = dati[7].Trim()
                };

                string jsonString = JsonConvert.SerializeObject(dipendente, Formatting.Indented);
                string filePath = Path.Combine(directoryPath, $"{dipendente.Nome}_{dipendente.Cognome}.json");
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERRORE INSERIMENTO DATI: {e.Message}");
                Console.WriteLine($"CODICE ERRORE: {e.HResult}");
                return;
            }

            Console.WriteLine("Vuoi inserire un altro dipendente? (s/n)");
            string? risposta = Console.ReadLine().Trim().ToLower();

            if (risposta == "n")
            {
                break;
            }
        } while (true);
    }

    static void VisualizzaDipendenti()
    {
        var files = Directory.GetFiles(directoryPath, "*.json");

        if (files.Length > 0)
        {
            Console.WriteLine("Lista dipendenti completa con tutti i dati:\n");
            foreach (var file in files)
            {
                StampaDati(file);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Nessun dipendente nel database.");
        }
    }

    static void CercaDipendente()
    {
        try
        {
            Console.WriteLine("\nInserisci nome e cognome del dipendente che vuoi cercare separati da virgola");
            var inserisciNome = Console.ReadLine();
            var nomi = inserisciNome.Split(',');

            if (nomi.Length != 2)
            {
                throw new FormatException("L'input deve contenere esattamente due valori separati da virgola: nome e cognome.");
            }

            string nome = nomi[0].Trim();
            string cognome = nomi[1].Trim();
            string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

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

    static void ModificaDipendente()
    {
        try
        {
            Console.WriteLine("\nInserisci nome e cognome del dipendente che vuoi modificare separati da virgola");
            var inserisciNome = Console.ReadLine();
            var nomi = inserisciNome.Split(',');

            if (nomi.Length != 2)
            {
                Console.WriteLine("Nomi non validi. Inserire nome, cognome separati da virgola");
                return;
            }

            string nome = nomi[0].Trim();
            string cognome = nomi[1].Trim();
            string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

            if (File.Exists(filePath))
            {
                var lavoratore = LeggiJson(filePath);

                Console.WriteLine("MODIFICA DIPENDENTE\n1. Cambia nome\n2. Cambia cognome\n3. Cambia data di nascita formato DD/MM/YYYY\n4. Cambia mansione\n5. Cambia stipendio\n6. Cambia punteggio performance\n7. Cambia giorni di assenze\n8. Cambia mail\n9. Esci");
                var scelta = Convert.ToInt32(Console.ReadLine());

                switch (scelta)
                {
                    case 1:
                        Console.WriteLine("Inserisci il nuovo nome");
                        lavoratore.Nome = Console.ReadLine().Trim();
                        break;
                    case 2:
                        Console.WriteLine("Inserisci il nuovo cognome");
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
                        Console.WriteLine("Inserisci il nuovo indirizzo email aziendale");
                        lavoratore.Mail = Console.ReadLine().Trim();
                        break;
                    case 9:
                        Console.WriteLine("\nL'applicazione si sta per chiudere\n");
                        break;
                    default:
                        Console.WriteLine("\nScelta errata. Prego scegliere tra le opzioni disponibili 1-9\n");
                        break;
                }

                string newFilePath = Path.Combine(directoryPath, $"{lavoratore.Nome}_{lavoratore.Cognome}.json");
                string jsonString = JsonConvert.SerializeObject(lavoratore, Formatting.Indented);

                File.Delete(filePath);
                File.WriteAllText(newFilePath, jsonString);

                Console.WriteLine("Dipendente aggiornato con successo.");
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

    static void RimuoviDipendente()
    {
        Console.WriteLine("Inserisci nome e cognome del dipendente che vuoi rimuovere separati da virgola");
        var inserisciNome = Console.ReadLine();
        var nomi = inserisciNome.Split(',');

        if (nomi.Length != 2)
        {
            Console.WriteLine("Nomi non validi. Inserire nome, cognome separati da virgola");
            return;
        }

        string nome = nomi[0].Trim();
        string cognome = nomi[1].Trim();
        string filePath = Path.Combine(directoryPath, $"{nome}_{cognome}.json");

        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.WriteLine("Dipendente rimosso con successo.");
            }
            else
            {
                Console.WriteLine("Dipendente non trovato");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Errore durante la rimozione del dipendente: {e.Message}");
            Console.WriteLine($"CODICE ERRORE: {e.HResult}");
        }
    }

    static void SortStipendio()
    {
        var dipendenti = GetDipendenti();

        for (int i = 0; i < dipendenti.Count - 1; i++)
        {
            for (int j = 0; j < dipendenti.Count - i - 1; j++)
            {
                if (dipendenti[j].Stipendio < dipendenti[j + 1].Stipendio)
                {
                    var temp = dipendenti[j];
                    dipendenti[j] = dipendenti[j + 1];
                    dipendenti[j + 1] = temp;
                }
            }
        }

        Console.WriteLine("\nDipendenti ordinati per stipendio in ordine discendente:\n");
        foreach (var dipendente in dipendenti)
        {
            Console.WriteLine($"Nome: {dipendente.Nome} {dipendente.Cognome}, Stipendio: {dipendente.Stipendio}, Performance: {dipendente.Performance}");
        }
    }

    static void StampaDati(string filePath)
    {
        string jsonRead = File.ReadAllText(filePath);
        var dipendente = JsonConvert.DeserializeObject<dynamic>(jsonRead);
        Console.WriteLine($"\nNome: {dipendente.Nome}");
        Console.WriteLine($"Cognome: {dipendente.Cognome}");
        Console.WriteLine($"Data di nascita: {dipendente.DataDiNascita}");
        Console.WriteLine($"Mansione: {dipendente.Mansione}");
        Console.WriteLine($"Stipendio: {dipendente.Stipendio}");
        Console.WriteLine($"Performance: {dipendente.Performance}");
        Console.WriteLine($"Giorni di assenza: {dipendente.Assenze}");
        Console.WriteLine($"Mail aziendale: {dipendente.Mail}");
    }

    static void TassoDiAssenteismo()
    {
        Console.WriteLine("\nDi seguito l'elenco con il tasso di assenteismo per ogni dipendente su 250 giorni lavorativi equivalente ad 1 anno\n");
        int giorniLavorativiTotali = 250;

        try
        {
            var dipendenti = GetDipendenti();

            foreach (var dipendente in dipendenti)
            {
                int assenze = dipendente.Assenze;
                double assenteismo = ((double)assenze / giorniLavorativiTotali) * 100;
                double tassoAssenteismo = Math.Round(assenteismo, 2);

                Console.WriteLine($"{dipendente.Nome} {dipendente.Cognome}, Tasso di assenteismo: {tassoAssenteismo}%");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Errore generale: {e.Message}");
        }
    }

    static void ValutazionePerformance()
    {
        var dipendenti = GetDipendenti();

        Console.WriteLine("\nDivide i dipendendenti in 2 gruppi in base al rendimento");

        for (int i = 0; i < dipendenti.Count - 1; i++)
        {
            for (int j = 0; j < dipendenti.Count - i - 1; j++)
            {
                if (dipendenti[j].Performance < dipendenti[j + 1].Performance)
                {
                    var temp = dipendenti[j];
                    dipendenti[j] = dipendenti[j + 1];
                    dipendenti[j + 1] = temp;
                }
            }
        }

        int split = dipendenti.Count / 2;
        List<dynamic> squadra1 = dipendenti.GetRange(0, split);
        List<dynamic> squadra2 = dipendenti.GetRange(split, dipendenti.Count - split);

        Console.WriteLine("\nGruppo con le performance più alte:\n");
        foreach (var impiegato in squadra1)
        {
            Console.WriteLine($"Nome: {impiegato.Nome} {impiegato.Cognome}, Performance: {impiegato.Performance}");
        }

        Console.WriteLine("\nGruppo con le performance più basse:\n");
        foreach (var impiegato in squadra2)
        {
            Console.WriteLine($"Nome: {impiegato.Nome} {impiegato.Cognome}, Performance: {impiegato.Performance}");
        }

        squadra2.Sort((x, y) => x.Performance.CompareTo(y.Performance));
        int index = (15 * squadra2.Count) / 100;

        if (index == 0 && squadra2.Count > 0)
        {
            index = 1;
        }

        Console.WriteLine("\nDi seguito il 15% delle performance peggiori\n");
        for (int i = 0; i < index; i++)
        {
            var membro = squadra2[i];
            Console.WriteLine($"Nome: {membro.Nome} {membro.Cognome}, Performance: {membro.Performance}");
        }
    }

    static List<dynamic> GetDipendenti()
    {
        var files = Directory.GetFiles(directoryPath, "*.json");
        List<dynamic> dipendenti = new List<dynamic>();

        foreach (var file in files)
        {
            var dipendente = LeggiJson(file);
            dipendenti.Add(dipendente);
        }

        return dipendenti;
    }

    static dynamic LeggiJson(string filePath)
    {
        string jsonRead = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<dynamic>(jsonRead);
    }

    static void IncidenzaPercentuale()
{
    string fileTxt = "fatturato.txt";
    double fatturato;

    if (!File.Exists(fileTxt))
    {
        Console.WriteLine("Inserisci fatturato");
        fatturato = Convert.ToDouble(Console.ReadLine());
        File.WriteAllText(fileTxt, fatturato.ToString());
    }
    else
    {
        string[] lines = File.ReadAllLines(fileTxt);
        if (lines.Length == 0)
        {
            Console.WriteLine("Inserisci fatturato");
            fatturato = Convert.ToDouble(Console.ReadLine());
            File.WriteAllText(fileTxt, fatturato.ToString());
        }
        else
        {
            fatturato = Convert.ToDouble(lines[0]);
        }
    }

    var dipendenti = GetDipendenti();

    dipendenti.Sort((y, x) => x.Stipendio.CompareTo(y.Stipendio));

    Console.WriteLine("\nDipendenti e incidenza stipendio lordo sul fatturato:\n");
   

    foreach (var dipendente in dipendenti)
    {
        double stipendio = Convert.ToDouble(dipendente.Stipendio);
        double costoPersonale = (stipendio / fatturato) * 100;
        double costoPercentuale = Math.Round(costoPersonale, 2);

        Console.WriteLine($"Nome:{dipendente.Nome}\nCognome:{dipendente.Cognome}\nData di nascita:{dipendente.DataDiNascita}\nMansione:{dipendente.Mansione}\nStipendio:{dipendente.Stipendio}\nRapporto stipendio/fatturato:{costoPercentuale}%\nPerformance:{dipendente.Performance}\nAssenze:{dipendente.Assenze}");
    }
}

}
