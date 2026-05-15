-- =====================================================
-- SEED: Назначения (ASSIGNMENTS)
-- Каждый учитель назначает каждому ученику по 3 теста
-- Каждый учитель назначает каждой группе по 3 теста
-- 
-- Структура:
--   StudentAssignment: индивидуальные назначения
--   GroupAssignment: групповые назначения (с автоматическим созданием StudentAssignment для членов группы)
-- =====================================================

BEGIN;

-- =====================================================
-- 1. ИНДИВИДУАЛЬНЫЕ НАЗНАЧЕНИЯ (StudentAssignment)
-- Каждый учитель -> каждому из 4 своих учеников -> 3 теста
-- 
-- Учитель 1 (id=1): ученики 6,7,8,9
-- Учитель 2 (id=2): ученики 10,11,12,13
-- Учитель 3 (id=3): ученики 14,15,16,17
-- Учитель 4 (id=4): ученики 18,19,20,21
-- Учитель 5 (id=5): ученики 22,23,24,25
-- =====================================================

-- ------------------------------------------------------------------
-- 1.1 УЧИТЕЛЬ 1 (teacher_id=1) -> ученики 6,7,8,9
-- Тесты учителя 1: 
--   test_id=1 (публичный), test_id=2 (приватный), test_id=3 (черновик, не опубликован)
--   Также использует чужие публичные тесты: test_id=4,7,10,13
-- ------------------------------------------------------------------

-- Ученик 6 (student_1)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (6, 1, 1, NULL, now() + interval '7 days', 3, now(), now(), false),   -- свой публичный
    (6, 1, 2, NULL, now() + interval '14 days', 2, now(), now(), false),  -- свой приватный
    (6, 1, 4, NULL, now() + interval '5 days', 1, now(), now(), false);   -- чужой публичный (учитель2)

-- Ученик 7 (student_2)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (7, 1, 1, NULL, now() + interval '10 days', 3, now(), now(), false),
    (7, 1, 3, NULL, now() + interval '3 days', 1, now(), now(), false),   -- черновик
    (7, 1, 7, NULL, now() + interval '20 days', 3, now(), now(), false);  -- чужой публичный (учитель3)

-- Ученик 8 (student_3)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (8, 1, 2, NULL, now() + interval '12 days', 2, now(), now(), false),
    (8, 1, 4, NULL, now() + interval '8 days', 3, now(), now(), false),
    (8, 1, 10, NULL, now() + interval '15 days', 2, now(), now(), false); -- чужой публичный (учитель4)

-- Ученик 9 (student_4) - вне группы, только индивидуальные
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (9, 1, 1, NULL, now() + interval '6 days', 3, now(), now(), false),
    (9, 1, 3, NULL, now() + interval '4 days', 1, now(), now(), false),
    (9, 1, 13, NULL, now() + interval '25 days', 3, now(), now(), false); -- чужой публичный (учитель5)

-- ------------------------------------------------------------------
-- 1.2 УЧИТЕЛЬ 2 (teacher_id=2) -> ученики 10,11,12,13
-- Тесты учителя 2: test_id=4 (публичный), test_id=5 (приватный), test_id=6 (черновик)
-- ------------------------------------------------------------------

-- Ученик 10 (student_5)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (10, 2, 4, NULL, now() + interval '7 days', 3, now(), now(), false),
    (10, 2, 5, NULL, now() + interval '14 days', 2, now(), now(), false),
    (10, 2, 7, NULL, now() + interval '10 days', 3, now(), now(), false); -- чужой (учитель3)

-- Ученик 11 (student_6)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (11, 2, 4, NULL, now() + interval '9 days', 3, now(), now(), false),
    (11, 2, 6, NULL, now() + interval '5 days', 1, now(), now(), false),
    (11, 2, 1, NULL, now() + interval '12 days', 2, now(), now(), false); -- чужой (учитель1)

-- Ученик 12 (student_7)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (12, 2, 5, NULL, now() + interval '11 days', 2, now(), now(), false),
    (12, 2, 7, NULL, now() + interval '8 days', 3, now(), now(), false),
    (12, 2, 13, NULL, now() + interval '18 days', 2, now(), now(), false); -- чужой (учитель5)

-- Ученик 13 (student_8) - вне группы
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (13, 2, 4, NULL, now() + interval '6 days', 3, now(), now(), false),
    (13, 2, 6, NULL, now() + interval '3 days', 1, now(), now(), false),
    (13, 2, 10, NULL, now() + interval '20 days', 3, now(), now(), false); -- чужой (учитель4)

-- ------------------------------------------------------------------
-- 1.3 УЧИТЕЛЬ 3 (teacher_id=3) -> ученики 14,15,16,17
-- Тесты учителя 3: test_id=7 (публичный), test_id=8 (приватный), test_id=9 (черновик)
-- ------------------------------------------------------------------

-- Ученик 14 (student_9)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (14, 3, 7, NULL, now() + interval '8 days', 3, now(), now(), false),
    (14, 3, 8, NULL, now() + interval '13 days', 2, now(), now(), false),
    (14, 3, 1, NULL, now() + interval '11 days', 3, now(), now(), false); -- чужой (учитель1)

-- Ученик 15 (student_10)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (15, 3, 7, NULL, now() + interval '10 days', 3, now(), now(), false),
    (15, 3, 9, NULL, now() + interval '4 days', 1, now(), now(), false),
    (15, 3, 4, NULL, now() + interval '14 days', 2, now(), now(), false); -- чужой (учитель2)

-- Ученик 16 (student_11)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (16, 3, 8, NULL, now() + interval '12 days', 2, now(), now(), false),
    (16, 3, 1, NULL, now() + interval '9 days', 3, now(), now(), false),
    (16, 3, 13, NULL, now() + interval '16 days', 2, now(), now(), false); -- чужой (учитель5)

-- Ученик 17 (student_12) - вне группы
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (17, 3, 7, NULL, now() + interval '5 days', 3, now(), now(), false),
    (17, 3, 9, NULL, now() + interval '3 days', 1, now(), now(), false),
    (17, 3, 10, NULL, now() + interval '22 days', 3, now(), now(), false); -- чужой (учитель4)

-- ------------------------------------------------------------------
-- 1.4 УЧИТЕЛЬ 4 (teacher_id=4) -> ученики 18,19,20,21
-- Тесты учителя 4: test_id=10 (публичный), test_id=11 (приватный), test_id=12 (черновик)
-- ------------------------------------------------------------------

-- Ученик 18 (student_13)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (18, 4, 10, NULL, now() + interval '9 days', 3, now(), now(), false),
    (18, 4, 11, NULL, now() + interval '15 days', 2, now(), now(), false),
    (18, 4, 4, NULL, now() + interval '7 days', 3, now(), now(), false); -- чужой (учитель2)

-- Ученик 19 (student_14)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (19, 4, 10, NULL, now() + interval '11 days', 3, now(), now(), false),
    (19, 4, 12, NULL, now() + interval '6 days', 1, now(), now(), false),
    (19, 4, 7, NULL, now() + interval '13 days', 2, now(), now(), false); -- чужой (учитель3)

-- Ученик 20 (student_15)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (20, 4, 11, NULL, now() + interval '14 days', 2, now(), now(), false),
    (20, 4, 4, NULL, now() + interval '10 days', 3, now(), now(), false),
    (20, 4, 1, NULL, now() + interval '18 days', 2, now(), now(), false); -- чужой (учитель1)

-- Ученик 21 (student_16) - вне группы
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (21, 4, 10, NULL, now() + interval '7 days', 3, now(), now(), false),
    (21, 4, 12, NULL, now() + interval '4 days', 1, now(), now(), false),
    (21, 4, 13, NULL, now() + interval '21 days', 3, now(), now(), false); -- чужой (учитель5)

-- ------------------------------------------------------------------
-- 1.5 УЧИТЕЛЬ 5 (teacher_id=5) -> ученики 22,23,24,25
-- Тесты учителя 5: test_id=13 (публичный), test_id=14 (приватный), test_id=15 (черновик)
-- ------------------------------------------------------------------

-- Ученик 22 (student_17)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (22, 5, 13, NULL, now() + interval '10 days', 3, now(), now(), false),
    (22, 5, 14, NULL, now() + interval '16 days', 2, now(), now(), false),
    (22, 5, 7, NULL, now() + interval '12 days', 3, now(), now(), false); -- чужой (учитель3)

-- Ученик 23 (student_18)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (23, 5, 13, NULL, now() + interval '8 days', 3, now(), now(), false),
    (23, 5, 15, NULL, now() + interval '5 days', 1, now(), now(), false),
    (23, 5, 10, NULL, now() + interval '15 days', 2, now(), now(), false); -- чужой (учитель4)

-- Ученик 24 (student_19)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (24, 5, 14, NULL, now() + interval '13 days', 2, now(), now(), false),
    (24, 5, 7, NULL, now() + interval '9 days', 3, now(), now(), false),
    (24, 5, 1, NULL, now() + interval '17 days', 2, now(), now(), false); -- чужой (учитель1)

-- Ученик 25 (student_20) - вне группы
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
VALUES
    (25, 5, 13, NULL, now() + interval '6 days', 3, now(), now(), false),
    (25, 5, 15, NULL, now() + interval '3 days', 1, now(), now(), false),
    (25, 5, 4, NULL, now() + interval '24 days', 3, now(), now(), false); -- чужой (учитель2)

-- =====================================================
-- 2. ГРУППОВЫЕ НАЗНАЧЕНИЯ (GroupAssignment)
-- Каждый учитель -> своей группе -> 3 теста
-- 
-- Группы:
--   group_id=1 (учитель1): ученики 6,7,8
--   group_id=2 (учитель2): ученики 10,11,12
--   group_id=3 (учитель3): ученики 14,15,16
--   group_id=4 (учитель4): ученики 18,19,20
--   group_id=5 (учитель5): ученики 22,23,24
-- =====================================================

-- ------------------------------------------------------------------
-- 2.1 УЧИТЕЛЬ 1 -> ГРУППА 1 (group_id=1)
-- ------------------------------------------------------------------
INSERT INTO public.group_assignment (group_id, teacher_id, test_id, expired_at, default_attempts_allowed, created_at, last_update_at, is_deleted)
VALUES
    (1, 1, 1, now() + interval '14 days', 3, now(), now(), false),   -- публичный
    (1, 1, 2, now() + interval '21 days', 2, now(), now(), false),   -- приватный
    (1, 1, 4, now() + interval '10 days', 3, now(), now(), false);   -- чужой публичный

-- Создаем StudentAssignment для каждого члена группы (автоматически через триггер, но для сида делаем явно)
-- Группа 1: ученики 6,7,8
WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 1 AND teacher_id = 1 AND test_id = 1 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 1, 1, (SELECT id FROM ga), now() + interval '14 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 1;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 1 AND teacher_id = 1 AND test_id = 2 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 1, 2, (SELECT id FROM ga), now() + interval '21 days', 2, now(), now(), false
FROM public.student_group_student WHERE group_id = 1;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 1 AND teacher_id = 1 AND test_id = 4 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 1, 4, (SELECT id FROM ga), now() + interval '10 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 1;

-- ------------------------------------------------------------------
-- 2.2 УЧИТЕЛЬ 2 -> ГРУППА 2 (group_id=2)
-- ------------------------------------------------------------------
INSERT INTO public.group_assignment (group_id, teacher_id, test_id, expired_at, default_attempts_allowed, created_at, last_update_at, is_deleted)
VALUES
    (2, 2, 4, now() + interval '14 days', 3, now(), now(), false),
    (2, 2, 5, now() + interval '20 days', 2, now(), now(), false),
    (2, 2, 7, now() + interval '12 days', 3, now(), now(), false);

-- Группа 2: ученики 10,11,12
WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 2 AND teacher_id = 2 AND test_id = 4 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 2, 4, (SELECT id FROM ga), now() + interval '14 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 2;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 2 AND teacher_id = 2 AND test_id = 5 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 2, 5, (SELECT id FROM ga), now() + interval '20 days', 2, now(), now(), false
FROM public.student_group_student WHERE group_id = 2;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 2 AND teacher_id = 2 AND test_id = 7 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 2, 7, (SELECT id FROM ga), now() + interval '12 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 2;

-- ------------------------------------------------------------------
-- 2.3 УЧИТЕЛЬ 3 -> ГРУППА 3 (group_id=3)
-- ------------------------------------------------------------------
INSERT INTO public.group_assignment (group_id, teacher_id, test_id, expired_at, default_attempts_allowed, created_at, last_update_at, is_deleted)
VALUES
    (3, 3, 7, now() + interval '15 days', 3, now(), now(), false),
    (3, 3, 8, now() + interval '18 days', 2, now(), now(), false),
    (3, 3, 1, now() + interval '11 days', 3, now(), now(), false);

-- Группа 3: ученики 14,15,16
WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 3 AND teacher_id = 3 AND test_id = 7 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 3, 7, (SELECT id FROM ga), now() + interval '15 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 3;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 3 AND teacher_id = 3 AND test_id = 8 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 3, 8, (SELECT id FROM ga), now() + interval '18 days', 2, now(), now(), false
FROM public.student_group_student WHERE group_id = 3;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 3 AND teacher_id = 3 AND test_id = 1 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 3, 1, (SELECT id FROM ga), now() + interval '11 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 3;

-- ------------------------------------------------------------------
-- 2.4 УЧИТЕЛЬ 4 -> ГРУППА 4 (group_id=4)
-- ------------------------------------------------------------------
INSERT INTO public.group_assignment (group_id, teacher_id, test_id, expired_at, default_attempts_allowed, created_at, last_update_at, is_deleted)
VALUES
    (4, 4, 10, now() + interval '13 days', 3, now(), now(), false),
    (4, 4, 11, now() + interval '22 days', 2, now(), now(), false),
    (4, 4, 4, now() + interval '9 days', 3, now(), now(), false);

-- Группа 4: ученики 18,19,20
WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 4 AND teacher_id = 4 AND test_id = 10 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 4, 10, (SELECT id FROM ga), now() + interval '13 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 4;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 4 AND teacher_id = 4 AND test_id = 11 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 4, 11, (SELECT id FROM ga), now() + interval '22 days', 2, now(), now(), false
FROM public.student_group_student WHERE group_id = 4;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 4 AND teacher_id = 4 AND test_id = 4 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 4, 4, (SELECT id FROM ga), now() + interval '9 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 4;

-- ------------------------------------------------------------------
-- 2.5 УЧИТЕЛЬ 5 -> ГРУППА 5 (group_id=5)
-- ------------------------------------------------------------------
INSERT INTO public.group_assignment (group_id, teacher_id, test_id, expired_at, default_attempts_allowed, created_at, last_update_at, is_deleted)
VALUES
    (5, 5, 13, now() + interval '16 days', 3, now(), now(), false),
    (5, 5, 14, now() + interval '19 days', 2, now(), now(), false),
    (5, 5, 7, now() + interval '13 days', 3, now(), now(), false);

-- Группа 5: ученики 22,23,24
WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 5 AND teacher_id = 5 AND test_id = 13 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 5, 13, (SELECT id FROM ga), now() + interval '16 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 5;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 5 AND teacher_id = 5 AND test_id = 14 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 5, 14, (SELECT id FROM ga), now() + interval '19 days', 2, now(), now(), false
FROM public.student_group_student WHERE group_id = 5;

WITH ga AS (SELECT id FROM public.group_assignment WHERE group_id = 5 AND teacher_id = 5 AND test_id = 7 AND is_deleted = false)
INSERT INTO public.student_assignment (student_id, teacher_id, test_id, group_assignment_id, expired_at, attempts_left, created_at, last_update_at, is_deleted)
SELECT student_id, 5, 7, (SELECT id FROM ga), now() + interval '13 days', 3, now(), now(), false
FROM public.student_group_student WHERE group_id = 5;

-- =====================================================
-- 3. СИНХРОНИЗАЦИЯ СЧЕТЧИКОВ SEQUENCE
-- =====================================================
SELECT setval(
    pg_get_serial_sequence('public.student_assignment', 'id'),
    COALESCE((SELECT MAX(id) FROM public.student_assignment), 0) + 1,
    false
);

SELECT setval(
    pg_get_serial_sequence('public.group_assignment', 'id'),
    COALESCE((SELECT MAX(id) FROM public.group_assignment), 0) + 1,
    false
);

COMMIT;

-- =====================================================
-- РЕЗУЛЬТАТ:
-- =====================================================
-- Индивидуальные назначения:
--   5 учителей × 4 ученика × 3 теста = 60 назначений
--
-- Групповые назначения:
--   5 учителей × 1 группа × 3 теста = 15 групповых назначений
--   Каждое групповое назначение создает 3 StudentAssignment (для членов группы)
--   +45 StudentAssignment от групп
--
-- Всего StudentAssignment: 60 (индивидуальные) + 45 (от групп) = 105
-- Всего GroupAssignment: 15
--
-- Для тестирования эндпоинтов:
--   POST   /assignments/students     - индивидуальное назначение
--   POST   /assignments/groups       - групповое назначение
--   PATCH  /assignments/             - изменение дедлайна/попыток
--   DELETE /assignments/             - отзыв назначения
--   GET    /assignments/teacher/me   - мои назначения (учитель)
--   GET    /assignments/student/me   - мои назначения (ученик)
--   GET    /assignments/student/details/{id} - детали назначения
--   GET    /assignments/student/{id}/list - назначения ученика (учитель)
--   GET    /assignments/group/details/{id} - детали группового назначения
--   GET    /assignments/group/{id}/list - назначения группы
--   GET    /assignments/group-score/{id} - статистика по группе
--   GET    /assignments/student-score/{id} - статистика по ученику
-- =====================================================