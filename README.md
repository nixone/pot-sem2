# 2. sem. práca predmetu Pokročilé Objektové Technológie

## Cieľ práce

Cieľom práce bolo naimplementovať jednoduchú hru použitím objektových technológií .NET, WCF, Entity Framework a WPF.

## Zvolená hra

Mojou zvolenou hrou je hra dáma, ktorá sa odohráva na štandardnej šachovnici 8x8 políčok, každý hráč má na začiatku
8 figúrok, s ktorými sa môže pohybovať podľa pravidiel hry a takisto preskakovať (a preskočením vyhadzovať) figúrky súpera.
Hrá má dvoch hráčov a končí sa, keď ktorýkoľvek hráč nemá už žiadnu figúrku.

### Užívateľské rozhranie hry

![ClassDiagram](https://github.com/nixone/pot-sem2/raw/develop/ui.png)

Priamo v užívateľskom rozhraní môže hráč buď vytvoriť hrací server (hru, do ktorej sa môže pripojiť spoluhráč alebo pozorovatelia) alebo
sa môže pripojiť už do vytvorenej hry zadaním adresy a portu servera. Po pripojení do hry (aj hráč, ktorý hru vytvoril, sa do nej musí pripojiť ako klient)
môže hráč interagovať so svojimi figúrkami a tlačidlami ovládať nasledovanie hry. Ak už na danom serveri boli nejaké hry odohrané, môže ich ktorýkoľvek hráč
kedykoľvek prehrať (pozrieť replay).

## Architektúra

Hra bola vypracovaná ako jeden celistvý projekt. Aj server aj client sú súčasťou toho istého projektu. Užívateľ nemusí spúšťať samostatnú aplikáciu pre 
spustenie servera.

Projekt je vrstvovo rozdelený na tri časti:

1. **Herná logika**: Samotné správanie hry, overovanie ťahov a prechody medzi hernými stavmi
2. **Komunikačná logika**: Komunikácia herného stavu a príkazov medzi hráčmi a serverom
3. **Logika užívateľského rozhrania**: Komunikácia herného stavu do užívateľského rozhrania a komunikácia interakcií užívateľského rozhrania späť na server

Takéto rozvrstvenie zaručí dostatočnú flexibilitu, ak by sa vo vývoji projektu pokračovalo a takisto zaručí vyššiu kvalitu kódu oddelením jednotlivých častí.

### UML Diagram tried

![ClassDiagram](https://github.com/nixone/pot-sem2/raw/develop/pot-sem2/ClassDiagram.png)

## Použité technológie

* .NET 6.0 pre samotný základ hry
* EntityFramework pre komunikáciu s databázou
* WCF pre komunikáciu medzi serverom a klientom (TCP komunikácia cez sieť)
* WPF pre vypracovanie užívateľského rozhrania

## Požiadavky na hru

* Pripojenie na server. Ak je serverom ten istý počítač (takéto nastavenie je možné), pripojenie sa samozrejme nevyžaduje

## Vyriešené problémy

* **Problém so serializáciou série herných stavov**: Bolo potrebné riešiť problém, kedy štandardnou deserializáciou herných stavov vznikala iná postupnosť herných stavov ako tá, ktorá
bola serializovaná. Po vymenení XML serializácie za DataContract serializáciu (ktorá sa natívne používa pri serializácii objektov pri WCF) sa problém prestal vyskytovať. 

* **Problém so serializáciou kolekcií definovanými rozhraniami do XML**: Bolo potrebné vytvoriť vlastnú implementáciu pre serializáciu generických kolekcií, konkrétne množín (HashSet), ktorých generický typ je definovaný iba rozhraním. Takýto use-case nie je naimplementovaný v základnej implementácii triedy XmlSerializer.

## Záver

Pri implementácii som vďaka nutnosti vyriešiť mnoho problémov získal veľa skúseností v objektových technológiách, s ktorými som 
doteraz nemal veľa príležitostí pracovať. Veľmi dobre sa mi pracovalo s technológiami WCF a WPF, existovalo veľké množstvo materiálov, z ktorých sa dalo
čerpať a prístup ku riešeniu problémov bol takmer vždy veľmi priamočiary. Samozrejme, ako všetky technológie, ktoré odbremeňujú vývojárov, aj pri týchto
je zo začiatku ťažké pochopiť niektoré novo prinesené koncepty. Napriek tomu je efektívne snažiť sa týmto technológiám porozumieť a osvojiť si koncepty namiesto 
vytvárania vlastnej implementácie.