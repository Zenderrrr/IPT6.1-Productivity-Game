# Level bestimmen

## Ziel

- Level soll klar aus XP ableitbar sein
- Level-Berechnung soll einheitlich und skalierbar sein
- Fortschritt zum nächsten Level soll anzeigbar sein

---

## Datenbasis

Benötigte User-Felder:

- `totalXP` (gesamte XP des Users)
- optional: `level` (muss nicht gespeichert werden, da berechenbar)

**Empfehlung:**  
Nur `totalXP` speichern und `level` dynamisch berechnen → verhindert Inkonsistenzen.

---

## Level-Logik (Formel)

Wir verwenden eine quadratisch wachsende Levelkurve.

### XP-Grenze pro Level

xpNeeded(L) = 100 \* L^2

### Level aus totalXP berechnen

level = floor( sqrt(totalXP / 100) ) + 1

---

## Beispiele

| totalXP | Level |
| ------- | ----- |
| 0       | 1     |
| 100     | 2     |
| 400     | 3     |
| 900     | 4     |
| 1600    | 5     |

---

## Fortschritt zum nächsten Level

Für Progress-Bar Berechnung:

level = floor( sqrt(totalXP / 100) ) + 1
xpCurrent = 100 _ (level - 1)^2
xpNext = 100 _ level^2

progress = (totalXP - xpCurrent) / (xpNext - xpCurrent)

`progress` ergibt einen Wert zwischen 0 und 1
