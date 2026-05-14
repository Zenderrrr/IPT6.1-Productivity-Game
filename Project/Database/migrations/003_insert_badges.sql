-- SQLite

PRAGMA foreign_keys = ON;

INSERT INTO Badge (
    name, description, rule_type, rule_value,
    rarity, primary_color, secondary_color, isVisible
) VALUES
('First Step', 'Absolviere deine erste Task ab und beginne deinen Weg für ein Produktiveres Leben.', 'tasks_completed', 1, 'Common', '#374151', '#E5E7EB', 1),
('Getting Started', 'Bewältige fünf Tasks auf deiner Reise auf FocusUp.', 'tasks_completed', 5, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('Task Grinder', 'Schaffe 50 Tasks. Dies ist erst der Anfang.', 'tasks_completed', 50, 'Epic', '#7C3AED', '#DDD6FE', 1),
('Machine', 'Wie? Du hast 100 Tasks abgeschlossen! Wie geht es weiter?', 'tasks_completed', 100, 'Legendary', '#CA8A04', '#FEF08A', 1),

('3 Day Streak', 'Die ersten Schritte zu einem Produktivem Alltag.', 'streak', 3, 'Common', '#374151', '#E5E7EB', 1),
('7 Day Warrior', 'Eine Woche am Stück bist du schon Produktiv. Weiter so!', 'streak', 7, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('30 Day Discipline', 'Du bist nicht aufzuhalten! Einen Monat bist du schon kontinuierlich dran.', 'streak', 30, 'Epic', '#7C3AED', '#DDD6FE', 1),
('100 Day Legend', 'Deine Willenskraft ist insane! Sehr beeindruckend!', 'streak', 100, 'Legendary', '#CA8A04', '#FEF08A', 1),

('XP Starter', 'Schritt für Schritt. Die ersten 50 XP.', 'xp_total', 50, 'Common', '#374151', '#E5E7EB', 1),
('Leveling Up', 'Das Rennen beginnt, wie weit wirst du noch kommen? Die 800 XP hast du schon mal.', 'xp_total', 800, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('XP Master', 'Du bist weit gekommen, der Master Rang kommt dir echt zu gute.', 'xp_total', 8000, 'Epic', '#7C3AED', '#DDD6FE', 1),

('Time Tracker', 'Wie fühlts sich an zu gamen, eh, zu arbeiten meine ich?', 'time_logged', 300, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('Deep Worker', 'Du bist konzentriert an der Sache, wollen wir mal nicht weiter Stören.', 'time_logged', 1000, 'Epic', '#7C3AED', '#DDD6FE', 1),
('Focus Beast', 'Konzentration ist in deinem Sinne, schon fast eine Untertreibung.', 'time_logged', 5000, 'Legendary', '#CA8A04', '#FEF08A', 1),

('Easy Rider', 'Die einfachen Aufgaben müssen auch mal gemacht werden. Schaffe 20 einfache Tasks.', 'difficulty_achived_easy', 20, 'Common', '#374151', '#E5E7EB', 1),
('Challenger', 'Jetzt kommt es zu den schweren Brocken.', 'difficulty_achived_hard', 10, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('Hardcore', 'Nur Tasks des Types Hard, du bist wirklich Hardcore.', 'difficulty_achived_hardcore', 10, 'Epic', '#7C3AED', '#DDD6FE', 1),

('Early Bird', 'Der frühe Vogel fängt den Wurm. Erledige Tasks vor 8 Uhr.', 'consistency_check_morning', 8, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('Night Owl', 'Während andere schlafen, bist du noch am Hustlen. Schaffe Tasks nach 22 Uhr.', 'consistency_check_evening', 22, 'Rare', '#1D4ED8', '#BFDBFE', 1),
('Deadline Master', 'Deadlines sind für dich keine Gefahr. Erledige Tasks bevor die DueDate erreicht wird.', 'consistency_check_deadline', 10, 'Epic', '#7C3AED', '#DDD6FE', 1),

('Comeback', 'Du warst lange weg, doch jetzt bist du zurück. Ein echter Comeback Moment.', 'comeback_after_break', 7, 'Epic', '#7C3AED', '#DDD6FE', 0),
('Perfect Day', 'Ein perfekter Tag voller Produktivität. Schaffe 20 Tasks an nur einem Tag.', 'tasks_completed_single_day', 20, 'Epic', '#7C3AED', '#DDD6FE', 0),
('Overkill', 'Das war nicht einfach nur Fokus, das war Overkill. Arbeite 180 Minuten an einer einzigen Task.', 'long_task_duration', 180, 'Epic', '#7C3AED', '#DDD6FE', 0);