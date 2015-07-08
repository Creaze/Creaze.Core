# Was ist Creaze?

Creaze erleichtert das oft zeitraubende Erstellen eines Dienstplans für Ministranten. 
Creaze erstellt auf Basis einer Liste von Ministranten und einer Liste von Gottesdiensten
einen Dienstplan für die Ministranten. Einzelne Ministranten können als "Oberministranten"
gekennzeichnet werden, zudem können Gruppen erstellt werden. Die Gruppen werden dann immer
als ganze Gruppe eingeteilt (sofern möglich).

Bei diesem Projekt handelt es sich nur um die Kern-Bibliothek (darum auch Creaze.Core :wink:).
Eine erste, mit WPF erstellte, grafische Oberfläche wird bald als
[Creaze.WPF](https://github.com/Creaze/Creaze.WPF) veröffentlicht. Bitte habt bis dahin noch
ein wenig Geduld.

# Mitmachen

Jede Art der Mithilfe ist herzlich willkommen! Einfach das Repository
[forken](https://github.com/Creaze/Creaze.Core/fork) und los geht's!

Da Creaze.Core in C# geschrieben ist, ist [Visual Studio](http://www.visualstudio.com)
(VS) für die Programmierung (und vor allem das Kompilieren...) nötig. Dafür reicht die
kostenlose 2010er Express-Edition für C# aus. Wenn Ihr zum Entwickeln eine neuere Version
von VS als VS 2010 verwendet, dann achtet bitte darauf, dass als Zielframework das
.NET Framework 4.0 ausgewählt ist. Zudem sollen die Projekt-Dateien (.sln und .csproj)
mit VS 2010 kompatibel bleiben. Das macht es leider erforderlich, dass Änderungen an den
Projekt-Einstellungen von Hand in die Dateien aus dem Repo eingetragen werden müssen
(mit einem Text-Editor). Hoffentlich finden wir dafür in Zukunft eine bessere Lösung...

## Wiki

Mithilfe beim Wiki ist natürlich auch eine super Sache. Das Wiki sollte die
API-Dokumentation und ein paar Anwendungsbeispiele enthalten. Wenn Euch was Gutes
einfällt, dann immer rein damit ins Wiki.

## Sprache

Auch wenn bis jetzt noch das meiste auf Deutsch ist, solltet Ihr darauf achten, dass
Creaze in Zukunft auch auf Englisch übersetzt werden soll. Das bedeutet konkret, dass
die Commit-Nachrichten **immer** auf Englisch verfasst werden und auch die Code-Doku in
englischer Sprache erfolgen sollte. Ein mehrsprachiges Wiki hat noch niemanden geschadet,
also hier ruhig auf Deutsch schreiben. Wenn Ihr mögt, dann gerne die deutschen Artikel
auf Englisch übersetzen und umgekehrt.

# Lizenz

Creaze steht unter der [GNU GPLv2](http://www.gnu.org/licenses/old-licenses/gpl-2.0).
Siehe auch [LICENSE](https://github.com/Creaze/Creaze.Core/LICENSE)