using System.Text.Json;

List<DiaryEntry> diary = LoadEntries();

bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("=================================");
    Console.WriteLine("      PÄIVÄKIRJASOVELLUS");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Lisää merkintä");
    Console.WriteLine("2. Näytä merkinnät");
    Console.WriteLine("3. Muokkaa merkintää");
    Console.WriteLine("4. Poista merkintä");
    Console.WriteLine("5. Tallenna tiedostoon");
    Console.WriteLine("6. Lopeta");
    Console.WriteLine();
    Console.Write("Valinta: ");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddEntry();
            break;

        case "2":
            ShowEntries();
            break;

        case "3":
            EditEntry();
            break;

        case "4":
            DeleteEntry();
            break;

        case "5":
            SaveEntries();
            break;

        case "6":
            SaveEntries();
            running = false;
            break;

        default:
            Console.WriteLine("Virheellinen valinta!");
            Pause();
            break;
    }
}

void AddEntry()
{
    Console.Clear();
    Console.WriteLine("UUSI MERKINTÄ");
    Console.WriteLine();

    Console.Write("Otsikko: ");
    string title = Console.ReadLine();

    Console.Write("Sisältö: ");
    string content = Console.ReadLine();

    diary.Add(new DiaryEntry
    {
        Date = DateTime.Now,
        Title = title,
        Content = content
    });

    Console.WriteLine();
    Console.WriteLine("Merkintä lisätty.");
    Pause();
}

void ShowEntries()
{
    Console.Clear();

    if (diary.Count == 0)
    {
        Console.WriteLine("Ei merkintöjä.");
        Pause();
        return;
    }

    Console.WriteLine("MERKINNÄT");
    Console.WriteLine();

    for (int i = 0; i < diary.Count; i++)
    {
        Console.WriteLine($"[{i + 1}]");
        Console.WriteLine($"Päivä: {diary[i].Date}");
        Console.WriteLine($"Otsikko: {diary[i].Title}");
        Console.WriteLine($"Sisältö: {diary[i].Content}");
        Console.WriteLine("---------------------------------");
    }

    Pause();
}

void EditEntry()
{
    Console.Clear();

    if (diary.Count == 0)
    {
        Console.WriteLine("Ei muokattavia merkintöjä.");
        Pause();
        return;
    }

    ShowEntriesWithoutPause();

    Console.Write("Anna muokattavan merkinnän numero: ");

    if (int.TryParse(Console.ReadLine(), out int index))
    {
        index--;

        if (index >= 0 && index < diary.Count)
        {
            Console.Write("Uusi otsikko: ");
            diary[index].Title = Console.ReadLine();

            Console.Write("Uusi sisältö: ");
            diary[index].Content = Console.ReadLine();

            Console.WriteLine("Merkintä päivitetty.");
        }
        else
        {
            Console.WriteLine("Virheellinen numero.");
        }
    }

    Pause();
}

void DeleteEntry()
{
    Console.Clear();

    if (diary.Count == 0)
    {
        Console.WriteLine("Ei poistettavia merkintöjä.");
        Pause();
        return;
    }

    ShowEntriesWithoutPause();

    Console.Write("Anna poistettavan merkinnän numero: ");

    if (int.TryParse(Console.ReadLine(), out int index))
    {
        index--;

        if (index >= 0 && index < diary.Count)
        {
            diary.RemoveAt(index);
            Console.WriteLine("Merkintä poistettu.");
        }
        else
        {
            Console.WriteLine("Virheellinen numero.");
        }
    }

    Pause();
}

void SaveEntries()
{
    string json = JsonSerializer.Serialize(
        diary,
        new JsonSerializerOptions
        {
            WriteIndented = true
        });

    File.WriteAllText("paivakirja.json", json);

    Console.WriteLine("Tallennettu tiedostoon.");
}

List<DiaryEntry> LoadEntries()
{
    if (File.Exists("paivakirja.json"))
    {
        string json = File.ReadAllText("paivakirja.json");

        try
        {
            return JsonSerializer.Deserialize<List<DiaryEntry>>(json)
                   ?? new List<DiaryEntry>();
        }
        catch
        {
            return new List<DiaryEntry>();
        }
    }

    return new List<DiaryEntry>();
}

void ShowEntriesWithoutPause()
{
    Console.WriteLine("MERKINNÄT");
    Console.WriteLine();

    for (int i = 0; i < diary.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {diary[i].Title}");
    }

    Console.WriteLine();
}

void Pause()
{
    Console.WriteLine();
    Console.WriteLine("Paina Enter jatkaaksesi...");
    Console.ReadLine();
}

public class DiaryEntry
{
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}