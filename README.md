# Syzyf

Syzyf to desktopowa aplikacja WPF do obsługi procesu rekrutacyjnego, projektów, zleceń, spotkań i powiadomień.

## Funkcje

- logowanie i rejestracja użytkowników
- zarządzanie projektami i kartami projektu
- obsługa zleceń i ich podglądu
- planowanie spotkań
- powiadomienia dla użytkowników
- dodawanie pracowników i zarządzanie kontem
- warstwa danych oparta o Entity Framework Core i MySQL/MariaDB

## Wymagania

- .NET 8 SDK
- Visual Studio 2022 lub inne środowisko z obsługą WPF
- MySQL / MariaDB

## Baza danych

W repozytorium znajduje się plik `syzyf.sql` z przykładową strukturą i danymi.

Domyślne połączenie aplikacji jest ustawione w `WpfApp1/App.xaml.cs`:

```text
server=localhost;port=3306;database=syzyf;user=root;password=;
```

Jeśli Twoje środowisko używa innych danych dostępu, zaktualizuj connection string przed uruchomieniem.

## Uruchomienie

```bash
dotnet restore WpfApp1.sln
dotnet run --project WpfApp1/WpfApp1.csproj
```

## Testy

```bash
dotnet test WpfApp1.sln
```

## Struktura projektu

- `WpfApp1/` — aplikacja WPF
- `WpfApp1/Views/` — widoki i ekrany
- `WpfApp1/Models/` — modele domenowe
- `WpfApp1/Data/` — kontekst EF Core
- `WpfApp1/Services/` — logika aplikacyjna
- `Test/` — testy jednostkowe MSTest

