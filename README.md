# Projekt Správa Výrobků a dílů

![alt text](https://github.com/GimliCZ/Sprava-Vyrobku-a-Dilu/blob/master/nahled.png?raw=true)

## Úvod

Tento projekt se zaměřuje na vytvoření .NET aplikace pro správu výrobků a dílů v databázi. Aplikace umožňuje přidávání, úpravy a mazání záznamů, a zobrazuje přehled všech výrobků a dílů.

## Požadavky

- .NET 8.0
- MS SQL Server
- Visual Studio (jakákoliv verze)

## 1. Databáze

### 1.1 Tabulka „Výrobek“

- **Sloupce:**
  - `Nazev` (VARCHAR) - Název výrobku (povinné pole)
  - `Popis` (TEXT) - Popis výrobku
  - `Cena` (DECIMAL) - Cena výrobku (povinné pole)
  - `Poznamka` (TEXT) - Poznámky
  - `Zalozeno` (DATETIME) - Datum a čas vytvoření záznamu
  - `Upraveno` (DATETIME) - Datum a čas poslední úpravy záznamu

- **Primární klíč:** `Id` (INT, AUTO_INCREMENT)

### 1.2 Tabulka „Díl“

- **Sloupce:**
  - `Nazev` (VARCHAR) - Název dílu (povinné pole)
  - `Popis` (TEXT) - Popis dílu
  - `Cena` (DECIMAL) - Cena dílu (povinné pole)
  - `Zalozeno` (DATETIME) - Datum a čas vytvoření záznamu
  - `Upraveno` (DATETIME) - Datum a čas poslední úpravy záznamu

- **Primární klíč:** `Id` (INT, AUTO_INCREMENT)

### 1.3 Ostatní Popis

- **Primární klíče:** Byly zvoleny auto-increment `Id` pro jednoznačnou identifikaci záznamů.
- **Relační Vazba:** Mezi tabulkou „Výrobek“ a tabulkou „Díl“ existuje relační vazba typu 1:N. Každý výrobek může mít více dílů, ale každý díl patří pouze jednomu výrobku.

## 2. Popis Aplikace

### 2.1 Hlavní Formulář

- **Funkce:**
  - Zobrazení přehledu všech výrobků a dílů v tabulce (grid).
  - Toolbar a menu pro:
    - Vložení nového záznamu
    - Editaci existujícího záznamu
    - Smazání záznamu
    - Obnovení dat (refresh)
  - Zobrazení sloupce s počtem dílů pro každý výrobek.

### 2.2 Insert/Edit Formulář

- **Funkce:**
  - Formulář pro vytvoření/úpravu výrobků.
  - Validace nezbytných položek:
    - `Nazev` a `Cena` jsou povinná pole.
  - Formulář pro vytvoření/úpravu dílu je implementován.

## 3. Doplňující Informace

- **Vývoj:** Projekt byl vyvinut v prostředí Visual Studio a je založen na .NET Framework.
- **Databáze:** MS SQL Server byla použita pro správu databáze.
- **Design:** Aplikace byla navržena pro snadnou údržbu a rozšiřitelnost, aby umožnila budoucí úpravy a vylepšení.

## 4. Výstup

- **Aplikace:** Kompletní aplikace pro správu výrobků a dílů.
- **Kompilovatelný Kód:** Zdrojový kód, který lze kompilovat v prostředí Visual Studio.
- **Databáze:** SQL skripty pro vytvoření a naplnění databáze.


# Licence
Tento projekt je licencován pod MIT License.
