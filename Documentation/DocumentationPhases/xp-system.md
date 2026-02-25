# Wie wird XP berechnet & gespeichert

## Ziele
- klare / verständliche Regeln
- wiederproduzierbare Berechnung
- klare Speicherung der XP
- nachvollziehbarkeit des XP verlaufs

## Wie wird XP berechnet?

| Faktor | Beispiel |
|--------|----------|
| Schwierigkeit | Einfach / Mittel / Schwer |
| Dauer | 10min / 30min / 1h / 2h |
| Streak | +2% je Tag |
| Zeitlich bedingter Bonus | +5% wenn man Badge freigeschalten hat |


### XP Berechnungs Formel
<pre>
    BaseXP = Difficulty
    TimeBonus = Duration / 3
    StreakBonus = (BaseXp + TimeBonus ) * (Streak * 0.02)
    TemporaryBonus = (BaseXP + TimeBonus + StreakBonus) * 0.05

    XP = BaseXP + TimeBonus + StreakBonus + TemporaryBonus
</pre>

## Wie wird XP gespeichert?

### Wie wird nachvollziehbarkeit sichergestellt?
Das wird dadurch erreicht, dass man die XP nicht als gesamte XP speichert (zb. 500 XP und bei einer erhöhung dann 550 XP), sondern es wird in der DB eine zusätzliche Tabelle angelegt. Diese Tabelle ist dann als Event zuständig, wenn die XP geupdated werden. Damit hat jedes Updaten der XP einen weiteren Datensatz zurfolge, wodurch besser verfolgt werden kann wann, wieviel und weshalb die XP angepasst wurden.

### XP Tabelle

<pre>
    XP (
        XPID (PK)
        UserID (FK), 
        TaskID (FK),
        XPAmount,
        Reason,
        CreatedAt
    )
</pre>

### Wie werden die gesammt XP berechnet?
Die gesamt XP werden durch einen SELECT Befehl herausgewonnen, dadurch das für User X alle XP Tabellen durchgegangen werden, welche alle einen Amount besitzen an XP, welcher dann als SUM ausgegeben werden kann.