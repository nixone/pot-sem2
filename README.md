# 1. sem. práca predmetu Pokročilé Objektové Technológie

## Cieľ práce

Cieľom práce bolo naimplementovať jednoduchú textovú hru použitím objektových technológií .NET a jazyka C#.
Pri implementácii bolo potrebné dbať na objektový návrh ako aj na oddelenie logiky implementácie textových
hier od samotnej implementácie nami zvoleného príbehu.

## Hra BŠTRNK

### Príbeh

Ocitli ste sa vo svete spravodlivosti, férového obchodu, verejných súťaží, vo svete bez korupcie, ktorú
dnešní mladí zvknú v hovorovom jazyku označovať aj ako svet alebo dobu kešu. Názov naráža samozrejme na
popularitu rovnomenných oričekov a nie na obrovské množstvo korupčných káuz a odovzdávaných peňazí v 
hotovosti.

### Cieľ hry

Zobudili ste sa krásneho dňa v miestnosti Bonaparte, u vás doma. Vašim cieľom na dnes je pozbierať
kompromitujúce materiály zo súdu, zo spoločnosti Váhstav a NAKA tak, aby ste minuly čo najviac
peňazí daňových poplatníkov. Každý prechod medzi lokáciami / miestnosťami totiž niečo stojí.
Na konci dňa sa musíte s komrpomitujúcimi materiálmy vrátiť domov, do skartovacej miestnosti, kde
použijete skartovací prístroj a hra končí.

## Architektúra

Implementácia je rozdelená na dva celky: Generalizovaná knižnica pre implementáciu textových hier a samotná implementácia hry BŠTRNK.

### UML Diagram knižnice

![ClassDiagram](https://github.com/nixone/pot-sem1/raw/develop/GameLib/ClassDiagram.png)

### UML Diagram implementácie BŠTRNK

![ClassDiagram](https://github.com/nixone/pot-sem1/raw/develop/POTSem1/ClassDiagram.png)

## Použité technológie

* .NET 6.0 pre samotný základ hry
* EntityFramework pre komunikáciu s databázou

## Požiadavky na hru

* Pripojenie na internet pre prístup k databáze nahratého skóre
* Zápis a čítanie zo súborov z miesta spustenia hry pre podporu ukladania a načítavania stavu hry

## Vyriešené problémy

* **Problém so serializáciou kolekcií definovanými rozhraniami do XML**: Bolo potrebné vytvoriť vlastnú implementáciu pre serializáciu generických kolekcií, konkrétne množín (HashSet), ktorých generický typ je definovaný iba rozhraním. Takýto use-case nie je naimplementovaný v základnej implementácii triedy XmlSerializer.

## Záver

Pri implementácii som vďaka nutnosti vyriešiť mnoho problémov získal veľa skúseností v objektových technológiách, s ktorými som doteraz nemal veľa príležitostí pracovať. V porovnaní s ostatnými platformami / jazykmi vnímam pozitívne niektoré časti .NET frameworku a jazyku C#, ako sú napríklad práce s generickými triedami, LINQ, EntityFramework, ale takisto vnímam aj niektoré negatíva, ako sú napríklad nutnosť úvadzať kombinácie override a virtual modifikátorov pred metódami.