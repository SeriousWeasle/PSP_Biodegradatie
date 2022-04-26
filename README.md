# PSP_Biodegradatie
Software voor het bepalen van bioafbreekbaarheid van plastics. Deel van een intern project bij het Polymer Science Park.

# Handleiding is [hier](https://github.com/SeriousWeasle/PSP_Biodegradatie/wiki) te vinden.

# Installatie

## Downloaden
1. Download het zip-bestand van de laatste release [hier](https://github.com/SeriousWeasle/PSP_Biodegradatie/releases/)
2. Unzip het bestand "Biodegradatie.zip"

## Arduino-gedeelte
3. Kijk of de Arduino-software op de computer is geinstalleerd. Zo niet is deze software [hier](https://www.arduino.cc/en/software) te downloaden
4. Maak zeker dat de Arduino op de computer is aangesloten met USB
5. Open het bestand "./biodeg_arduino/biodeg_arduino.ino".
6. Ga in de software naar het tabblad "Hulpmiddelen", stel de optie "Board:" in op "Arduino Mega or Mega 2560", en "Processor:" op "ATmega2560 (Mega 2560)"

![image](https://user-images.githubusercontent.com/30732669/165261707-d8079957-b606-4ac6-8224-a90062fef998.png)

8. Kies onder het tabblad "Hulpmiddelen" de Arduino door bij de optie "Poort" een poort te kiezen. De Arduino verschijnt meestal als "COM3", "COM4" of iets vergelijkbaars. Het nummer kan afwijken.
9. Druk op het groene pijltje in de groene balk bovenaan:

![image](https://user-images.githubusercontent.com/30732669/165262834-18acb7aa-bf4c-4255-8314-00cd8ae2f15b.png)

Als alles goed gaat dan verschijnt er onderaan het venster een voortgangsbalk en verschijnt er in het zwarte gebied tekst. Witte tekst is informatie, gele tekst zijn waarschuwingen en rode tekst foutmeldingen. Bij het verschijnen van een foutmelding kunnen de volgende stappen genomen worden:

### Troubleshooting
1. Check of de instellingen onder het tabblad "Hulpmiddelen" goed ingesteld staan. Let op: als de Arduino vervangen is met een ander model moeten er mogelijk andere instellingen gekozen worden dan aangegeven.
2. Koppel de Arduino los van de computer en verbind deze daarna weer, soms zijn er problemen die verholpen kunnen worden door de Arduino opnieuw te verbinden met de computer.
3. Controleer of er nog andere programma's open staan die met de Arduino proberen te communiceren. Het kan hier gaan om vorige versies van de biodegradatie-software of een ander venster dat met het apparaat probeert te communiceren.
4. Maak zeker dat er geen bewerkingen op de code in het midden van het venster gedaan zijn die zorgen voor foutmeldingen.
5. Maak zeker dat er een poort gekozen is. Verder als er een poort gekozen is en er meerdere poorten verschenen zijn in de lijst, kies een andere poort.


Als er geen foutmeldingen zijn en de software aangeeft dat de code succesvol geupload is is de Arduino klaar voor gebruik.

## Computer-gedeelte
10. Kijk of de .NET runtime geinstalleerd is, zo niet of bij twijfel is [hier](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-6.0.3-windows-x64-installer) een download verkijgbaar voor .NET versie 6.0.3.
11. Kopieer het mapje "./BioDeg (versienummer)" naar een gewenste plek.
12. Stel het tijdsinterval in voor de metingen door in het mapje "BioDeg (versienummer)" het bestand "settings.json" te openen met Kladblok. **Belangrijk: dit bestand is heel gevoelig voor verandering van leestekens. Er kunnen geen kommagetallen gebruikt worden. Pas alleen de getallen aan en *niets anders*. Laat ook geen velden leeg.**
13. Maak zeker dat de Arduino op de computer is aangesloten. Dubbelklik op "BioDeg_cli.exe", er zou nu een venster moeten openen die aangeeft wat het meetinterval is in seconden en een vraag voor een bestandsnaam.

![image](https://user-images.githubusercontent.com/30732669/165267788-55aaffd0-ced6-40fe-85ae-e50f47e9d978.png)

Vul een bestandsnaam in en druk op Enter. De software gaat nu automatisch op zoek naar de Arduino en zal als de Arduino gevonden is meteen beginnen met meten. Als dat niet gebeurt zijn er hier een aantal gevallen voor wat er mis kan zijn:

#### Could not write output to csv, make sure other progams are closed
Zoals aangegeven kan de software niet het CSV-bestand updaten. Maak zeker dat deze niet open staat in een ander bestand die voorkomt dat de software ernaartoe kan schrijven.

#### No Serial devices found, retrying...
Maak zeker dat de Arduino door middel van een USB-kabel aan de computer vastzit

#### Device on port (poortnaam) is busy or failed to communicate
Maak zeker dat er niet meerdere programma's tegelijk met de Arduino proberen te communiceren. Hieronder vallen meerdere instanties van deze software of mogelijk de Arduino-software.

#### Failed to parse measurement, retrying...
In dit geval heeft de Arduino foutieve of incomplete data naar de computer gestuurd. Ontkoppel de Arduino van de computer en koppel deze daarna opnieuw door of de reset-knop op de Arduino in te drukken of de USB-kabel uit de computer te halen en deze er opnieuw in te steken als deze foutmelding doorloopt voor meer dan 9 keer.

# Aanvullende informatie
Voor informatie over het veranderen van instellingen of uitgebreidere instructies, is [hier](https://github.com/SeriousWeasle/PSP_Biodegradatie/wiki) meer informatie te vinden.
