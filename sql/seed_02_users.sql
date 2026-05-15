-- =====================================================
-- SEED: Пользователи (USER)
-- Учителя (id 1-5), Ученики (id 6-25), Админ (id 26)
-- С синхронизацией счетчика последовательности
-- Идемпотентный скрипт (можно запускать многократно)
-- =====================================================

BEGIN;

-- ------------------------------------------------------------------
-- Хеш пароля для всех пользователей:
-- AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==
-- ------------------------------------------------------------------

-- ------------------------------------------------------------------
-- 1. УЧИТЕЛЯ (role = 2) - id: 1, 2, 3, 4, 5
-- ------------------------------------------------------------------
INSERT INTO public."user" (
    id, 
    username, 
    password_hash, 
    email, 
    role, 
    is_email_verified,
    is_deleted, 
    created_at, 
    last_update_at
)
VALUES
    (1, 'teacher_1', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'teacher1@teacher1.com', 2, true, false, now(), now()),
    (2, 'teacher_2', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'teacher2@teacher2.com', 2, true, false, now(), now()),
    (3, 'teacher_3', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'teacher3@teacher3.com', 2, true, false, now(), now()),
    (4, 'teacher_4', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'teacher4@teacher4.com', 2, true, false, now(), now()),
    (5, 'teacher_5', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'teacher5@teacher5.com', 2, true, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    username = EXCLUDED.username,
    password_hash = EXCLUDED.password_hash,
    email = EXCLUDED.email,
    role = EXCLUDED.role,
    is_email_verified = EXCLUDED.is_email_verified,
    is_deleted = EXCLUDED.is_deleted,
    last_update_at = now()
WHERE public."user".username IS DISTINCT FROM EXCLUDED.username
   OR public."user".email IS DISTINCT FROM EXCLUDED.email;

-- ------------------------------------------------------------------
-- 2. УЧЕНИКИ (role = 1) - id: 6, 7, 8, ..., 25 (всего 20 учеников)
-- ------------------------------------------------------------------
INSERT INTO public."user" (
    id, 
    username, 
    password_hash, 
    email, 
    role, 
    is_email_verified,
    is_deleted, 
    created_at, 
    last_update_at
)
VALUES
    (6,  'student_1',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student1@student1.com',  1, true, false, now(), now()),
    (7,  'student_2',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student2@student2.com',  1, true, false, now(), now()),
    (8,  'student_3',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student3@student3.com',  1, true, false, now(), now()),
    (9,  'student_4',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student4@student4.com',  1, true, false, now(), now()),
    (10, 'student_5',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student5@student5.com',  1, true, false, now(), now()),
    (11, 'student_6',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student6@student6.com',  1, true, false, now(), now()),
    (12, 'student_7',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student7@student7.com',  1, true, false, now(), now()),
    (13, 'student_8',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student8@student8.com',  1, true, false, now(), now()),
    (14, 'student_9',  'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student9@student9.com',  1, true, false, now(), now()),
    (15, 'student_10', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student10@student10.com', 1, true, false, now(), now()),
    (16, 'student_11', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student11@student11.com', 1, true, false, now(), now()),
    (17, 'student_12', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student12@student12.com', 1, true, false, now(), now()),
    (18, 'student_13', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student13@student13.com', 1, true, false, now(), now()),
    (19, 'student_14', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student14@student14.com', 1, true, false, now(), now()),
    (20, 'student_15', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student15@student15.com', 1, true, false, now(), now()),
    (21, 'student_16', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student16@student16.com', 1, true, false, now(), now()),
    (22, 'student_17', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student17@student17.com', 1, true, false, now(), now()),
    (23, 'student_18', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student18@student18.com', 1, true, false, now(), now()),
    (24, 'student_19', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student19@student19.com', 1, true, false, now(), now()),
    (25, 'student_20', 'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 'student20@student20.com', 1, true, false, now(), now())
ON CONFLICT (id) DO UPDATE SET
    username = EXCLUDED.username,
    password_hash = EXCLUDED.password_hash,
    email = EXCLUDED.email,
    role = EXCLUDED.role,
    is_email_verified = EXCLUDED.is_email_verified,
    is_deleted = EXCLUDED.is_deleted,
    last_update_at = now()
WHERE public."user".username IS DISTINCT FROM EXCLUDED.username
   OR public."user".email IS DISTINCT FROM EXCLUDED.email;

-- ------------------------------------------------------------------
-- 3. АДМИНИСТРАТОР (role = 3) - id: 26
-- ------------------------------------------------------------------
INSERT INTO public."user" (
    id, 
    username, 
    password_hash, 
    email, 
    role, 
    is_email_verified,
    is_deleted, 
    created_at, 
    last_update_at
)
VALUES (
    26, 
    'admin_1', 
    'AQAAAAIAAYagAAAAEFNye2BLDyfWL6ud1n6a5tk50P++C3yBKeTz1OTOzJGFDMISwzvyfiAlMDAW0h8bbg==', 
    'admin1@admin1.com', 
    3, 
    true, 
    false, 
    now(), 
    now()
)
ON CONFLICT (id) DO UPDATE SET
    username = EXCLUDED.username,
    password_hash = EXCLUDED.password_hash,
    email = EXCLUDED.email,
    role = EXCLUDED.role,
    is_email_verified = EXCLUDED.is_email_verified,
    is_deleted = EXCLUDED.is_deleted,
    last_update_at = now()
WHERE public."user".username IS DISTINCT FROM EXCLUDED.username
   OR public."user".email IS DISTINCT FROM EXCLUDED.email;

-- ------------------------------------------------------------------
-- 4. СИНХРОНИЗАЦИЯ СЧЕТЧИКА SEQUENCE
--    Устанавливаем следующее значение на max(id) + 1
-- ------------------------------------------------------------------
SELECT setval(
    pg_get_serial_sequence('"user"', 'id'),
    COALESCE((SELECT MAX(id) FROM public."user"), 0) + 1,
    false
);

COMMIT;

-- =====================================================
-- РЕЗУЛЬТАТ:
-- =====================================================
-- Учителя:    id 1-5   (teacher_1 ... teacher_5)
-- Ученики:    id 6-25  (student_1 ... student_20) 
-- Админ:      id 26    (admin_1)
--
-- Все используют один и тот же хеш пароля
-- Счетчик последовательности синхронизирован
-- =====================================================