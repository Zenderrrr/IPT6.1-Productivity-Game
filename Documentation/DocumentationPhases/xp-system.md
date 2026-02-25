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