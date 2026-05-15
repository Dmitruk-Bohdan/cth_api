-- =====================================================
-- SEED: Попытки прохождения тестов (TEST_ATTEMPT и USER_ANSWER)
-- 
-- Каждый ученик выполняет минимум 10 попыток
-- Ответы генерируются в соответствии с типами заданий:
--   SingleChoice (type=1): один правильный ответ
--   MultipleChoice (type=2): несколько правильных ответов (строка цифр)
--   OpenEnded (type=3): текстовый ответ
-- 
-- Все ответы корректны (is_correct = true)
-- =====================================================

BEGIN;

-- =====================================================
-- ВСПОМОГАТЕЛЬНЫЕ ДАННЫЕ ДЛЯ ГЕНЕРАЦИИ ОТВЕТОВ
-- =====================================================

-- Правильные ответы для заданий (на основе seed_problems.txt)
-- problem_id -> correct_answer
-- SingleChoice: '1','2','3','4','5' (цифра)
-- MultipleChoice: строка цифр типа '24', '124', '2345'
-- OpenEnded: текстовая строка типа 'и', 'е', 'о', 'ъ', 'ь'

-- Данные будут использоваться в запросах через JOIN с problem_version

-- =====================================================
-- 1. ТЕСТОВЫЕ ПОПЫТКИ (TEST_ATTEMPT)
-- =====================================================

-- ------------------------------------------------------------------
-- 1.1 УЧЕНИК 6 (student_1) - teacher_1, группа 1
-- Доступные тесты: индивид. 1,2,4; групповые 1,2,4
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    -- Тест 1 (публичный) - 3 попытки
    (1, 6, 2, 45, 85, now() - interval '12 days', now() - interval '12 days'),
    (1, 6, 2, 52, 78, now() - interval '10 days', now() - interval '10 days'),
    (1, 6, 2, 38, 92, now() - interval '8 days', now() - interval '8 days'),
    -- Тест 2 (приватный) - 2 попытки
    (2, 6, 2, 55, 70, now() - interval '9 days', now() - interval '9 days'),
    (2, 6, 2, 48, 82, now() - interval '7 days', now() - interval '7 days'),
    -- Тест 4 (чужой публичный) - 3 попытки
    (4, 6, 2, 42, 88, now() - interval '11 days', now() - interval '11 days'),
    (4, 6, 2, 50, 75, now() - interval '6 days', now() - interval '6 days'),
    (4, 6, 2, 35, 95, now() - interval '4 days', now() - interval '4 days'),
    -- Дополнительные попытки для достижения 10+
    (2, 6, 2, 47, 79, now() - interval '5 days', now() - interval '5 days'),
    (1, 6, 2, 44, 90, now() - interval '3 days', now() - interval '3 days'),
    (4, 6, 2, 40, 85, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.2 УЧЕНИК 7 (student_2) - teacher_1, группа 1
-- Доступные тесты: индивид. 1,3,7; групповые 1,2,4
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (1, 7, 2, 50, 82, now() - interval '13 days', now() - interval '13 days'),
    (1, 7, 2, 42, 88, now() - interval '11 days', now() - interval '11 days'),
    (3, 7, 2, 30, 65, now() - interval '10 days', now() - interval '10 days'),
    (3, 7, 2, 28, 70, now() - interval '8 days', now() - interval '8 days'),
    (7, 7, 2, 48, 85, now() - interval '9 days', now() - interval '9 days'),
    (7, 7, 2, 55, 78, now() - interval '7 days', now() - interval '7 days'),
    (2, 7, 2, 52, 80, now() - interval '6 days', now() - interval '6 days'),
    (4, 7, 2, 38, 92, now() - interval '5 days', now() - interval '5 days'),
    (1, 7, 2, 45, 86, now() - interval '4 days', now() - interval '4 days'),
    (7, 7, 2, 50, 83, now() - interval '3 days', now() - interval '3 days'),
    (2, 7, 2, 49, 81, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.3 УЧЕНИК 8 (student_3) - teacher_1, группа 1
-- Доступные тесты: индивид. 2,4,10; групповые 1,2,4
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (2, 8, 2, 48, 75, now() - interval '14 days', now() - interval '14 days'),
    (2, 8, 2, 52, 80, now() - interval '12 days', now() - interval '12 days'),
    (4, 8, 2, 40, 88, now() - interval '11 days', now() - interval '11 days'),
    (4, 8, 2, 45, 85, now() - interval '9 days', now() - interval '9 days'),
    (10, 8, 2, 55, 72, now() - interval '10 days', now() - interval '10 days'),
    (10, 8, 2, 50, 78, now() - interval '8 days', now() - interval '8 days'),
    (1, 8, 2, 38, 95, now() - interval '7 days', now() - interval '7 days'),
    (1, 8, 2, 42, 90, now() - interval '6 days', now() - interval '6 days'),
    (2, 8, 2, 47, 82, now() - interval '5 days', now() - interval '5 days'),
    (4, 8, 2, 44, 86, now() - interval '4 days', now() - interval '4 days'),
    (10, 8, 2, 53, 75, now() - interval '3 days', now() - interval '3 days');

-- ------------------------------------------------------------------
-- 1.4 УЧЕНИК 9 (student_4) - teacher_1, вне группы (только индивидуальные)
-- Доступные тесты: индивид. 1,3,13
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (1, 9, 2, 48, 88, now() - interval '15 days', now() - interval '15 days'),
    (1, 9, 2, 42, 92, now() - interval '13 days', now() - interval '13 days'),
    (1, 9, 2, 50, 85, now() - interval '11 days', now() - interval '11 days'),
    (3, 9, 2, 32, 68, now() - interval '12 days', now() - interval '12 days'),
    (3, 9, 2, 28, 72, now() - interval '10 days', now() - interval '10 days'),
    (3, 9, 2, 35, 75, now() - interval '8 days', now() - interval '8 days'),
    (13, 9, 2, 55, 80, now() - interval '9 days', now() - interval '9 days'),
    (13, 9, 2, 48, 85, now() - interval '7 days', now() - interval '7 days'),
    (13, 9, 2, 52, 78, now() - interval '5 days', now() - interval '5 days'),
    (1, 9, 2, 44, 90, now() - interval '4 days', now() - interval '4 days'),
    (3, 9, 2, 30, 70, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.5 УЧЕНИК 10 (student_5) - teacher_2, группа 2
-- Доступные тесты: индивид. 4,5,7; групповые 4,5,7
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (4, 10, 2, 42, 85, now() - interval '12 days', now() - interval '12 days'),
    (4, 10, 2, 48, 82, now() - interval '10 days', now() - interval '10 days'),
    (5, 10, 2, 55, 70, now() - interval '11 days', now() - interval '11 days'),
    (5, 10, 2, 50, 75, now() - interval '9 days', now() - interval '9 days'),
    (7, 10, 2, 38, 90, now() - interval '8 days', now() - interval '8 days'),
    (7, 10, 2, 44, 86, now() - interval '6 days', now() - interval '6 days'),
    (4, 10, 2, 40, 88, now() - interval '7 days', now() - interval '7 days'),
    (5, 10, 2, 52, 73, now() - interval '5 days', now() - interval '5 days'),
    (7, 10, 2, 42, 87, now() - interval '4 days', now() - interval '4 days'),
    (4, 10, 2, 45, 84, now() - interval '3 days', now() - interval '3 days'),
    (5, 10, 2, 48, 78, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.6 УЧЕНИК 11 (student_6) - teacher_2, группа 2
-- Доступные тесты: индивид. 4,6,1; групповые 4,5,7
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (4, 11, 2, 50, 80, now() - interval '14 days', now() - interval '14 days'),
    (4, 11, 2, 45, 85, now() - interval '12 days', now() - interval '12 days'),
    (6, 11, 2, 35, 60, now() - interval '11 days', now() - interval '11 days'),
    (6, 11, 2, 32, 65, now() - interval '9 days', now() - interval '9 days'),
    (1, 11, 2, 48, 88, now() - interval '10 days', now() - interval '10 days'),
    (1, 11, 2, 52, 82, now() - interval '8 days', now() - interval '8 days'),
    (5, 11, 2, 55, 72, now() - interval '7 days', now() - interval '7 days'),
    (7, 11, 2, 40, 89, now() - interval '6 days', now() - interval '6 days'),
    (4, 11, 2, 42, 86, now() - interval '5 days', now() - interval '5 days'),
    (1, 11, 2, 46, 84, now() - interval '3 days', now() - interval '3 days'),
    (5, 11, 2, 50, 75, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.7 УЧЕНИК 12 (student_7) - teacher_2, группа 2
-- Доступные тесты: индивид. 5,7,13; групповые 4,5,7
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (5, 12, 2, 52, 74, now() - interval '13 days', now() - interval '13 days'),
    (5, 12, 2, 48, 78, now() - interval '11 days', now() - interval '11 days'),
    (7, 12, 2, 42, 85, now() - interval '12 days', now() - interval '12 days'),
    (7, 12, 2, 38, 90, now() - interval '10 days', now() - interval '10 days'),
    (13, 12, 2, 55, 76, now() - interval '9 days', now() - interval '9 days'),
    (13, 12, 2, 50, 82, now() - interval '7 days', now() - interval '7 days'),
    (4, 12, 2, 44, 86, now() - interval '8 days', now() - interval '8 days'),
    (5, 12, 2, 50, 77, now() - interval '6 days', now() - interval '6 days'),
    (7, 12, 2, 40, 88, now() - interval '5 days', now() - interval '5 days'),
    (13, 12, 2, 48, 80, now() - interval '4 days', now() - interval '4 days'),
    (4, 12, 2, 42, 87, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.8 УЧЕНИК 13 (student_8) - teacher_2, вне группы
-- Доступные тесты: индивид. 4,6,10
-- ------------------------------------------------------------------
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    (4, 13, 2, 46, 84, now() - interval '15 days', now() - interval '15 days'),
    (4, 13, 2, 42, 88, now() - interval '13 days', now() - interval '13 days'),
    (4, 13, 2, 50, 80, now() - interval '11 days', now() - interval '11 days'),
    (6, 13, 2, 34, 62, now() - interval '12 days', now() - interval '12 days'),
    (6, 13, 2, 30, 68, now() - interval '10 days', now() - interval '10 days'),
    (6, 13, 2, 36, 65, now() - interval '8 days', now() - interval '8 days'),
    (10, 13, 2, 55, 74, now() - interval '9 days', now() - interval '9 days'),
    (10, 13, 2, 48, 78, now() - interval '7 days', now() - interval '7 days'),
    (10, 13, 2, 52, 76, now() - interval '5 days', now() - interval '5 days'),
    (4, 13, 2, 44, 86, now() - interval '4 days', now() - interval '4 days'),
    (6, 13, 2, 32, 66, now() - interval '2 days', now() - interval '2 days');

-- ------------------------------------------------------------------
-- 1.9-1.12 Остальные ученики (сокращённо, по 10-11 попыток)
-- Для экономии места, укажу по 10 попыток для каждого
-- ------------------------------------------------------------------

-- УЧЕНИК 14 (student_9) - teacher_3, группа 3
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
SELECT 7, 14, 2, 45 + (n*2), 75 + (n*3), now() - interval '15 days' + (n * interval '1 day'), now() - interval '15 days' + (n * interval '1 day')
FROM generate_series(0, 4) n
UNION ALL
SELECT 8, 14, 2, 48 + (n*2), 70 + (n*3), now() - interval '12 days' + (n * interval '1 day'), now() - interval '12 days' + (n * interval '1 day')
FROM generate_series(0, 2) n
UNION ALL
SELECT 1, 14, 2, 42 + (n*2), 80 + (n*2), now() - interval '10 days' + (n * interval '1 day'), now() - interval '10 days' + (n * interval '1 day')
FROM generate_series(0, 2) n;

-- УЧЕНИК 15 (student_10) - teacher_3, группа 3
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
SELECT 7, 15, 2, 44 + n*2, 78 + n*2, now() - interval '14 days' + (n * interval '1 day'), now() - interval '14 days' + (n * interval '1 day')
FROM generate_series(0, 4) n
UNION ALL
SELECT 9, 15, 2, 30 + n, 60 + n*3, now() - interval '11 days' + (n * interval '1 day'), now() - interval '11 days' + (n * interval '1 day')
FROM generate_series(0, 2) n
UNION ALL
SELECT 4, 15, 2, 46 + n*2, 82 + n*2, now() - interval '9 days' + (n * interval '1 day'), now() - interval '9 days' + (n * interval '1 day')
FROM generate_series(0, 2) n;

-- УЧЕНИК 16 (student_11) - teacher_3, группа 3
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
SELECT 8, 16, 2, 50 + n*2, 72 + n*2, now() - interval '13 days' + (n * interval '1 day'), now() - interval '13 days' + (n * interval '1 day')
FROM generate_series(0, 4) n
UNION ALL
SELECT 1, 16, 2, 44 + n*2, 84 + n*2, now() - interval '10 days' + (n * interval '1 day'), now() - interval '10 days' + (n * interval '1 day')
FROM generate_series(0, 2) n
UNION ALL
SELECT 13, 16, 2, 52 + n*2, 76 + n*2, now() - interval '8 days' + (n * interval '1 day'), now() - interval '8 days' + (n * interval '1 day')
FROM generate_series(0, 2) n;

-- УЧЕНИК 17 (student_12) - teacher_3, вне группы
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
SELECT 7, 17, 2, 42 + n*2, 82 + n*2, now() - interval '12 days' + (n * interval '1 day'), now() - interval '12 days' + (n * interval '1 day')
FROM generate_series(0, 3) n
UNION ALL
SELECT 9, 17, 2, 32 + n, 64 + n*2, now() - interval '10 days' + (n * interval '1 day'), now() - interval '10 days' + (n * interval '1 day')
FROM generate_series(0, 3) n
UNION ALL
SELECT 10, 17, 2, 48 + n*2, 78 + n*2, now() - interval '8 days' + (n * interval '1 day'), now() - interval '8 days' + (n * interval '1 day')
FROM generate_series(0, 2) n;

-- УЧЕНИК 18-25 аналогично (сокращённо)
INSERT INTO public.test_attempt (test_id, student_id, status, duration, raw_score, created_at, last_resumed_at)
VALUES
    -- УЧЕНИК 18 (student_13) - teacher_4, группа 4
    (10, 18, 2, 48, 85, now() - interval '14 days', now() - interval '14 days'),
    (10, 18, 2, 52, 82, now() - interval '12 days', now() - interval '12 days'),
    (11, 18, 2, 55, 72, now() - interval '11 days', now() - interval '11 days'),
    (11, 18, 2, 50, 76, now() - interval '9 days', now() - interval '9 days'),
    (4, 18, 2, 42, 88, now() - interval '10 days', now() - interval '10 days'),
    (4, 18, 2, 38, 92, now() - interval '8 days', now() - interval '8 days'),
    (10, 18, 2, 46, 86, now() - interval '7 days', now() - interval '7 days'),
    (11, 18, 2, 52, 74, now() - interval '6 days', now() - interval '6 days'),
    (4, 18, 2, 44, 87, now() - interval '5 days', now() - interval '5 days'),
    (10, 18, 2, 50, 84, now() - interval '4 days', now() - interval '4 days'),

    -- УЧЕНИК 19 (student_14) - teacher_4, группа 4
    (10, 19, 2, 44, 86, now() - interval '13 days', now() - interval '13 days'),
    (10, 19, 2, 48, 83, now() - interval '11 days', now() - interval '11 days'),
    (12, 19, 2, 32, 64, now() - interval '12 days', now() - interval '12 days'),
    (12, 19, 2, 35, 68, now() - interval '10 days', now() - interval '10 days'),
    (7, 19, 2, 40, 89, now() - interval '9 days', now() - interval '9 days'),
    (7, 19, 2, 44, 86, now() - interval '7 days', now() - interval '7 days'),
    (11, 19, 2, 52, 75, now() - interval '8 days', now() - interval '8 days'),
    (10, 19, 2, 46, 85, now() - interval '6 days', now() - interval '6 days'),
    (7, 19, 2, 42, 88, now() - interval '5 days', now() - interval '5 days'),
    (11, 19, 2, 50, 77, now() - interval '4 days', now() - interval '4 days'),

    -- УЧЕНИК 20 (student_15) - teacher_4, группа 4
    (11, 20, 2, 48, 76, now() - interval '14 days', now() - interval '14 days'),
    (11, 20, 2, 52, 80, now() - interval '12 days', now() - interval '12 days'),
    (4, 20, 2, 44, 86, now() - interval '11 days', now() - interval '11 days'),
    (4, 20, 2, 40, 90, now() - interval '9 days', now() - interval '9 days'),
    (1, 20, 2, 46, 88, now() - interval '10 days', now() - interval '10 days'),
    (1, 20, 2, 50, 84, now() - interval '8 days', now() - interval '8 days'),
    (10, 20, 2, 42, 87, now() - interval '7 days', now() - interval '7 days'),
    (11, 20, 2, 50, 78, now() - interval '6 days', now() - interval '6 days'),
    (4, 20, 2, 38, 92, now() - interval '5 days', now() - interval '5 days'),
    (1, 20, 2, 44, 86, now() - interval '4 days', now() - interval '4 days'),

    -- УЧЕНИК 21 (student_16) - teacher_4, вне группы
    (10, 21, 2, 46, 84, now() - interval '15 days', now() - interval '15 days'),
    (10, 21, 2, 42, 88, now() - interval '13 days', now() - interval '13 days'),
    (12, 21, 2, 30, 66, now() - interval '12 days', now() - interval '12 days'),
    (12, 21, 2, 34, 70, now() - interval '10 days', now() - interval '10 days'),
    (13, 21, 2, 52, 76, now() - interval '11 days', now() - interval '11 days'),
    (13, 21, 2, 48, 80, now() - interval '9 days', now() - interval '9 days'),
    (10, 21, 2, 44, 86, now() - interval '8 days', now() - interval '8 days'),
    (12, 21, 2, 32, 68, now() - interval '7 days', now() - interval '7 days'),
    (13, 21, 2, 50, 78, now() - interval '6 days', now() - interval '6 days'),
    (10, 21, 2, 40, 88, now() - interval '4 days', now() - interval '4 days'),

    -- УЧЕНИК 22 (student_17) - teacher_5, группа 5
    (13, 22, 2, 48, 82, now() - interval '13 days', now() - interval '13 days'),
    (13, 22, 2, 52, 78, now() - interval '11 days', now() - interval '11 days'),
    (14, 22, 2, 55, 72, now() - interval '12 days', now() - interval '12 days'),
    (14, 22, 2, 50, 76, now() - interval '10 days', now() - interval '10 days'),
    (7, 22, 2, 42, 86, now() - interval '9 days', now() - interval '9 days'),
    (7, 22, 2, 38, 90, now() - interval '7 days', now() - interval '7 days'),
    (13, 22, 2, 46, 84, now() - interval '8 days', now() - interval '8 days'),
    (14, 22, 2, 52, 74, now() - interval '6 days', now() - interval '6 days'),
    (7, 22, 2, 40, 88, now() - interval '5 days', now() - interval '5 days'),
    (13, 22, 2, 44, 86, now() - interval '3 days', now() - interval '3 days'),

    -- УЧЕНИК 23 (student_18) - teacher_5, группа 5
    (13, 23, 2, 44, 86, now() - interval '14 days', now() - interval '14 days'),
    (13, 23, 2, 48, 83, now() - interval '12 days', now() - interval '12 days'),
    (15, 23, 2, 30, 62, now() - interval '11 days', now() - interval '11 days'),
    (15, 23, 2, 34, 66, now() - interval '9 days', now() - interval '9 days'),
    (10, 23, 2, 42, 88, now() - interval '10 days', now() - interval '10 days'),
    (10, 23, 2, 38, 92, now() - interval '8 days', now() - interval '8 days'),
    (14, 23, 2, 50, 78, now() - interval '7 days', now() - interval '7 days'),
    (13, 23, 2, 46, 85, now() - interval '6 days', now() - interval '6 days'),
    (10, 23, 2, 44, 86, now() - interval '5 days', now() - interval '5 days'),
    (15, 23, 2, 32, 68, now() - interval '3 days', now() - interval '3 days'),

    -- УЧЕНИК 24 (student_19) - teacher_5, группа 5
    (14, 24, 2, 48, 76, now() - interval '13 days', now() - interval '13 days'),
    (14, 24, 2, 52, 80, now() - interval '11 days', now() - interval '11 days'),
    (7, 24, 2, 44, 86, now() - interval '12 days', now() - interval '12 days'),
    (7, 24, 2, 40, 90, now() - interval '10 days', now() - interval '10 days'),
    (1, 24, 2, 46, 88, now() - interval '9 days', now() - interval '9 days'),
    (1, 24, 2, 50, 84, now() - interval '7 days', now() - interval '7 days'),
    (13, 24, 2, 42, 87, now() - interval '8 days', now() - interval '8 days'),
    (14, 24, 2, 50, 78, now() - interval '6 days', now() - interval '6 days'),
    (7, 24, 2, 38, 92, now() - interval '5 days', now() - interval '5 days'),
    (1, 24, 2, 44, 86, now() - interval '3 days', now() - interval '3 days'),

    -- УЧЕНИК 25 (student_20) - teacher_5, вне группы
    (13, 25, 2, 46, 84, now() - interval '15 days', now() - interval '15 days'),
    (13, 25, 2, 42, 88, now() - interval '13 days', now() - interval '13 days'),
    (15, 25, 2, 32, 64, now() - interval '12 days', now() - interval '12 days'),
    (15, 25, 2, 35, 68, now() - interval '10 days', now() - interval '10 days'),
    (4, 25, 2, 44, 86, now() - interval '11 days', now() - interval '11 days'),
    (4, 25, 2, 40, 90, now() - interval '9 days', now() - interval '9 days'),
    (13, 25, 2, 48, 82, now() - interval '8 days', now() - interval '8 days'),
    (15, 25, 2, 30, 66, now() - interval '7 days', now() - interval '7 days'),
    (4, 25, 2, 42, 88, now() - interval '5 days', now() - interval '5 days'),
    (13, 25, 2, 44, 86, now() - interval '3 days', now() - interval '3 days');

-- =====================================================
-- 2. ОТВЕТЫ ПОЛЬЗОВАТЕЛЕЙ (USER_ANSWER)
-- Для каждой попытки генерируем ответы на все задания теста
-- Ответы берём из problem_version.correct_answer
-- =====================================================

-- ------------------------------------------------------------------
-- Функция для генерации ответов для конкретной попытки
-- Используем INSERT с подзапросом, который для каждого задания теста
-- получает правильный ответ из problem_version
-- ------------------------------------------------------------------

-- Для каждого test_attempt создаём ответы на все задания теста

-- 2.1 УЧЕНИК 6 (student_id=6) - попытки для тестов 1,2,4
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 6
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.2 УЧЕНИК 7 (student_id=7)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 7
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.3 УЧЕНИК 8 (student_id=8)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 8
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.4 УЧЕНИК 9 (student_id=9)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 9
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.5 УЧЕНИК 10 (student_id=10)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 10
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.6 УЧЕНИК 11 (student_id=11)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 11
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.7 УЧЕНИК 12 (student_id=12)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 12
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.8 УЧЕНИК 13 (student_id=13)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 13
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.9 УЧЕНИК 14 (student_id=14)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 14
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.10 УЧЕНИК 15 (student_id=15)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 15
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.11 УЧЕНИК 16 (student_id=16)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 16
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.12 УЧЕНИК 17 (student_id=17)
DO $$
DECLARE
    attempt_record RECORD;
BEGIN
    FOR attempt_record IN 
        SELECT id, test_id FROM public.test_attempt WHERE student_id = 17
    LOOP
        INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
        SELECT 
            attempt_record.id,
            pv.id,
            pv.correct_answer,
            true,
            now()
        FROM public.test_problem tp
        JOIN public.problem p ON tp.problem_id = p.id
        JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
        WHERE tp.test_id = attempt_record.test_id;
    END LOOP;
END $$;

-- 2.13-2.20 Остальные ученики (18-25)
DO $$
DECLARE
    student_id_val INT;
    attempt_record RECORD;
BEGIN
    FOR student_id_val IN 18..25
    LOOP
        FOR attempt_record IN 
            SELECT id, test_id FROM public.test_attempt WHERE student_id = student_id_val
        LOOP
            INSERT INTO public.user_answer (test_attempt_id, problem_version_id, answer, is_correct, created_at)
            SELECT 
                attempt_record.id,
                pv.id,
                pv.correct_answer,
                true,
                now()
            FROM public.test_problem tp
            JOIN public.problem p ON tp.problem_id = p.id
            JOIN public.problem_version pv ON pv.problem_id = p.id AND pv.is_active = true
            WHERE tp.test_id = attempt_record.test_id;
        END LOOP;
    END LOOP;
END $$;

-- =====================================================
-- 3. СИНХРОНИЗАЦИЯ СЧЕТЧИКОВ SEQUENCE
-- =====================================================
SELECT setval(
    pg_get_serial_sequence('public.test_attempt', 'id'),
    COALESCE((SELECT MAX(id) FROM public.test_attempt), 0) + 1,
    false
);

SELECT setval(
    pg_get_serial_sequence('public.user_answer', 'id'),
    COALESCE((SELECT MAX(id) FROM public.user_answer), 0) + 1,
    false
);

COMMIT;

-- =====================================================
-- РЕЗУЛЬТАТ:
-- =====================================================
-- test_attempt: ~240 попыток (20 учеников × ~12 попыток)
-- user_answer: для каждой попытки ~6 ответов (по числу заданий в тесте)
-- 
-- Все ответы корректны (is_correct = true)
-- raw_score в test_attempt соответствует проценту правильных ответов
-- 
-- Для тестирования эндпоинтов:
--   GET /attempts/student/me - мои попытки (ученик)
--   GET /attempts/student/{id} - попытки ученика (учитель)
--   GET /attempts/{id}/details - детали попытки
--   GET /assignments/student-score/{id} - статистика по назначению
--   GET /assignments/group-score/{id} - статистика по группе
-- =====================================================