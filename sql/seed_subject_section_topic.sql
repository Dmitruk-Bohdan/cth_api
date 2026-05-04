-- =====================================================
-- SEED: Предметы, Секции, Топики (Русский язык)
-- Идемпотентный скрипт (можно запускать многократно)
-- =====================================================

BEGIN;

-- ------------------------------------------------------------------
-- 1. Предмет (SUBJECT) - русский язык с ID = 1
-- ------------------------------------------------------------------
INSERT INTO public.subject (id, name, is_deleted, created_at, last_update_at)
VALUES (1, 'Русский язык', false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    last_update_at = now(),
    is_deleted = false
WHERE public.subject.name IS DISTINCT FROM EXCLUDED.name;

-- ------------------------------------------------------------------
-- 2. Секции (SECTION) - соответствуют разделам из спецификации
-- ------------------------------------------------------------------
WITH subject_const AS (
    SELECT id FROM public.subject WHERE id = 1
)
INSERT INTO public.section (id, name, subject_id, is_deleted, created_at, last_update_at)
VALUES
    (1, 'Орфография', (SELECT id FROM subject_const), false, now(), now()),
    (2, 'Пунктуация', (SELECT id FROM subject_const), false, now(), now()),
    (3, 'Лексика', (SELECT id FROM subject_const), false, now(), now()),
    (4, 'Культура речи', (SELECT id FROM subject_const), false, now(), now()),
    (5, 'Фонетика', (SELECT id FROM subject_const), false, now(), now()),
    (6, 'Состав слова и словообразование', (SELECT id FROM subject_const), false, now(), now()),
    (7, 'Морфология', (SELECT id FROM subject_const), false, now(), now()),
    (8, 'Синтаксис', (SELECT id FROM subject_const), false, now(), now()),
    (9, 'Текст и стили речи', (SELECT id FROM subject_const), false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    subject_id = EXCLUDED.subject_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.section.name IS DISTINCT FROM EXCLUDED.name;

-- ------------------------------------------------------------------
-- 3. Топики (TOPIC) - темы внутри каждой секции
-- ------------------------------------------------------------------

-- Орфография (section_id = 1)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (1, 'Проверяемые безударные гласные в корне', 1, false, now(), now()),
    (2, 'Непроверяемые гласные в корне', 1, false, now(), now()),
    (3, 'Чередующиеся гласные о–а', 1, false, now(), now()),
    (4, 'Чередующиеся гласные е–и', 1, false, now(), now()),
    (5, 'О, Е, Ё после шипящих', 1, false, now(), now()),
    (6, 'О, Е после Ц', 1, false, now(), now()),
    (7, 'Ы, И после Ц', 1, false, now(), now()),
    (8, 'Правописание приставок', 1, false, now(), now()),
    (9, 'И, Ы после приставок на согласный', 1, false, now(), now()),
    (10, 'Разделительные Ь и Ъ', 1, false, now(), now()),
    (11, 'Ь после шипящих', 1, false, now(), now()),
    (12, 'Ь для обозначения мягкости согласного', 1, false, now(), now()),
    (13, 'Проверяемые согласные', 1, false, now(), now()),
    (14, 'Непроверяемые согласные', 1, false, now(), now()),
    (15, 'Непроизносимые согласные', 1, false, now(), now()),
    (16, 'Двойные согласные', 1, false, now(), now()),
    (17, 'Падежные окончания существительных', 1, false, now(), now()),
    (18, 'Суффиксы существительных', 1, false, now(), now()),
    (19, 'Суффиксы прилагательных', 1, false, now(), now()),
    (20, 'Гласные в падежных окончаниях прилагательных', 1, false, now(), now()),
    (21, 'Гласные в падежных окончаниях причастий', 1, false, now(), now()),
    (22, 'Безударные гласные в личных окончаниях глаголов', 1, false, now(), now()),
    (23, 'Суффиксы -ова- (-ева-), -ыва- (-ива-)', 1, false, now(), now()),
    (24, 'Глаголы прошедшего времени перед суффиксом -л-', 1, false, now(), now()),
    (25, '-ТСЯ и -ТЬСЯ в глаголах', 1, false, now(), now()),
    (26, 'Гласные в суффиксах причастий настоящего времени', 1, false, now(), now()),
    (27, 'Гласные перед суффиксом -вш- в причастиях', 1, false, now(), now()),
    (28, 'Буквы А (Я), Е перед Н и НН в причастиях', 1, false, now(), now()),
    (29, 'Гласная перед суффиксами -в-, -вши- в деепричастиях', 1, false, now(), now()),
    (30, 'Н и НН в словах разных частей речи', 1, false, now(), now()),
    (31, 'НЕ со словами разных частей речи', 1, false, now(), now()),
    (32, 'НИ со словами разных частей речи', 1, false, now(), now()),
    (33, 'Слитное и дефисное написание существительных', 1, false, now(), now()),
    (34, 'Слитное и дефисное написание прилагательных', 1, false, now(), now()),
    (35, 'Слитное, раздельное и дефисное написание наречий', 1, false, now(), now()),
    (36, 'Гласные на конце наречий', 1, false, now(), now()),
    (37, 'Правописание предлогов, союзов, частиц', 1, false, now(), now()),
    (38, 'Ь в середине и на конце числительных', 1, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Пунктуация (section_id = 2)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (101, 'Тире между подлежащим и сказуемым', 2, false, now(), now()),
    (102, 'Знаки препинания при однородных членах', 2, false, now(), now()),
    (103, 'Обобщающее слово при однородных членах', 2, false, now(), now()),
    (104, 'Обособленные определения', 2, false, now(), now()),
    (105, 'Обособленные приложения', 2, false, now(), now()),
    (106, 'Обособленные обстоятельства', 2, false, now(), now()),
    (107, 'Знаки препинания при обращениях', 2, false, now(), now()),
    (108, 'Знаки препинания при вводных словах', 2, false, now(), now()),
    (109, 'Конструкции с КАК', 2, false, now(), now()),
    (110, 'Знаки препинания в сложносочиненных предложениях (ССП)', 2, false, now(), now()),
    (111, 'Знаки препинания в СПП с одной придаточной', 2, false, now(), now()),
    (112, 'Знаки препинания в СПП с несколькими придаточными', 2, false, now(), now()),
    (113, 'Знаки препинания в бессоюзных предложениях (БСП)', 2, false, now(), now()),
    (114, 'Знаки препинания в сложных предложениях с разными видами связи', 2, false, now(), now()),
    (115, 'Знаки препинания при прямой речи', 2, false, now(), now()),
    (116, 'Знаки препинания при диалоге', 2, false, now(), now()),
    (117, 'Знаки препинания при цитировании', 2, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Лексика (section_id = 3)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (201, 'Лексическое значение слова', 3, false, now(), now()),
    (202, 'Прямое и переносное значение слова', 3, false, now(), now()),
    (203, 'Многозначные и однозначные слова', 3, false, now(), now()),
    (204, 'Синонимы', 3, false, now(), now()),
    (205, 'Антонимы', 3, false, now(), now()),
    (206, 'Омонимы', 3, false, now(), now()),
    (207, 'Фразеологические обороты', 3, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Культура речи (section_id = 4)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (301, 'Произносительная норма', 4, false, now(), now()),
    (302, 'Лексическая норма', 4, false, now(), now()),
    (303, 'Морфологическая норма', 4, false, now(), now()),
    (304, 'Синтаксическая норма', 4, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Фонетика (section_id = 5)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (401, 'Гласные и согласные звуки', 5, false, now(), now()),
    (402, 'Двойная роль букв Е, Ё, Ю, Я', 5, false, now(), now()),
    (403, 'Парные и непарные звонкие и глухие согласные', 5, false, now(), now()),
    (404, 'Оглушение звонких и озвончение глухих согласных', 5, false, now(), now()),
    (405, 'Парные и непарные твердые и мягкие согласные', 5, false, now(), now()),
    (406, 'Обозначение на письме мягкости согласных', 5, false, now(), now()),
    (407, 'Ударение', 5, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Состав слова и словообразование (section_id = 6)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (501, 'Морфемы – значимые части слова', 6, false, now(), now()),
    (502, 'Формообразовательные и словообразовательные морфемы', 6, false, now(), now()),
    (503, 'Основа слова', 6, false, now(), now()),
    (504, 'Корень, приставка, суффикс, постфикс', 6, false, now(), now()),
    (505, 'Окончание', 6, false, now(), now()),
    (506, 'Чередование звуков при образовании и изменении слов', 6, false, now(), now()),
    (507, 'Суффиксальный способ образования слов', 6, false, now(), now()),
    (508, 'Приставочный способ', 6, false, now(), now()),
    (509, 'Постфиксальный способ', 6, false, now(), now()),
    (510, 'Приставочно-суффиксальный способ', 6, false, now(), now()),
    (511, 'Сложение как способ словообразования', 6, false, now(), now()),
    (512, 'Сложение в сочетании с суффиксацией', 6, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Морфология (section_id = 7)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (601, 'Имя существительное (общее значение, признаки, роль)', 7, false, now(), now()),
    (602, 'Имя прилагательное', 7, false, now(), now()),
    (603, 'Имя числительное', 7, false, now(), now()),
    (604, 'Местоимение', 7, false, now(), now()),
    (605, 'Глагол', 7, false, now(), now()),
    (606, 'Наречие', 7, false, now(), now()),
    (607, 'Причастие как особая форма глагола', 7, false, now(), now()),
    (608, 'Деепричастие как особая форма глагола', 7, false, now(), now()),
    (609, 'Предлог как служебная часть речи', 7, false, now(), now()),
    (610, 'Союз (сочинительные и подчинительные)', 7, false, now(), now()),
    (611, 'Частица как служебная часть речи', 7, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Синтаксис (section_id = 8)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (701, 'Словосочетание', 8, false, now(), now()),
    (702, 'Виды связи слов в словосочетании (согласование, управление, примыкание)', 8, false, now(), now()),
    (703, 'Классификация простых предложений', 8, false, now(), now()),
    (704, 'Грамматическая основа предложения', 8, false, now(), now()),
    (705, 'Подлежащее и способы его выражения', 8, false, now(), now()),
    (706, 'Глагольное сказуемое (простое и составное)', 8, false, now(), now()),
    (707, 'Составное именное сказуемое', 8, false, now(), now()),
    (708, 'Второстепенные члены предложения', 8, false, now(), now()),
    (709, 'Односоставные предложения', 8, false, now(), now()),
    (710, 'Простое осложненное предложение', 8, false, now(), now()),
    (711, 'Сложносочиненное предложение (ССП)', 8, false, now(), now()),
    (712, 'Сложноподчиненное предложение (СПП)', 8, false, now(), now()),
    (713, 'Бессоюзное сложное предложение (БСП)', 8, false, now(), now()),
    (714, 'Сложные предложения с разными видами связи', 8, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

-- Текст и стили речи (section_id = 9)
INSERT INTO public.topic (id, name, section_id, is_deleted, created_at, last_update_at)
VALUES
    (801, 'Текст и его основные признаки', 9, false, now(), now()),
    (802, 'Тема и основная мысль текста', 9, false, now(), now()),
    (803, 'Подтемы текста', 9, false, now(), now()),
    (804, 'Средства связи предложений в тексте', 9, false, now(), now()),
    (805, 'Типы речи', 9, false, now(), now()),
    (806, 'Стили речи', 9, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    name = EXCLUDED.name,
    section_id = EXCLUDED.section_id,
    last_update_at = now(),
    is_deleted = false
WHERE public.topic.name IS DISTINCT FROM EXCLUDED.name;

COMMIT;