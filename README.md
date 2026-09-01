# IM_A2 Kryptologie, Coding-Übungen

Kleine Übungen zu Block 1 des Moduls IM_A2 an der HF Zug. Es kommen im Lauf des Blocks
weitere dazu, `git pull` holt sie.

Die Tests sind der Auftrag. Ein roter Test sagt, was fehlt, ein grüner, dass es stimmt.
Sie müssen nichts abgeben.

## Voraussetzungen

.NET-10-SDK. Prüfen mit:

```bash
dotnet --version
```

Muss mit `10.` beginnen. Falls nicht: https://dotnet.microsoft.com/download

## Loslegen

```bash
git clone https://github.com/Mad-Kat/IM_A2.git
cd IM_A2
dotnet test
```

Das erste Mal dauert es etwas, danach eine Sekunde. Rote Tests sind erwartet, das sind
die Aufgaben.

## Die Übungen

|     | Woche | Datei                               | Worum es geht        |
| --- | ----- | ----------------------------------- | -------------------- |
| C1  | W2    | `Library/SymmetricEncryptor.cs`     | AES im CBC-Modus     |
| C2  | W2    | `Library/AuthenticatedEncryptor.cs` | AES im GCM-Modus     |
| C3  | W4    | `Library/AsymmetricEncryptor.cs`    | RSA und seine Grenze |

Im Code steht an den offenen Stellen `// TODO: implement`. Was dort hingehört, sagen die
Tests. Die Testdateien selbst bleiben unverändert, es gibt keine Aufgabe, bei der Sie
einen Test anpassen müssen.

Wenn alles grün ist, schauen Sie sich die Musterlösung an. Dort steht als Kommentar,
worum es in der Aufgabe eigentlich ging.

### C1, AES im CBC-Modus

`Encrypt` verschlüsselt noch nicht, `Decrypt` ist schon da. Zwei Tests sind rot.

Der erste wird grün, sobald `Encrypt` überhaupt verschlüsselt. Für den zweiten reicht das
nicht: er verschlüsselt dieselbe Nachricht zweimal und erwartet zwei verschiedene
Chiffrate, die sich beide wieder entschlüsseln lassen. Dafür braucht jede Nachricht
ihren eigenen Initialisierungsvektor, und der Empfänger muss ihn trotzdem bekommen.
Üblich ist, ihn dem Chiffrat voranzustellen.

### C2, AES im GCM-Modus

Zwei `// TODO` in `AuthenticatedEncryptor.cs`, verschlüsseln und entschlüsseln. Setzt C1
voraus: einer der Tests verschlüsselt mit Ihrem `SymmetricEncryptor`.

Der interessante Test heisst `CbcAcceptsAManipulatedCiphertext`. Er spielt einen
Angreifer, der den Schlüssel nicht kennt, aber das Chiffrat unterwegs verändern kann.
Lassen Sie ihn mit Ausgabe laufen und lesen Sie, was ankommt:

```bash
dotnet test --logger "console;verbosity=detailed"
```

### C3, RSA und seine Grenze

Drei `// TODO` in `AsymmetricEncryptor.cs`: das Schlüsselpaar mit 2048 Bit, `Encrypt` und
`Decrypt`. Welches Padding Sie nehmen, ist Ihre Sache, `RSAEncryptionPadding.OaepSHA256`
ist die übliche Wahl. Verschlüsseln und entschlüsseln müssen dasselbe nehmen.

Zwei Tests erwarten eine Exception. RSA verschlüsselt nicht beliebig viel: der Klartext
muss als Zahl kleiner sein als der Modul, und das Padding belegt einen Teil davon.
32 Byte gehen durch, 256 nicht. Rechnen Sie nach, wie viel Ihr Padding übrig lässt, und
überlegen Sie, wie Sie damit eine Datei von 4 GB verschlüsseln.

## Nur eine Übung laufen lassen

```bash
dotnet test --filter SymmetricEncryptorTests
```

## Lösungen

Alle Lösungen liegen auf dem Branch `origin/solutions`. Sie müssen den Branch dafür nicht
wechseln, und Sie verlieren nichts:

```bash
# die gelöste Datei ansehen
git show origin/solutions:Library/SymmetricEncryptor.cs

# den Unterschied zwischen Aufgabe und Lösung ansehen
git diff HEAD origin/solutions -- Library/SymmetricEncryptor.cs

# die Lösung übernehmen
git checkout origin/solutions -- Library/SymmetricEncryptor.cs
```

Schauen Sie erst hin, wenn Sie es selbst versucht haben.

## Aufbau

```
Library/          die fünf Klassen, hier arbeiten Sie
Library.Tests/    die Tests, die brauchen Sie nicht anzufassen
```

xUnit mit den eingebauten `Assert`-Methoden, keine zusätzliche Assertion-Bibliothek.
Hex-Umwandlung über `Convert.ToHexStringLower` und `Convert.FromHexString` aus der BCL.
