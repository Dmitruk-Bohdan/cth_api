-- =====================================================
-- SEED: Связи учитель-ученик, группы и состав групп
-- =====================================================

BEGIN;

-- ------------------------------------------------------------------
-- 1. СВЯЗИ УЧИТЕЛЬ-УЧЕНИК (teacher_student)
-- ------------------------------------------------------------------
INSERT INTO public.teacher_student (teacher_id, student_id, status, is_deleted, created_at, last_update_at)
VALUES
    (1, 6,  1, false, now(), now()),
    (1, 7,  1, false, now(), now()),
    (1, 8,  1, false, now(), now()),
    (1, 9,  1, false, now(), now()),
    (2, 10, 1, false, now(), now()),
    (2, 11, 1, false, now(), now()),
    (2, 12, 1, false, now(), now()),
    (2, 13, 1, false, now(), now()),
    (3, 14, 1, false, now(), now()),
    (3, 15, 1, false, now(), now()),
    (3, 16, 1, false, now(), now()),
    (3, 17, 1, false, now(), now()),
    (4, 18, 1, false, now(), now()),
    (4, 19, 1, false, now(), now()),
    (4, 20, 1, false, now(), now()),
    (4, 21, 1, false, now(), now()),
    (5, 22, 1, false, now(), now()),
    (5, 23, 1, false, now(), now()),
    (5, 24, 1, false, now(), now()),
    (5, 25, 1, false, now(), now());

-- ------------------------------------------------------------------
-- 2. ГРУППЫ (group)
-- ------------------------------------------------------------------
LOCK TABLE public."group" IN EXCLUSIVE MODE;
DELETE FROM public."group" WHERE id BETWEEN 1 AND 5;

INSERT INTO public."group" (id, teacher_id, name, subject_id, is_deleted, created_at, last_update_at)
VALUES
    (1, 1, 'Русский язык - Группа 1', 1, false, now(), now()),
    (2, 2, 'Русский язык - Группа 2', 1, false, now(), now()),
    (3, 3, 'Русский язык - Группа 3', 1, false, now(), now()),
    (4, 4, 'Русский язык - Группа 4', 1, false, now(), now()),
    (5, 5, 'Русский язык - Группа 5', 1, false, now(), now());

-- ------------------------------------------------------------------
-- 3. СОСТАВ ГРУПП (student_group_student)
-- ------------------------------------------------------------------
DELETE FROM public.student_group_student WHERE group_id BETWEEN 1 AND 5;

INSERT INTO public.student_group_student (group_id, student_id)
VALUES
    (1, 6), (1, 7), (1, 8),
    (2, 10), (2, 11), (2, 12),
    (3, 14), (3, 15), (3, 16),
    (4, 18), (4, 19), (4, 20),
    (5, 22), (5, 23), (5, 24);

-- ------------------------------------------------------------------
-- 4. СИНХРОНИЗАЦИЯ СЧЕТЧИКОВ SEQUENCE
-- ------------------------------------------------------------------
SELECT setval(
    pg_get_serial_sequence('teacher_student', 'id'),
    COALESCE((SELECT MAX(id) FROM public.teacher_student), 0) + 1,
    false
);

SELECT setval(
    pg_get_serial_sequence('"group"', 'id'),
    COALESCE((SELECT MAX(id) FROM public."group"), 5) + 1,
    false
);

COMMIT;

-- ПРОВЕРКА
SELECT 'teacher_student' as table_name, COUNT(*) as row_count FROM public.teacher_student
UNION ALL
SELECT '"group"', COUNT(*) FROM public."group"
UNION ALL
SELECT 'student_group_student', COUNT(*) FROM public.student_group_student;